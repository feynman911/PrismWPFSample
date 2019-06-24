using Module9.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Module9
{
    public class M9Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M9));
            regionManager.RegisterViewWithRegion("MenuRegion", typeof(M9Menu));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ViewModels.M9ViewModel>();
        }
    }
}