using Module7.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Module7
{
    public class M7Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M7));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}