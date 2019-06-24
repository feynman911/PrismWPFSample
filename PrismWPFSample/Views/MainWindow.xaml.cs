using System.Windows;

namespace PrismWPFSample.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetWindowPosition();
        }

        // アプリ終了時の画面サイズ＆位置を再現する方法

        // Setting.settingsに修飾子publicスコープユーザーで下記項目を作成
        // WindowLeft       double  ユーザー    0.0
        // WindowTop        double  ユーザー    0.0
        // WindowWidth      double  ユーザー    800.0
        // WindowHeight     double  ユーザー    600.0
        // WindowMaximized  double  ユーザー    false

        // ViewでClosingをダブルクリックしてイベントハンドラーWindow_Closingを作成して書く

        /// <summary>
        /// 画面を閉じる時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.WindowMaximized = (WindowState == WindowState.Maximized);
            WindowState = WindowState.Normal; // 最大化解除
            settings.WindowLeft = Left;
            settings.WindowTop = Top;
            settings.WindowWidth = Width;
            settings.WindowHeight = Height;
            settings.Save();
        }

        /// <summary>
        /// 画面の位置・サイズの復元
        /// </summary>
        void SetWindowPosition()
        {
            var settings = Properties.Settings.Default;

            if (settings.WindowLeft >= 0 &&
                (settings.WindowLeft + settings.WindowWidth) < SystemParameters.VirtualScreenWidth)
            { Left = settings.WindowLeft; }

            if (settings.WindowTop >= 0 &&
                (settings.WindowTop + settings.WindowHeight) < SystemParameters.VirtualScreenHeight)
            { Top = settings.WindowTop; }

            if (settings.WindowWidth > 0 &&
                settings.WindowWidth <= SystemParameters.WorkArea.Width)
            { Width = settings.WindowWidth; }

            if (settings.WindowHeight > 0 &&
                settings.WindowHeight <= SystemParameters.WorkArea.Height)
            { Height = settings.WindowHeight; }

            if (settings.WindowMaximized)
            {
                Loaded += (o, e) => WindowState = WindowState.Maximized;
            }
        }
    }
}
