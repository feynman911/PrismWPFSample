using CommonModels;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleA.ViewModels
{
    public class MAViewModel : BindableBase, INavigationAware
    {
        IEventAggregator _ea;
        private readonly IRegionManager _regionManager;

        public MAViewModel() {   }

        public MAViewModel(CModel commonModel, IEventAggregator ea, IRegionManager regionManager)
        {
            MyCModel = commonModel;
            _ea = ea;
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEMA");
            _regionManager = regionManager;
        }

        private string _message = "ModuleA";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private CModel myCModel = new CModel();
        public CModel MyCModel
        {
            get { return myCModel; }
            set { SetProperty(ref myCModel, value); }
        }

        /// <summary>
        /// 言語設定が変わった時の処理
        /// </summary>
        public void ChangedLang()
        {
            Title = CModel.GetLocalizedValue<string>("TITLEMA");
        }

        /// <summary>
        /// ナビゲーション移動する時に呼ばれる
        /// </summary>
        /// <param name="navigationContext"></param>
        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        /// <summary>
        /// ナビゲーション対象かどうかを指定する
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// ナビゲーションしてきた時に呼ばれる
        /// </summary>
        /// <param name="navigationContext"></param>
        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private string testData = "test";
        public string TestData
        {
            get { return testData; }
            set { SetProperty(ref testData, value); }
        }



    }
}
