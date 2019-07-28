﻿using CommonModels;
using Prism.Events;
using Prism.Mvvm;

namespace Module7.ViewModels
{
    public class M7ViewModel : BindableBase
    {
        IEventAggregator _ea;

        public M7ViewModel() { }

        public M7ViewModel(CModel cModel, IEventAggregator ea)
        {
            MyCModel = cModel;
            _ea = ea;
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEM7");
        }

        private string _message = "Module7";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private CModel myCModel;
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
            Title = CModel.GetLocalizedValue<string>("TITLEM7");
        }


    }
}
