using PrismWPFSample.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace PrismWPFSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //アプリ全体で共有したいオブジェクトをシングルトンで生成する。
            //モジュール側ではViewModelのコンストラクターの引数で受け取る
            containerRegistry.RegisterSingleton<CommonModels.CModel>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            //Modulesフォルダーにあるモジュールを読み込む。
            //読み込まれる先はモジュール側に書かれたRegionNameの場所
            return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        }

        //2重起動禁止
        // App.xamlでプロパティーを開いてイベントハンドラーを追加
        private static System.Threading.Mutex mutex;

        private void PrismApplication_Startup(object sender, StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(false, "Mutexの名称");

            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("起動済み");
                mutex.Close();
                mutex = null;

                this.Shutdown();
            }
        }

        private void PrismApplication_Exit(object sender, ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
            }
        }
    }
}
