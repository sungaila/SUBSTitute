using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sungaila.SUBSTitute.Views
{
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();
            
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()?.Location);
            ProductNameTextBlock.Text = fileVersionInfo.ProductName;
            ProductVersionTextBlock.Text = fileVersionInfo.ProductVersion;
            CompanyNameTextBlock.Text = fileVersionInfo.CompanyName;
            LegalCopyrightTextBlock.Text = fileVersionInfo.LegalCopyright;

            AnimationFun? animationFun = null;

            SpriteBaseImage.MouseLeftButtonUp += (s, e) =>
            {
                if (animationFun == null)
                    animationFun = new AnimationFun(SpriteCanvas, SpriteBaseImage, SpriteReactionImage);
                else
                    animationFun.Poke();
                
            };
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(e.Uri.AbsoluteUri);
            }
            catch (Win32Exception)
            {
                Process.Start("explorer", e.Uri.AbsoluteUri);
            }
            e.Handled = true;
        }

        private class AnimationFun
        {
            private static readonly BitmapImage JoySprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/Joy.png"));

            private static readonly BitmapImage AngerSprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/Anger.png"));

            private static readonly BitmapImage ExclamationMarkSprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/ExclamationMark.png"));

            private static readonly BitmapImage QuestionMarkSprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/QuestionMark.png"));

            private static readonly BitmapImage SleepSprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/Sleep.png"));

            private static readonly BitmapImage FishSprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/Fish.png"));

            private static readonly BitmapImage Front0Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/front0.png"));

            private static readonly BitmapImage Front1Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/front1.png"));

            private static readonly BitmapImage Front2Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/front2.png"));

            private static readonly BitmapImage Back0Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/back0.png"));

            private static readonly BitmapImage Back1Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/back1.png"));

            private static readonly BitmapImage Back2Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/back2.png"));

            private static readonly BitmapImage Left0Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/left0.png"));

            private static readonly BitmapImage Left1Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/left1.png"));

            private static readonly BitmapImage Right0Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/right0.png"));

            private static readonly BitmapImage Right1Sprite = new BitmapImage(new Uri("pack://application:,,,/SUBSTitute;component/Misc/right1.png"));

            private readonly Random _random = new Random();

            private readonly DispatcherTimer _animationTimer = new DispatcherTimer();

            private readonly Canvas _canvas;

            private readonly Image _baseSprite;

            private readonly Image _reactionSprite;

            public AnimationFun(Canvas canvas, Image baseSprite, Image reactionSprite)
            {
                _canvas = canvas;
                _baseSprite = baseSprite;
                _reactionSprite = reactionSprite;

                _reactionSprite.Source = ExclamationMarkSprite;

                EventHandler? removeReaction = null;

                removeReaction = (s, e) =>
                {
                    _reactionSprite.Source = null;

                    _animationTimer.Tick -= removeReaction;
                    _animationTimer.Interval = TimeSpan.FromSeconds(0.5);
                    _animationTimer.Tick += AnimationTimer_Tick;
                };

                _animationTimer.Tick += removeReaction;
                _animationTimer.Interval = TimeSpan.FromSeconds(1);
                _animationTimer.Start();
            }

            private enum SpriteActions
            {
                Nothing,

                GoLeft,

                GoRight,

                GoUp,

                GoDown,

                LookLeft,

                LookRight,

                LookUp,

                LookDown,

                ReactionJoy,

                ReactionAnger,

                ReactionExclamationMark,

                ReactionQuestionMark,

                ReactionSleep,

                ReactionFish
            }

            public void Poke()
            {
                if (_isBusy)
                    return;

                _isBusy = true;

                EventHandler? reaction = null;
                reaction = (s, e) =>
                {
                    _isBusy = false;
                    _animationTimer.Tick -= reaction;
                };

                int value = _random.Next(10);

                if (value <= 7)
                {
                    _reactionSprite.Source = QuestionMarkSprite;
                    _baseSprite.Source = Front0Sprite;
                    _animationTimer.Interval = TimeSpan.FromSeconds(1);
                }
                else
                {
                    _reactionSprite.Source = JoySprite;
                    _baseSprite.Source = Front0Sprite;
                    _animationTimer.Interval = TimeSpan.FromSeconds(1);
                }

                _animationTimer.Tick += reaction;
            }

            private DateTime? _lastMovementInput;

            private void AnimationTimer_Tick(object? sender, EventArgs e)
            {
                if (_isBusy)
                    return;

                if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.Right) ||
                    Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.Down))
                {
                    if (_lastMovementInput != null && (DateTime.Now - _lastMovementInput) < TimeSpan.FromSeconds(0.5))
                        return;

                    if (Keyboard.IsKeyDown(Key.Left))
                        Move(SpriteActions.GoLeft, true);
                    else if (Keyboard.IsKeyDown(Key.Right))
                        Move(SpriteActions.GoRight, true);
                    else if (Keyboard.IsKeyDown(Key.Up))
                        Move(SpriteActions.GoUp, true);
                    else if (Keyboard.IsKeyDown(Key.Down))
                        Move(SpriteActions.GoDown, true);

                    _lastMovementInput = DateTime.Now;
                    _animationTimer.Interval = TimeSpan.FromSeconds(0.1);
                    return;
                }

                if (_lastMovementInput != null && (DateTime.Now - _lastMovementInput) < TimeSpan.FromSeconds(5))
                    return;

                _animationTimer.Interval = TimeSpan.FromSeconds(0.5);

                int value = _random.Next(200);

                if (value <= 20)
                {
                    _reactionSprite.Source = null;
                    _baseSprite.Source = Left0Sprite;
                }
                else if (value <= 40)
                {
                    _reactionSprite.Source = null;
                    _baseSprite.Source = Right0Sprite;
                }
                else if (value <= 60)
                {
                    _reactionSprite.Source = null;
                    _baseSprite.Source = Back0Sprite;
                }
                else if (value <= 80)
                {
                    _reactionSprite.Source = null;
                    _baseSprite.Source = Front0Sprite;
                }
                else if (value <= 95)
                {
                    Move(SpriteActions.GoLeft);
                }
                else if (value <= 110)
                {
                    Move(SpriteActions.GoRight);
                }
                else if (value <= 125)
                {
                    Move(SpriteActions.GoUp);
                }
                else if (value <= 140)
                {
                    Move(SpriteActions.GoDown);
                }
                else if (value <= 142)
                {
                    _reactionSprite.Source = JoySprite;
                    _baseSprite.Source = Front0Sprite;
                    _animationTimer.Interval = TimeSpan.FromSeconds(1);
                }
                else if (value <= 144)
                {
                    _reactionSprite.Source = AngerSprite;
                    _baseSprite.Source = Front0Sprite;
                    _animationTimer.Interval = TimeSpan.FromSeconds(1);
                }
                else if (value <= 146)
                {
                    _reactionSprite.Source = ExclamationMarkSprite;
                    _baseSprite.Source = Front0Sprite;
                    _animationTimer.Interval = TimeSpan.FromSeconds(1);
                }
                else if (value <= 148)
                {
                    _reactionSprite.Source = QuestionMarkSprite;
                    _baseSprite.Source = Front0Sprite;
                    _animationTimer.Interval = TimeSpan.FromSeconds(1);
                }
                else if (value <= 150)
                {
                    _reactionSprite.Source = SleepSprite;
                    _baseSprite.Source = Front0Sprite;
                    _animationTimer.Interval = TimeSpan.FromSeconds(1);
                }
                else if (value <= 152)
                {
                    _reactionSprite.Source = FishSprite;
                    _baseSprite.Source = Front0Sprite;
                    _animationTimer.Interval = TimeSpan.FromSeconds(1);
                }
            }

            private bool _isBusy;

            private bool _isAlternateStep;

            private void Move(SpriteActions action, bool wallBumping = false)
            {
                _reactionSprite.Source = null;

                double xMovement = 0;
                double yMovement = 0;

                switch (action)
                {
                    case SpriteActions.GoLeft:
                        xMovement = Canvas.GetLeft(_baseSprite) >= 16 ? -16 : 0;
                        break;
                    case SpriteActions.GoRight:
                        xMovement = Canvas.GetLeft(_baseSprite) <= _canvas.Width - 32 ? 16 : 0;
                        break;
                    case SpriteActions.GoUp:
                        yMovement = Canvas.GetTop(_baseSprite) >= 32 ? -16 : 0;
                        break;
                    case SpriteActions.GoDown:
                        yMovement = Canvas.GetTop(_baseSprite) <= _canvas.Height - 32 ? 16 : 0;
                        break;
                }

                if (!wallBumping && xMovement == 0 && yMovement == 0)
                    return;

                if (action == SpriteActions.GoLeft)
                    _baseSprite.Source = Left1Sprite;
                else if (action == SpriteActions.GoRight)
                    _baseSprite.Source = Right1Sprite;
                else if (action == SpriteActions.GoUp)
                    _baseSprite.Source = _isAlternateStep ? Back1Sprite : Back2Sprite;
                else if (action == SpriteActions.GoDown)
                    _baseSprite.Source = _isAlternateStep ? Front1Sprite : Front2Sprite;

                if (yMovement < 0 || yMovement > 0)
                    _isAlternateStep = !_isAlternateStep;

                DispatcherTimer middleTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(0.3)
                };
                middleTimer.Tick += (s, e) =>
                {
                    if (action == SpriteActions.GoLeft)
                        _baseSprite.Source = Left0Sprite;
                    else if (action == SpriteActions.GoRight)
                        _baseSprite.Source = Right0Sprite;
                    else if (action == SpriteActions.GoUp)
                        _baseSprite.Source = Back0Sprite;
                    else if (action == SpriteActions.GoDown)
                        _baseSprite.Source = Front0Sprite;

                    middleTimer.Stop();
                    _isBusy = false;
                };

                _isBusy = true;
                middleTimer.Start();

                _baseSprite.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(Canvas.GetLeft(_baseSprite), Canvas.GetLeft(_baseSprite) + xMovement, TimeSpan.FromSeconds(0.5)));
                _baseSprite.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(Canvas.GetTop(_baseSprite), Canvas.GetTop(_baseSprite) + yMovement, TimeSpan.FromSeconds(0.5)));

                _reactionSprite.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(Canvas.GetLeft(_reactionSprite), Canvas.GetLeft(_reactionSprite) + xMovement, TimeSpan.FromSeconds(0.5)));
                _reactionSprite.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(Canvas.GetTop(_reactionSprite), Canvas.GetTop(_reactionSprite) + yMovement, TimeSpan.FromSeconds(0.5)));
            }
        }
    }
}
