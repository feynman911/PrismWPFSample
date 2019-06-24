using Module6.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Module6
{
    public class M6Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M6));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}