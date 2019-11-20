using CommonModels;
using CommonModels.Converters;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleB.ViewModels
{
    public class MBViewModel : BindableBase
    {
        IEventAggregator _ea;
        private readonly IRegionManager _regionManager;

        public MBViewModel()
        {

        }

        public MBViewModel(CModel commonModel, IEventAggregator ea, IRegionManager regionManager)
        {
            MyCModel = commonModel;
            _ea = ea;
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEMB");
            _regionManager = regionManager;
        }

        private string _message = "ModuleB";
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
            Title = CModel.GetLocalizedValue<string>("TITLEMB");
        }




        private bool ledOn = false;
        public bool LedOn
        {
            get { return ledOn; }
            set { SetProperty(ref ledOn, value); }
        }

        private bool flickerOn = false;
        public bool FlickerOn
        {
            get { return flickerOn; }
            set { SetProperty(ref flickerOn, value); }
        }

        private string color = "Green";
        public string Color
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }

        private EnumDefines.RBEnum colorSelect = EnumDefines.RBEnum.RB0;
        public EnumDefines.RBEnum ColorSelect
        {
            get { return colorSelect; }
            set { SetProperty(ref colorSelect, value);
                Color = "Green";
                if (value == EnumDefines.RBEnum.RB0) Color = "Green";
                if (value == EnumDefines.RBEnum.RB1) Color = "Gold";
                if (value == EnumDefines.RBEnum.RB2) Color = "Red";
            }
        }

        private double flickerRatio = 1.0;
        public double FlickerRatio
        {
            get { return flickerRatio; }
            set { SetProperty(ref flickerRatio, value); }
        }



    }
}
