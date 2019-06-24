using Module5.Views;
using Module5.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CommonModels;

namespace Module5
{
    public class M5Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //M5View を ContentRegion に入れる
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M5));

            //ViewModelのTitleにTabItemのHeaderの文言を入れておく
            //MainWindow側のXAMLのTabControlのスタイル設定でHeaderにTitleをバインドしておく
            //IRegion region = regionManager.Regions["ContentRegion"];
            //var t = containerProvider.Resolve<ViewA>();
            //(t.DataContext as ViewAViewModel).Title = CModel.GetLocalizedValue<string>("TITLEM5");
            //region.Add(t);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}