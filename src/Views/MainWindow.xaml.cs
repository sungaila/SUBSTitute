using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.ImmersiveDarkMode.WinUI;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using WinUIEx;
using Icon = System.Drawing.Icon;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class MainWindow : WindowEx
    {
        internal NavigationView NavigationView => NavView;

        public MainWindow()
        {
            InitializeComponent();
            SetupTitleBar();

            if (Content is FrameworkElement frameworkElement)
            {
                frameworkElement.ActualThemeChanged += (_, _) => this.SetTitlebarTheme();
                if (Enum.TryParse<ElementTheme>(App.LocalSettings.Values[nameof(App.RequestedAppTheme)]?.ToString(), out var theme))
                {
                    frameworkElement.RequestedTheme = theme;
                }

                var viewModel = new MainViewModel();
                viewModel.Mapping.QueryDrives.Execute(viewModel.Mapping);
                frameworkElement.DataContext = viewModel;
            }

            if (App.IsElevated)
            {
                Title += $" ({App.ResourceLoader.GetString("Administrator")})";
                AppTitleBar.Subtitle = "Administrator";
            }

            if (App.LocalSettings.Values["MainWindowWidth"] is double width && App.LocalSettings.Values["MainWindowHeight"] is double height)
            {
                try
                {
                    this.SetWindowSize(width, height);
                }
                catch { }
            }

            if (App.LocalSettings.Values["MainWindowX"] is int x && App.LocalSettings.Values["MainWindowY"] is int y)
            {
                try
                {
                    this.Move(x, y);
                }
                catch { }
            }

            if (App.LocalSettings.Values["MainWindowWindowState"] is int savedWindowState)
            {
                var windowState = (WindowState)savedWindowState;

                if (windowState == WindowState.Maximized)
                    WindowState = WindowState.Maximized;
            }

            if (App.LocalSettings.Values["NavViewPaneOpen"] is bool navPaneOpen && !navPaneOpen)
            {
                NavView.IsPaneOpen = false;
            }

            UpdatePatternCanvasVisibility();
        }

        private void SetupTitleBar()
        {
            this.InitTitlebarTheme();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            NavigationView_ActualThemeChanged(null!, null!);
            NavView.ActualThemeChanged += NavigationView_ActualThemeChanged;

            var exePath = Environment.ProcessPath!;
            var icon = Icon.ExtractAssociatedIcon(exePath)!;
            AppWindow.SetIcon(Win32Interop.GetIconIdFromIcon(icon.Handle));
        }

        private void NavigationView_ActualThemeChanged(FrameworkElement sender, object args)
        {
            AppWindow.TitleBar.ButtonForegroundColor = NavView.ActualTheme == ElementTheme.Dark
                ? Color.FromArgb(255, 242, 242, 242)
                : Color.FromArgb(255, 23, 23, 23);
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is not NavigationView navigationView)
                return;

            DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
            {
                navigationView.SelectedItem = navigationView.MenuItems.First();
            });
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type? pageType = null;

            if (args.IsSettingsSelected)
            {
                pageType = typeof(SettingsView);
            }
            else if (args.SelectedItemContainer?.Tag is Type type)
            {
                pageType = type;
            }

            ContentFrame.Navigate(
                pageType,
                null,
                args.RecommendedNavigationTransitionInfo);
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
                ContentFrame.GoBack();
        }

        public void ShowInfoBar(string message, InfoBarSeverity severity)
        {
            InfoBar.Severity = severity;
            InfoBar.Message = message;
            InfoBar.IsOpen = true;
        }

#pragma warning disable CA1822
        private void WindowEx_WindowStateChanged(object sender, WindowState e)
