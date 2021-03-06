﻿using CommonModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Threading.Tasks;

namespace Module9.ViewModels
{
    public class M9ViewModel : BindableBase
    {
        IEventAggregator _ea;
        private readonly IRegionManager _regionManager;

        public M9ViewModel() { }

        public M9ViewModel(CModel commonModel, IEventAggregator ea, IRegionManager regionManager)
        {
            MyCModel = commonModel;
            _ea = ea;
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEM9");

            _regionManager = regionManager;
        }

        private string _message = "Module9";
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
            Title = CModel.GetLocalizedValue<string>("TITLEM9");
            HeaderMenu1 = CModel.GetLocalizedValue<string>("M9MENU1");
            HeaderMenu2 = CModel.GetLocalizedValue<string>("M9MENU2");
        }

        //メニュー用

        private string _headerMenu1 = CModel.GetLocalizedValue<string>("M9MENU1");
        /// <summary>
        /// TabItemのHeaderにバインドされる
        /// </summary>
        public string HeaderMenu1
        {
            get { return _headerMenu1; }
            set { SetProperty(ref _headerMenu1, value); }
        }

        private string _headerMenu2 = CModel.GetLocalizedValue<string>("M9MENU2");
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
            MyCModel.CommandEnable = false;
            _regionManager.RequestNavigate("ContentRegion", "M9");
            _ea.GetEvent<MessageSentEvent>().Publish("メニュー設定モジュール(Module9)のメニュー１が実行されました");
            await Task.Delay(2000);
            _ea.GetEvent<MessageSentEvent>().Publish("");
            MyCModel.CommandEnable = true;
        }

        private DelegateCommand commandMenu2;
        public DelegateCommand CommandMenu2 =>
            commandMenu2 ?? (commandMenu2 = new DelegateCommand(ExecuteCommandMenu2));
        async void ExecuteCommandMenu2()
        {
            MyCModel.CommandEnable = false;
            _regionManager.RequestNavigate("ContentRegion", "M9");
            _ea.GetEvent<MessageSentEvent>().Publish("メニュー設定モジュール(Module9)のメニュー２が実行されました");
            await Task.Delay(2000);
            _ea.GetEvent<MessageSentEvent>().Publish("");
            MyCModel.CommandEnable = true;
        }
    }
}
