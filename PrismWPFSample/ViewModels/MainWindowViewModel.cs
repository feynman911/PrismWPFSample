using CommonModels;
using Prism.Events;
using Prism.Mvvm;
using System.Globalization;
using WPFLocalizeExtension.Engine;

namespace PrismWPFSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IEventAggregator _ea;

        public MainWindowViewModel() { }
        public MainWindowViewModel(CModel commondata, IEventAggregator ea)
        {
            _ea = ea;
            MyCommonData = commondata;

            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo(System.Globalization.CultureInfo.CurrentCulture.Name);
            //LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("ja-JP");
            Title = CModel.GetLocalizedValue<string>("TITLE");

            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);
            _ea.GetEvent<MessageSentEvent>().Subscribe(SetMessage);
            //Trace.WriteLine("main");
        }

        private string _title;
        /// <summary>
        /// ウインドウタイトルとバインド
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
            Title = CModel.GetLocalizedValue<string>("TITLE");
        }

        private CModel myCommondata;
        /// <summary>
        /// アプリ全体からアクセスできるオブジェクト
        /// </summary>
        public CModel MyCommonData
        {
            get { return myCommondata; }
            set { SetProperty(ref myCommondata, value); }
        }

        private void SetMessage(string st)
        {
            MessageDisplay = st;
        }

        private string messageDisplay = "";
        public string MessageDisplay
        {
            get { return messageDisplay; }
            set { SetProperty(ref messageDisplay, value); }
        }
    }
}