#pragma warning restore CA1822
        {
            if (e == WindowState.Minimized)
                return;

            App.LocalSettings.Values["MainWindowWindowState"] = (int)e;
        }

        private void WindowEx_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            if (!Visible || WindowState != WindowState.Normal)
                return;

            App.LocalSettings.Values["MainWindowWidth"] = args.Size.Width;
            App.LocalSettings.Values["MainWindowHeight"] = args.Size.Height;
        }

        private void WindowEx_PositionChanged(object sender, PointInt32 e)
        {
            if (!Visible || WindowState != WindowState.Normal)
                return;

            // WindowEx_PositionChanged is called before WindowEx_WindowStateChanged
            // so we have to call Win32 APIs to check the current window state
            WINDOWPLACEMENT windowplacement = new();
            PInvoke.GetWindowPlacement((HWND)this.GetWindowHandle(), ref windowplacement);

            if (windowplacement.showCmd == SHOW_WINDOW_CMD.SW_SHOWMAXIMIZED)
                return;

            App.LocalSettings.Values["MainWindowX"] = e.X;
            App.LocalSettings.Values["MainWindowY"] = e.Y;
        }

        CanvasBitmap? _canvasBitmap;

        private void PatternCanvas_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            static async Task CreateResources(MainWindow @this, CanvasControl sender)
            {
                var uri = new Uri("ms-appx:///Assets/BG pattern.png");

                if (await StorageFile.GetFileFromApplicationUriAsync(uri) is not StorageFile file)
                    return;

                using var pattern = await file.OpenStreamForReadAsync();
                using var ms = new MemoryStream((int)pattern.Length);
                pattern.CopyTo(ms);

                using var stream = new InMemoryRandomAccessStream();
                stream.WriteAsync(ms.ToArray().AsBuffer()).GetAwaiter().GetResult();
                stream.Seek(0);

                @this._canvasBitmap = await CanvasBitmap.LoadAsync(sender, stream);
            }

            args.TrackAsyncAction(CreateResources(this, sender).AsAsyncAction());
        }

        private void PatternCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (_canvasBitmap == null || !GetPatternCanvasVisible())
                return;

            using var list = new CanvasCommandList(sender);
            using var session = list.CreateDrawingSession();
            session.DrawImage(_canvasBitmap, 0, 0, _canvasBitmap.Bounds, 1f, CanvasImageInterpolation.NearestNeighbor);

            using var tile = new TileEffect
            {
                Source = list,
                CacheOutput = true,
                SourceRectangle = _canvasBitmap.Bounds
            };

            using var rotate = new Transform2DEffect
            {
                Source = tile,
                CacheOutput = true,
                TransformMatrix = Matrix3x2.CreateTranslation(Vector2.Zero) * Matrix3x2.CreateRotation(-0.0872665f, Vector2.Zero),
                InterpolationMode = CanvasImageInterpolation.HighQualityCubic
            };

            if (sender == PatternCanvasVertical)
            {
                using var move = new Transform2DEffect
                {
                    Source = rotate,
                    CacheOutput = true,
                    TransformMatrix = Matrix3x2.CreateTranslation(Vector2.Zero) * Matrix3x2.CreateTranslation(0, (float)-PatternCanvasHorizontal.ActualHeight),
                    InterpolationMode = CanvasImageInterpolation.HighQualityCubic
                };

                args.DrawingSession.DrawImage(move);
            }
            else
            {
                args.DrawingSession.DrawImage(rotate);
            }
        }

        public static bool GetPatternCanvasVisible() => App.LocalSettings.Values["RenderBackgroundPattern"] is not bool render || render;

        public static void SetPatternCanvasVisible(bool value)
        {
            App.LocalSettings.Values["RenderBackgroundPattern"] = value;
            App.MainWindow?.UpdatePatternCanvasVisibility();
        }

        private void UpdatePatternCanvasVisibility()
        {
            PatternCanvasHorizontal.Visibility = GetPatternCanvasVisible()
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void NavView_PaneOpening(NavigationView sender, object args) => App.LocalSettings.Values["NavViewPaneOpen"] = true;

        private void NavView_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args) => App.LocalSettings.Values["NavViewPaneOpen"] = false;

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PatternCanvasVertical.Width = NavigationView.ActualWidth - ContentFrame.ActualWidth;
        }
    }
}