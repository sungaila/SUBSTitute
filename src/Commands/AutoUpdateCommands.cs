using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.SUBSTitute.Helper;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using Windows.Win32;
using Windows.Win32.System.Recovery;

namespace Sungaila.SUBSTitute.Commands
{
    public static class AutoUpdateCommands
    {
        public static readonly IAsyncRelayCommand<SettingsViewModel> CheckForUpdates = new AsyncRelayCommand<SettingsViewModel>(async parameter =>
        {
            var result = await AppInstallerHelper.GetUpdateAvailabilityAsync();

            switch (result)
            {
                case PackageUpdateAvailability.Unknown:
                    App.MainWindow?.ShowInfoBar(App.ResourceLoader.GetString("PackageUpdateAvailability+Unknown"), InfoBarSeverity.Error);
                    break;
                case PackageUpdateAvailability.NoUpdates:
                    App.MainWindow?.ShowInfoBar(App.ResourceLoader.GetString("PackageUpdateAvailability+NoUpdates"), InfoBarSeverity.Success);
                    break;
                case PackageUpdateAvailability.Available:
                    App.MainWindow?.ShowInfoBar(App.ResourceLoader.GetString("PackageUpdateAvailability+Available"), InfoBarSeverity.Informational);
                    break;
                case PackageUpdateAvailability.Required:
                    App.MainWindow?.ShowInfoBar(App.ResourceLoader.GetString("PackageUpdateAvailability+Required"), InfoBarSeverity.Warning);
                    break;
                case PackageUpdateAvailability.Error:
                    App.MainWindow?.ShowInfoBar(App.ResourceLoader.GetString("PackageUpdateAvailability+Error"), InfoBarSeverity.Error);
                    break;
            }

            if (result != PackageUpdateAvailability.Available && result != PackageUpdateAvailability.Required)
                return;

            if (Package.Current.GetAppInstallerInfo() is not AppInstallerInfo info || info.Uri is null)
            {
                App.MainWindow?.ShowInfoBar(App.ResourceLoader.GetString("PackageUpdateAvailability+Error"), InfoBarSeverity.Error);
                return;
            }

            var dialog = new ContentDialog
            {
                XamlRoot = App.MainWindow?.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = App.ResourceLoader.GetString("UpdateAvailableContentDialogTitle"),
                PrimaryButtonText = App.ResourceLoader.GetString("ContentDialogInstall"),
                CloseButtonText = App.ResourceLoader.GetString("ContentDialogCancel"),
                DefaultButton = ContentDialogButton.Primary,
                Content = App.ResourceLoader.GetString("UpdateAvailableContentDialogContent"),
                RequestedTheme = App.RequestedAppTheme,
                FlowDirection = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft
                    ? FlowDirection.RightToLeft
                    : FlowDirection.LeftToRight
            };

            if (await dialog.ShowAsync() != ContentDialogResult.Primary)
                return;

            PInvoke.RegisterApplicationRestart(
                null,
                REGISTER_APPLICATION_RESTART_FLAGS.RESTART_NO_CRASH | REGISTER_APPLICATION_RESTART_FLAGS.RESTART_NO_HANG | REGISTER_APPLICATION_RESTART_FLAGS.RESTART_NO_REBOOT);

            var progress = new PackageManager().RequestAddPackageByAppInstallerFileAsync(
                info.Uri,
                AddPackageByAppInstallerOptions.ForceTargetAppShutdown,
                null);

            progress.Progress = (status, percent) =>
            {
                App.MainWindow?.DispatcherQueue.TryEnqueue(() =>
                {
                    if (status.Status == Windows.Foundation.AsyncStatus.Error)
                    {
                        App.MainWindow?.ShowInfoBar(status.ErrorCode.Message, InfoBarSeverity.Error);
                    }
                    else
                    {
                        App.MainWindow?.ShowInfoBar(string.Format(App.ResourceLoader.GetString("DeploymentProgress"), percent.percentage.ToString("P0")), InfoBarSeverity.Informational);
                    }
                });
            };

            await progress;
        });
    }
}