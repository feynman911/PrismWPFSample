using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CommonModels.Controls
{
    /// <summary>
    /// LedControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LedControl : UserControl
    {
        public LedControl()
        {
            InitializeComponent();

            Light.Fill = LedOffBrush;
            LightOn = false;
            FlickerOn = false;

            board = (Storyboard)FindResource("FlickerStory");
        }

        Storyboard board;

        #region ******************************* LedLightBrush
        [Category("LED")]
        [Description("Led Light Brush")]
        public Brush LedLightBrush
        {
            get { return (Brush)this.GetValue(LedLightColorProperty); }
            set { this.SetValue(LedLightColorProperty, value); }
        }

        public static readonly DependencyProperty LedLightColorProperty =
            DependencyProperty.Register("LedLightBrush", typeof(Brush),
                typeof(LedControl),
                new FrameworkPropertyMetadata(Brushes.Red,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    LedLightColorChangeFunc));

        static void LedLightColorChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (Brush)e.OldValue;
            var nf = (Brush)e.NewValue;
            var obj = (LedControl)target;

            if (obj.LightOn) obj.Light.Fill = nf;
        }
        #endregion


        #region ******************************* LedOffBrush
        [Category("LED")]
        [Description("Led off Brush")]
        public Brush LedOffBrush
        {
            get { return (Brush)this.GetValue(LedOffBrushProperty); }
            set { this.SetValue(LedOffBrushProperty, value); }
        }

        public static readonly DependencyProperty LedOffBrushProperty =
            DependencyProperty.Register("LedOffBrush", typeof(Brush),
                typeof(LedControl),
                new FrameworkPropertyMetadata(Brushes.Gray,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion


        #region ******************************* LightOn
        [Category("LED")]
        [Description("Led Light On")]
        public bool LightOn
        {
            get { return (bool)this.GetValue(LightOnProperty); }
            set { this.SetValue(LightOnProperty, value); }
        }

        public static readonly DependencyProperty LightOnProperty =
            DependencyProperty.Register("LightOn", typeof(bool),
                typeof(LedControl),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    LightOnChangeFunc));

        static void LightOnChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (bool)e.OldValue;
            var nf = (bool)e.NewValue;
            var obj = (LedControl)target;

            if (nf)
            {
                obj.Light.Fill = obj.LedLightBrush;
                if (obj.FlickerOn)
                {
                    obj.board.RepeatBehavior = RepeatBehavior.Forever;
                    obj.board.Begin();
                    obj.board.SetSpeedRatio(obj.FlickerRatio);
                }
            }
            else
            {
                obj.Light.Fill = obj.LedOffBrush;
                obj.board.Stop();
            }
        }
        #endregion

        #region ******************************* Title
        [Category("LED")]
        [Description("Led Title")]
        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string),
                typeof(LedControl),
                new FrameworkPropertyMetadata("LedControl",
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    TitleChangeFunc));

        static void TitleChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (string)e.OldValue;
            var nf = (string)e.NewValue;
            var obj = (LedControl)target;

            obj.title.Text = nf;
        }
        #endregion

        #region ******************************* FlickerOn
        [Category("LED")]
        [Description("Flicker On")]
        public bool FlickerOn
        {
            get { return (bool)this.GetValue(FlickerOnProperty); }
            set { this.SetValue(FlickerOnProperty, value); }
        }

        public static readonly DependencyProperty FlickerOnProperty =
            DependencyProperty.Register("FlickerOn", typeof(bool),
                typeof(LedControl),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    FlickerOnChangeFunc));

        static void FlickerOnChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (bool)e.OldValue;
            var nf = (bool)e.NewValue;
            var obj = (LedControl)target;

            if(nf && obj.LightOn)
            {
                obj.board.RepeatBehavior = RepeatBehavior.Forever;
                obj.board.Begin();
                obj.board.SetSpeedRatio(obj.FlickerRatio);
            }
            if (!nf)
            {
                obj.board.Stop();
            }
        }
        #endregion


        #region ******************************* FlickerRatio
        [Category("LED")]
        [Description("Flicker Ratio")]
        public double FlickerRatio
        {
            get { return (double)this.GetValue(FlickerRatioProperty); }
            set { this.SetValue(FlickerRatioProperty, value); }
        }

        public static readonly DependencyProperty FlickerRatioProperty =
            DependencyProperty.Register("FlickerRatio", typeof(double),
                typeof(LedControl),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    FlickerRatioChangeFunc,
                    FlickerRatioCoerceFunc));

        static void FlickerRatioChangeFunc(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var of = (double)e.OldValue;
            var nf = (double)e.NewValue;
            var obj = (LedControl)target;

            obj.board.SetSpeedRatio(nf);
        }

        static object FlickerRatioCoerceFunc(DependencyObject target, object baseValue)
        {
            var obj = (LedControl)target;
            var val = (double)baseValue;

            return val;
        }
        #endregion


    }
}
