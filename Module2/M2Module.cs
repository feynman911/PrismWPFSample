using Module2.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Module2
{
    public class M2Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //ViewA を ContentRegion に入れる
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M2));
            regionManager.RegisterViewWithRegion("MenuRegion", typeof(M2Menu));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ViewModels.M2ViewModel>();
        }
    }
}