using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace JoystickUserControl
{
    public sealed partial class Joystick : UserControl
    {

        public double XValue
        {
            get { return _joystickX / _controlRadius; }
            private
set { _joystickX = value; }
        }
        private double _joystickX;

        public double YValue
        {
            get
            { return -_joystickY / _controlRadius; }
            private set
            { _joystickY = value; }
        }
        private double _joystickY;

        private bool _controllerPressed;
        private double _controlRadius;
        private ResourceDictionary _resources;

        public Joystick()
        {
            InitializeComponent();
            _resources = JoystickControl.Resources;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ConfigureEvents();
            SetTheme();
            SetDefaults();
        }

        void ConfigureEvents()
        {
            Window.Current.Content.PointerMoved += OnPointerMoved;
            Window.Current.Content.PointerReleased += OnPointerReleased;
        }

        void SetDefaults()
        {
            InnerDiameter = InnerDiameter == 0 ? 60 : InnerDiameter;
            OuterDiameter = OuterDiameter == 0 ? 100 : OuterDiameter;
            Theme = 0;
        }

        void SetTheme()
        {
            switch (Theme)
            {
                case JoystickTheme.AccentTheme:
                    var x = Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
                    InnerFill = InnerFill ?? x;
                    OuterStroke = OuterStroke ?? x;
                    break;

                case JoystickTheme.Dark:
                    OuterFill = OuterFill ?? (Brush)Resources["DarkA"];
                    InnerFill = (Brush)Resources["DarkB"];
                    break;
                case JoystickTheme.Light:
                    OuterFill = OuterFill ?? (Brush)Resources["LightA"];
                    InnerFill = InnerFill ?? (Brush)Resources["LightB"];
                    break;

            }
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs eventArgs)
        {
            if (!_controllerPressed) return;

            var x = eventArgs.GetCurrentPoint(ControlArea).Position.X - _controlRadius;
            var y = eventArgs.GetCurrentPoint(ControlArea).Position.Y - _controlRadius;
            var disp = Math.Sqrt(x * x + y * y);
            if (disp < _controlRadius)
            {

                JoystickTransform.X = XValue = x;
                JoystickTransform.Y = YValue = y;

            }
            else
            {
                JoystickTransform.X = XValue = _controlRadius * (x / disp);       //A*cos(x)
                JoystickTransform.Y = YValue = _controlRadius * (y / disp);       //A*sin(x)
            }

            OnJoystickMoved?.Invoke(this, new EventArgs());

        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _controllerPressed = false;
            JoystickTransform.X = 0;
            JoystickTransform.Y = 0;
            XValue = 0;
            YValue = 0;
            OnJoystickReleased?.Invoke(this, new EventArgs());
        }

        private void Controller_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _controlRadius = OuterDiameter / 2;
            _controllerPressed = true;
            OnJoystickPressed?.Invoke(this, new EventArgs());
        }

        /// <summary>  
        ///  Gets or sets the outer diameter of joystick.  
        /// </summary>  
        public double OuterDiameter
        {
            get
            { return (double)GetValue(OuterDiameterProperty); }
            set
            {
                SetValue(OuterDiameterProperty, value);
            }
        }
        public static readonly DependencyProperty OuterDiameterProperty =
            DependencyProperty.Register("OuterDiameter", typeof(double), typeof(Joystick), null);

        /// <summary> 
        ///  Gets or sets inner diameter of the joystick.  
        /// </summary>
        public double InnerDiameter
        {
            get
            {
                return (double)GetValue(InnerDiameterProperty);
            }
            set
            {
                SetValue(InnerDiameterProperty, value);
            }
        }
        public static readonly DependencyProperty InnerDiameterProperty =
            DependencyProperty.Register("InnerDiameter", typeof(double), typeof(Joystick), null);

        /// <summary>  
        ///  Gets or sets the color of joystick i.e color of inner circle.  
        /// </summary>
        public Brush InnerFill
        {
            get
            {
                return (Brush)GetValue(InnerFillProperty);
            }
            set
            {
                SetValue(InnerFillProperty, value);
            }
        }
        public static readonly DependencyProperty InnerFillProperty =
            DependencyProperty.Register("InnerFill", typeof(Brush), typeof(Joystick), null);

        /// <summary>  
        ///  Gets or sets the inner border color of joystick i.e border of inner circle.  
        /// </summary>
        public Brush InnerStroke
        {
            get
            {
                return (Brush)GetValue(InnerStrokeProperty);
            }
            set
            {
                SetValue(InnerStrokeProperty, value);
            }
        }
        public static readonly DependencyProperty InnerStrokeProperty =
            DependencyProperty.Register("InnerStroke", typeof(Brush), typeof(Joystick), null);

        /// <summary>  
        ///  Gets or sets the color of joystick control area.  
        /// </summary>
        public Brush OuterFill
        {
            get
            {
                return (Brush)GetValue(OuterFillProperty);
            }
            set
            {
                SetValue(OuterFillProperty, value);
            }
        }
        public static readonly DependencyProperty OuterFillProperty =
            DependencyProperty.Register("OuterFill", typeof(Brush), typeof(Joystick), null);

        /// <summary>  
        ///  Gets or sets the border color of joystick control area.  
        /// </summary>
        public Brush OuterStroke
        {
            get
            {
                return (Brush)GetValue(OuterStrokeProperty);
            }
            set
            {
                SetValue(OuterStrokeProperty, value);
            }
        }
        public static readonly DependencyProperty OuterStrokeProperty =
            DependencyProperty.Register("OuterStroke", typeof(Brush), typeof(Joystick), null);



        public enum JoystickTheme
        {
            AccentTheme,
            Dark,
            Light
        }



        public JoystickTheme Theme
        {
            get { return (JoystickTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Theme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(JoystickTheme), typeof(Joystick), new PropertyMetadata(0));


        public event EventHandler OnJoystickPressed;
        public event EventHandler OnJoystickMoved;
        public event EventHandler OnJoystickReleased;


    }
}
