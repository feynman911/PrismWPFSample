using CommonModels;
using CommonModels.Converters;
using Prism.Events;
using Prism.Mvvm;

namespace Module6.ViewModels
{
    public class M6ViewModel : BindableBase
    {
        IEventAggregator _ea;

        public M6ViewModel()
        {

        }

        public M6ViewModel(CModel cModel, IEventAggregator ea)
        {
            MyCModel = cModel;
            _ea = ea;
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEM6");
        }

        private string _message = "Module6";
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

        private CModel myCommonData;
        public CModel MyCModel
        {
            get { return myCommonData; }
            set { SetProperty(ref myCommonData, value); }
        }

        /// <summary>
        /// 言語設定が変わった時の処理
        /// </summary>
        public void ChangedLang()
        {
            Title = CModel.GetLocalizedValue<string>("TITLEM6");
        }

        private EnumDefines.RBEnum myType = EnumDefines.RBEnum.RB1;
        /// <summary>
        /// ラジオボタンバインドテスト用
        /// </summary>
        public EnumDefines.RBEnum MyType
        {
            get { return this.myType; }
            set { SetProperty(ref myType, value); }
        }

        private double testData = 1.0;
        /// <summary>
        /// スケールコンバーターテスト用
        /// </summary>
        public double TestData
        {
            get { return testData; }
            set { SetProperty(ref testData, value); }
        }

        private ushort testData2 = 0x1234;
        /// <summary>
        /// スケールコンバーターテスト用
        /// </summary>
        public ushort TestData2
        {
            get { return testData2; }
            set { SetProperty(ref testData2, value); }
        }



    }
}
