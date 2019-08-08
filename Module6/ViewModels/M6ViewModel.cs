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
        /// Ushort2Bit16Converterテスト用
        /// </summary>
        public ushort TestData2
        {
            get { return testData2; }
            set { SetProperty(ref testData2, value); }
        }


        private double testData3 = 5.0;
        /// <summary>
        /// DoubleCheck2BrushConverterテスト用
        /// </summary>
        public double TestData3
        {
            get { return testData3; }
            set { SetProperty(ref testData3, value); }
        }

        private string testData4 = "ERROR";
        /// <summary>
        /// StringCheck2BrushConverterテスト用
        /// </summary>
        public string TestData4
        {
            get { return testData4; }
            set { SetProperty(ref testData4, value); }
        }


        private bool testData5 = true;
        /// <summary>
        /// BoolVisiblityConverterテスト用
        /// </summary>
        public bool TestData5
        {
            get { return testData5; }
            set { SetProperty(ref testData5, value); }
        }


        private bool testData6 = false;
        /// <summary>
        /// BoolVisiblityConverterテスト用
        /// </summary>
        public bool TestData6
        {
            get { return testData6; }
            set { SetProperty(ref testData6, value); }

        }


        private int testData7 = 5;
        /// <summary>
        /// BoolVisiblityConverterテスト用
        /// </summary>
        public int TestData7
        {
            get { return testData7; }
            set { SetProperty(ref testData7, value); }

        }
    }
}
