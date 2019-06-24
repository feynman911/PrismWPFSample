using Prism.Commands;
using Prism.Mvvm;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CommonModels;
using Module2.Models;

namespace Module2.ViewModels
{
    public class M2ViewModel : BindableBase
    {

        public M2ViewModel() { }

        IEventAggregator _ea;

        public M2ViewModel(CModel cModel, IEventAggregator ea)
        {
            MyCModel = cModel;
            _ea = ea;

            //モデルの生成
            MyViewAModel = new M2Model(MyCModel, _ea);
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEM2");
        }

        private M2Model myViewAModel;
        /// <summary>
        /// このモジュールのモデル
        /// </summary>
        public M2Model MyViewAModel
        {
            get { return myViewAModel; }
            set { SetProperty(ref myViewAModel, value); }
        }

        private string _message = "Module2";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title = CModel.GetLocalizedValue<string>("TITLEM2");
        /// <summary>
        /// TabItemのHeaderにバインドされる
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _headerMenu1 = CModel.GetLocalizedValue<string>("M2MENU1");
        /// <summary>
        /// TabItemのHeaderにバインドされる
        /// </summary>
        public string HeaderMenu1
        {
            get { return _headerMenu1; }
            set { SetProperty(ref _headerMenu1, value); }
        }

        private string _headerMenu2 = CModel.GetLocalizedValue<string>("M2MENU2");
        /// <summary>
        /// TabItemのHeaderにバインドされる
        /// </summary>
        public string HeaderMenu2
        {
            get { return _headerMenu2; }
            set { SetProperty(ref _headerMenu2, value); }
        }

        /// <summary>
        /// 言語設定が変わった時の処理
        /// </summary>
        public void ChangedLang()
        {
            Title = CModel.GetLocalizedValue<string>("TITLEM2");
            HeaderMenu1 = CModel.GetLocalizedValue<string>("M2MENU1");
            HeaderMenu2 = CModel.GetLocalizedValue<string>("M2MENU2");
        }

        private CModel myCModel;
        public CModel MyCModel
        {
            get { return myCModel; }
            set { SetProperty(ref myCModel, value); }
        }

        private DelegateCommand commandEventTest1;
        public DelegateCommand CommandEventTest1 =>
            commandEventTest1 ?? (commandEventTest1 = new DelegateCommand(ExecuteCommandEventTest1));
        async void ExecuteCommandEventTest1()
        {
            _ea.GetEvent<MessageSentEvent>().Publish("モジュール２のメニュー１が選択されました");
            await Task.Delay(2000);
            _ea.GetEvent<MessageSentEvent>().Publish("");
        }

        private DelegateCommand commandEventTest2;
        public DelegateCommand CommandEventTest2 =>
            commandEventTest2 ?? (commandEventTest2 = new DelegateCommand(ExecuteCommandEventTest2));
        async void ExecuteCommandEventTest2()
        {
            _ea.GetEvent<MessageSentEvent>().Publish("モジュール２のメニュー２が選択されました");
            await Task.Delay(2000);
            _ea.GetEvent<MessageSentEvent>().Publish("");
        }
    }
}
