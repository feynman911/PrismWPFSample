using Prism.Commands;
using Prism.Mvvm;
using Prism.Events;
using System.Linq;
using CommonModels;
using Module5.Models;
using System.Collections.ObjectModel;

namespace Module5.ViewModels
{
    public class M5ViewModel : BindableBase
    {
        public M5ViewModel(){ }

        IEventAggregator _ea;

        public M5ViewModel(CModel cModel, IEventAggregator ea)
        {
            MyCModel = cModel;
            _ea = ea;

            //モデルの生成
            MyViewAModel = new M5Model(MyCModel, _ea);
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEM5");

        }

        private M5Model myViewAModel;
        public M5Model MyViewAModel
        {
            get { return myViewAModel; }
            set { SetProperty(ref myViewAModel, value); }
        }

        private string _message = "Module5";
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

        /// <summary>
        /// 言語設定が変わった時の処理
        /// </summary>
        public void ChangedLang()
        {
            Title = CModel.GetLocalizedValue<string>("TITLEM5");
        }

        private CModel myCommonData;
        public CModel MyCModel
        {
            get { return myCommonData; }
            set { SetProperty(ref myCommonData, value); }
        }

        private ObservableCollection<SettingObject> settingCollection 
            = new ObservableCollection<SettingObject>();
        public ObservableCollection<SettingObject> SettingCollection
        {
            get { return settingCollection; }
            set { SetProperty(ref settingCollection, value); }
        }

        private DelegateCommand dispSetting;
        /// <summary>
        /// 設定読み出しコマンド
        /// </summary>
        public DelegateCommand DispSetting =>
            dispSetting ?? (dispSetting = new DelegateCommand(ExecuteDispSetting));
        void ExecuteDispSetting()
        {
            makeSettingCollection();
        }

        /// <summary>
        /// CommonDataのSettings.settingsをSettingCollectionに読み出し
        /// </summary>
        private void makeSettingCollection()
        {
            var tempCollection = new ObservableCollection<SettingObject>();
            foreach (System.Configuration.SettingsProperty item in MyCModel.SettingInfo.Properties)
            {
                var name = item.Name;
                var value = MyCModel.SettingInfo[name];
                tempCollection.Add(new SettingObject(name,value));
            }
            SettingCollection = new ObservableCollection<SettingObject>(tempCollection.OrderBy(n => n.Key));
        }


        private DelegateCommand saveSetting;
        /// <summary>
        /// 設定保存コマンド
        /// </summary>
        public DelegateCommand SaveSetting =>
            saveSetting ?? (saveSetting = new DelegateCommand(ExecuteSaveSetting));
        void ExecuteSaveSetting()
        {
            saveSettingCollection();
        }

        /// <summary>
        /// SettingCollectionをCommonDataのSettings.settingsに保存
        /// </summary>
        private void saveSettingCollection()
        {
            MyCModel.LogString = "";
            if (SettingCollection.Count == 0) MyCModel.LogString = "Setting Save No Data Error!";
            foreach(SettingObject item in SettingCollection)
            {
                string key = item.Key;
                object value = item.Value;
                MyCModel.SettingInfo[key] = value;
            }
            MyCModel.SettingInfo.Save();
        }


        private DelegateCommand resetSetting;
        /// <summary>
        /// 設定リセットコマンド
        /// </summary>
        public DelegateCommand ResetSetting =>
            resetSetting ?? (resetSetting = new DelegateCommand(ExecuteResetSetting));
        void ExecuteResetSetting()
        {
            resetSettingCollection();
        }

        /// <summary>
        /// SettingCollectionをCommonDataのSettings.settingsにリセット
        /// </summary>
        private void resetSettingCollection()
        {
            MyCModel.LogString = "";
            MyCModel.SettingInfo.Reset();
            makeSettingCollection();
        }


        public class SettingObject
        {
            public string Key { get; set; }
            public object Value { get; set; }

            public SettingObject(string a, object b)
            {
                Key = a;
                Value = b;
            }
        }

    }


}
