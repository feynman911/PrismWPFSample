using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Prism.Mvvm;
using System.Globalization;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;
using System.Diagnostics;

namespace CommonModels
{
    public class CModel: BindableBase
    {
         public CModel(){ }

        /// <summary>
        /// 多言語対応のためにResourcesから文言を抜き出すためのメソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetLocalizedValue<T>(string key)
        {
            //メインアプリのリソースを使う時
            //var temp = Assembly.GetCallingAssembly().GetName().Name + ":Resources:" + key;

            //任意のプロジェクトのリソースを固定して使う時は直接書けばいい
            var temp = "CommonModels:Resources:" + key;
            var ret = LocExtension.GetLocalizedValue<T>(temp);
            return ret;

        }

        private string logString = "";
        /// <summary>
        /// このプロパティーに設定するとログに書き込まれる
        /// </summary>
        public string LogString
        {
            get { return logString; }
            set {
                if (value != "")Trace.WriteLine(value);
                SetProperty(ref logString, value);
            }
        }

        private string statusString = "";
        /// <summary>
        /// このプロパティーに設定するとメイン画面に表示される
        /// </summary>
        public string StatusString
        {
            get { return statusString; }
            set { SetProperty(ref statusString, value); }
        }

        private Properties.Settings settingInfo = Properties.Settings.Default;
        /// <summary>
        /// 共通でこのPropertiesを使用する
        /// Properties.Settings.settingsのアクセス修飾子をPublicにしておくこと
        /// </summary>
        public Properties.Settings SettingInfo
        {
            get { return settingInfo; }
            set { SetProperty(ref settingInfo, value); }
        }

        private bool commandEnable = true;
        /// <summary>
        /// コマンドボタンとバインドして同時押し禁止
        /// </summary>
        public bool CommandEnable
        {
            get { return commandEnable; }
            set { SetProperty(ref commandEnable, value); }
        }

    }
}
