using Module8.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Module8
{
    public class M8Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M8));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}