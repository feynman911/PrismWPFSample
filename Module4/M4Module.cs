using Module4.Views;
using Module4.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CommonModels;

namespace Module4
{
    public class M4Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //ViewA を ContentRegion に入れる
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M4));

            //ViewModelのTitleにTabItemのHeaderの文言を入れておく
            //MainWindow側のXAMLのTabControlのスタイル設定でHeaderにTitleをバインドしておく
            //IRegion region = regionManager.Regions["ContentRegion"];
            //var t = containerProvider.Resolve<ViewA>();
            //(t.DataContext as ViewAViewModel).Title = CModel.GetLocalizedValue<string>("TITLEM4");
            //region.Add(t);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}