using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using CommonModels;
using Module4.Models;
using System.Diagnostics;
using OxyPlot;
using OxyPlot.Series;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Threading;

namespace Module4.ViewModels
{
    public class M4ViewModel : BindableBase
    {
        IEventAggregator _ea;

        public M4ViewModel() {  }

        public M4ViewModel(CModel cModel, IEventAggregator ea)
        {
            MyCModel = cModel;
            _ea = ea;

            //モデルの生成
            MySoundModel = new SoundModel(MyCModel, _ea);
            _ea.GetEvent<LanguageChangeEvent>().Subscribe(ChangedLang);

            //TabItemのHeaderになる言葉を多言語設定の為Resoucesから取り出す
            Title = CModel.GetLocalizedValue<string>("TITLEM4");

            Plot1 = MySoundModel.PlotModelRT;
            Plot2 = MySoundModel.PlotModelFFT;
            
        }

        private SoundModel myViewAModel;
        /// <summary>
        /// このモジュールのモデル
        /// </summary>
        public SoundModel MySoundModel
        {
            get { return myViewAModel; }
            set { SetProperty(ref myViewAModel, value); }
        }

        private string _message = "Module4";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title;
        /// <summary>
        /// TabConrtrolのHeader
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
            Title = CModel.GetLocalizedValue<string>("TITLEM4");
        }

        private CModel myCommonData;
        /// <summary>
        /// アプリ共通データ
        /// </summary>
        public CModel MyCModel
        {
            get { return myCommonData; }
            set { SetProperty(ref myCommonData, value); }
        }

        #region *****************************  トグルスイッチ関連
        private bool ts_Status;
        /// <summary>
        /// トグルスイッチの表示状態
        /// 状態変化に応じた処理を実行する
        /// </summary>
        public bool Ts_Status
        {
            get { return ts_Status; }
            set
            {
                if (Ts_Process(value)) SetProperty(ref ts_Status, value);
            }
        }

        /// <summary>
        /// トグルスイッチの変化で起動する処理
        /// </summary>
        /// <param name="val"></param>
        public bool Ts_Process(bool val)
        {
            bool ret = true;
            if (val)
            {
                MySoundModel.SoundCaptureOn();
                ret = MySoundModel.captureFlag;
            }
            else
            {
                MySoundModel.SoundCaptureOff();
                ret = !MySoundModel.captureFlag;
            }
            return ret;
        }
        #endregion

        #region ******************** コンテキストメニュー用コマンド

        private DelegateCommand<PlotModel> commandSavePlot;
        /// <summary>
        /// 指定されたプロットモデル画像をファイルに保存するコマンド
        /// Plot1 or Plot2
        /// </summary>
        public DelegateCommand<PlotModel> CommandSavePlot =>
            commandSavePlot ?? (commandSavePlot = new DelegateCommand<PlotModel>(ExecuteCommandSavePlot));
        void ExecuteCommandSavePlot(PlotModel parameter)
        {
            SavePlot(parameter);
        }

        /// <summary>
        /// PlotModel画像を保存する
        /// </summary>
        /// <param name="pm">PlotModel</param>
        /// <param name="filename">保存ファイル名</param>
        /// <param name="width">保存画像幅</param>
        /// <param name="height">保存画像高さ</param>
        public void SavePlot(PlotModel pm, string filename = "", int width = 800, int height = 600)
        {
            if ( filename == "")
            {
                var dlg = new SaveFileDialog
                {
                    Filter = ".png files|*.png|.pdf files|*.pdf",
                    DefaultExt = ".png"
                };
                if (dlg.ShowDialog().Value) filename = dlg.FileName;
            }
            if (filename != "")
            {
                var ext = Path.GetExtension(filename).ToLower();
                switch (ext)
                {
                    case ".png":
                        using (var s = File.Create(filename))
                        {
                            Application.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                OxyPlot.Wpf.PngExporter.Export(pm, s, width, height, OxyColors.White);
                            }));
                        }
                        break;
                    case ".pdf":
                        using (var s = File.Create(filename))
                        {
                            Application.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                OxyPlot.PdfExporter.Export(pm, s, width, height);
                            }));
                        }
                            break;
                    default:
                        break;

                }
            }
        }


        private DelegateCommand<PlotModel> commandCopyPlot;
        /// <summary>
        /// 指定されたプロットモデル画像をファイルに保存するコマンド
        /// Plot1 or Plot2
        /// </summary>
        public DelegateCommand<PlotModel> CommandCopyPlot =>
            commandCopyPlot ?? (commandCopyPlot = new DelegateCommand<PlotModel>(ExecuteCommandCopyPlot));
        void ExecuteCommandCopyPlot(PlotModel parameter)
        {
            CopyPlot(parameter);
        }

        /// <summary>
        /// PlotModelのグラフをクリップボードにコピーする
        /// </summary>
        /// <param name="pm"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CopyPlot(PlotModel pm, int width = 800, int height = 600)
        {
            var pngExporter = new OxyPlot.Wpf.PngExporter { Width = width, Height = height, Background = OxyColors.White };
            Application.Current.Dispatcher.Invoke((Action)(() => 
            {
                var bitmap = pngExporter.ExportToBitmap(pm);
                Clipboard.SetImage(bitmap);
            }));
        }


        private DelegateCommand<PlotModel> commandResizePlot;
        /// <summary>
        /// 指定されたプロットモデルの表示範囲をリセット
        /// Plot1 or Plot2
        /// </summary>
        public DelegateCommand<PlotModel> CommandResizePlot =>
            commandResizePlot ?? (commandResizePlot = new DelegateCommand<PlotModel>(ExecuteCommandResizePlot));
        void ExecuteCommandResizePlot(PlotModel parameter)
        {
            AutoResizePlot(parameter);
        }

        /// <summary>
        /// PlotModelの表示エリアをリセット
        /// </summary>
        /// <param name="pm"></param>
        public void AutoResizePlot(PlotModel pm)
        {
            pm.ResetAllAxes();
            pm.InvalidatePlot(true);
        }

        #endregion

        #region ******************** バインド用PlotModel　画面と１対１

        private PlotModel plot1 = new PlotModel();
        /// <summary>
        /// バインディング用プロットモデル１
        /// </summary>
        public PlotModel Plot1
        {
            get { return plot1; }
            set { SetProperty(ref plot1, value); }
        }

        private PlotModel plot2 = new PlotModel();
        /// <summary>
        /// バインディング用プロットモデル２
        /// </summary>
        public PlotModel Plot2
        {
            get { return plot2; }
            set { SetProperty(ref plot2, value); }
        }

        #endregion

    }
}
