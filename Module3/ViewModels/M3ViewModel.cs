using CommonModels;
using Module3.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace Module3.ViewModels
{
    public class M3ViewModel : BindableBase
    {
        IEventAggregator _ea;

        public M3ViewModel() { }

        public M3ViewModel(CModel cModel, IEventAggregator ea)
        {
            MyCModel = cModel;
            _ea = ea;

            //モデルの生成
            MyViewAModel = new M3Model(MyCModel, _ea);
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEM3");

        }

        private M3Model myViewAModel;
        /// <summary>
        /// このモジュールのモデル
        /// </summary>
        public M3Model MyViewAModel
        {
            get { return myViewAModel; }
            set { SetProperty(ref myViewAModel, value); }
        }

        private string _message = "Module3";
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
            Title = CModel.GetLocalizedValue<string>("TITLEM3");
        }

        private CModel myCommonData;
        public CModel MyCModel
        {
            get { return myCommonData; }
            set { SetProperty(ref myCommonData, value); }
        }

        private DelegateCommand commandLogWrite;
        /// <summary>
        /// ログ保存テストコマンド
        /// </summary>
        public DelegateCommand CommandLogWrite =>
            commandLogWrite ?? (commandLogWrite = new DelegateCommand(ExecuteCommandLogWrite));
        async void ExecuteCommandLogWrite()
        {
            myCommonData.LogString = "Module3のボタンが押されました";
            await Task.Delay(2000);
            myCommonData.LogString = "";
        }




    }
}
