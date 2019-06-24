using Module3.Views;
using Module3.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CommonModels;

namespace Module3
{
    public class M3Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //ViewA を ContentRegion に入れる
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M3));

            //ViewModelのTitleにTabItemのHeaderの文言を入れておく
            //MainWindow側のXAMLのTabControlのスタイル設定でHeaderにTitleをバインドしておく
            //IRegion region = regionManager.Regions["ContentRegion"];
            //var t = containerProvider.Resolve<ViewA>();
            //(t.DataContext as ViewAViewModel).Title = CModel.GetLocalizedValue<string>("TITLEM3");
            //region.Add(t);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}