
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Module1.Views;
using Module1.ViewModels;
using CommonModels;

using System.Globalization;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace Module1
{
    public class M1Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //ViewA を ContentRegion に入れる
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(M1));
            regionManager.RegisterViewWithRegion("MenuRegion", typeof(M1Menu));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ViewModels.M1ViewModel>();
        }
    }
}