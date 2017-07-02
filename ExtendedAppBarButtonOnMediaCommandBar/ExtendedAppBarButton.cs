using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace ExtendedAppBarButtonOnMediaCommandBar
{
    [TemplatePart(Name = "FocusVisual", Type = typeof(Rectangle))]
    public class ExtendedAppBarButton : AppBarButton
    {
        private Rectangle _focusVisual;
        private bool _ignoreGlobalSpaceKeyPress;

        public ExtendedAppBarButton()
        {
            DefaultStyleKey = typeof(ExtendedAppBarButton);

            AddHandler(KeyDownEvent, new KeyEventHandler(OnKeyDown), true);
            AddHandler(KeyUpEvent, new KeyEventHandler(OnKeyUp), true);

            GotFocus += OnGotFocus;
            LostFocus += OnLostFocus;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _focusVisual = (Rectangle)GetTemplateChild("FocusVisual");
        }

        public bool IsPlayPauseButton
        {
            get => (bool)GetValue(IsPlayPauseButtonProperty);
            set => SetValue(IsPlayPauseButtonProperty, value);
        }
        public static readonly DependencyProperty IsPlayPauseButtonProperty = DependencyProperty.Register(
            "IsPlayPauseButton", typeof(bool), typeof(ExtendedAppBarButton), new PropertyMetadata(false,
                (s, e) =>
                {
                    var self = (ExtendedAppBarButton)s;
                    var allowSpace = (bool)e.NewValue;

                    if (allowSpace)
                    {
                        //self.AllowFocusOnInteraction = true;
                        Window.Current.CoreWindow.KeyDown += self.OnWindowKeyDown;
                    }
                    else
                    {
                        //self.AllowFocusOnInteraction = false;
                        Window.Current.CoreWindow.KeyDown -= self.OnWindowKeyDown;
                    }
                }));

        private void OnWindowKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (!_ignoreGlobalSpaceKeyPress && args.VirtualKey == VirtualKey.Space)
            {
                var ap = new ButtonAutomationPeer(this);
                var ip = ap.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                ip?.Invoke();
            }
        }

        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!IsPlayPauseButton && e.Key == VirtualKey.Space)
            {
                IsEnabled = false;
            }
            else if (IsPlayPauseButton && e.Key == VirtualKey.Space)
            {
                _ignoreGlobalSpaceKeyPress = true;
            }
        }

        private void OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (!IsPlayPauseButton && e.Key == VirtualKey.Space)
            {
                IsEnabled = true;
            }

            _ignoreGlobalSpaceKeyPress = false;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (AllowFocusWhenDisabled)
            {
                _focusVisual.Opacity = 1;
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (AllowFocusWhenDisabled)
            {
                _focusVisual.Opacity = 0;
            }
        }
    }
}
