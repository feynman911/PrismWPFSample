﻿using CommonModels;
using Module1.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;
using Prism.Regions;

namespace Module1.ViewModels
{
    public class M1ViewModel : BindableBase,INavigationAware
    {
        public M1ViewModel(){  }

        IEventAggregator _ea;
        private readonly IRegionManager _regionManager;

        public M1ViewModel(CModel cModel, IEventAggregator ea, IRegionManager regionManager)
        {
            MyCModel = cModel;
            _ea = ea;

            //モデルの生成
            MyViewAModel = new M1Model(MyCModel, _ea);
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEM1");
            _regionManager = regionManager;
        }

        private M1Model myViewAModel;
        /// <summary>
        /// このモジュールのモデル
        /// </summary>
        public M1Model MyViewAModel
        {
            get { return myViewAModel; }
            set { SetProperty(ref myViewAModel, value); }
        }

        private string _message = "Module1";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title;
        /// <summary>
        /// TabItemのHeaderにバインドされる
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// 言語設定が変わった時の処理
        /// </summary>
        public void ChangedLang()
        {
            Title = CModel.GetLocalizedValue<string>("TITLEM1");
            HeaderMenu1 = CModel.GetLocalizedValue<string>("M1MENU1");
            HeaderMenu2 = CModel.GetLocalizedValue<string>("M1MENU2");
        }

        private CModel myCModel;
        public CModel MyCModel
        {
            get { return myCModel; }
            set { SetProperty(ref myCModel, value); }
        }

        private ObservableCollection<CultureInfo> _LangList = LocalizeDictionary.Instance.MergedAvailableCultures;
        /// <summary>
        /// コンボボックス用設定されている言語リスト
        /// </summary>
        public ObservableCollection<CultureInfo> LangList
        {
            get { return _LangList; }
            set { SetProperty(ref _LangList, value); }
        }

        private CultureInfo selectedCulture = LocalizeDictionary.Instance.Culture;
        /// <summary>
        /// コンボボックスの選択言語
        /// 変更された時に言語設定変更とその通知イベントを送る
        /// </summary>
        public CultureInfo SelectedCulture
        {
            get { return selectedCulture; }
            set { SetProperty(ref selectedCulture, value);
                LocalizeDictionary.Instance.Culture = (value);
                _ea.GetEvent<LanguageChangeEvent>().Publish();
            }
        }


        private string _headerMenu1 = CModel.GetLocalizedValue<string>("M1MENU1");
        /// <summary>
        /// TabItemのHeaderにバインドされる
        /// </summary>
        public string HeaderMenu1
        {
            get { return _headerMenu1; }
            set { SetProperty(ref _headerMenu1, value); }
        }

        private string _headerMenu2 = CModel.GetLocalizedValue<string>("M1MENU2");
        /// <summary>
        /// TabItemのHeaderにバインドされる
        /// </summary>
        public string HeaderMenu2
        {
            get { return _headerMenu2; }
            set { SetProperty(ref _headerMenu2, value); }
        }


        private DelegateCommand commandMenu1;
        public DelegateCommand CommandMenu1 =>
            commandMenu1 ?? (commandMenu1 = new DelegateCommand(ExecuteCommandMenu1));
        async void ExecuteCommandMenu1()
        {
            _regionManager.RequestNavigate("ContentRegion","M1");
            _ea.GetEvent<MessageSentEvent>().Publish("モジュール１のメニュー１が選択されました");
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("ja-JP");
            _ea.GetEvent<LanguageChangeEvent>().Publish();
            await Task.Delay(2000);
            _ea.GetEvent<MessageSentEvent>().Publish("");
        }

        private DelegateCommand commandMenu2;
        public DelegateCommand CommandMenu2 =>
            commandMenu2 ?? (commandMenu2 = new DelegateCommand(ExecuteCommandMenu2));
        async void ExecuteCommandMenu2()
        {
            _regionManager.RequestNavigate("ContentRegion", "M1");
            _ea.GetEvent<MessageSentEvent>().Publish("モジュール１のメニュー２が選択されました");
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("en-US");
            _ea.GetEvent<LanguageChangeEvent>().Publish();
            await Task.Delay(2000);
            _ea.GetEvent<MessageSentEvent>().Publish("");
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

    }
}
