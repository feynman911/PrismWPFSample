using CommonModels;
using NAudio.Dsp;
using NAudio.Wave;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

//OxyPlot & NAudio & FFT

namespace Module4.Models
{
    //BindableBaseにしておくことで、直接画面とバインドできる
    //画面表示の為のデータ構造にはしない
    //画面表示の為に変換が必要な時にはViewModelでラップする
    //そのまま表示できるものは、わざわざViewModelでリレーするようなことはしない
    public class SoundModel:BindableBase,IDisposable
    {
        IEventAggregator ea;
        CModel commondata;

        public SoundModel() { }

        public SoundModel(CModel _commondata, IEventAggregator _ea)
        {
            ea = _ea;
            commondata = _commondata;

            InitPlotModelRT();
            InitPlotModelFFT();
            InitPlotModelSpectrogram();
        }


        WaveIn waveIn;
        const int datanum = 4096; //WAVE波形表示用 4096 / 8000 = 0.512秒
        const int fftnum = 1024; //スペクトグラムデータ数 0.128秒

        public bool captureFlag = false;
        float[] SoundData = new float[datanum]; // 音声データ


        /// <summary>
        /// サウンドキャプチャー開始処理
        /// </summary>
        public void SoundCaptureOn()
        {
            if (captureFlag)
            {
                commondata.StatusString = "キャプチャー中です";
                return;
            }
            commondata.StatusString = "";
            if (WaveIn.DeviceCount == 0)
            {
                commondata.StatusString = "サウンドデバイスがありません";
                return;
            }
            //デバイスの抽出
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var deviceInfo = WaveIn.GetCapabilities(i);
                commondata.StatusString = String.Format("Device {0}: {1}, {2} channels",
                    i, deviceInfo.ProductName, deviceInfo.Channels);
            }

            //デバイス０のサンプリングをスタート
            waveIn = new WaveIn()
            {
                DeviceNumber = 0, // Default
            };
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.WaveFormat = new WaveFormat(sampleRate: 8000, channels: 1);
            waveIn.StartRecording();
            captureFlag = true;
        }

        /// <summary>
        /// サウンドキャプチャー用のコールバック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // 32bitで最大値1.0fにする
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);

                float sample32 = sample / 32768f;
                Capture(sample32);
            }
        }

        /// <summary>
        /// サウンドキャプチャー終了処理
        /// </summary>
        public void SoundCaptureOff()
        {
            if (!captureFlag) return;
            waveIn.StopRecording();
            waveIn.DataAvailable -= WaveIn_DataAvailable;
            captureFlag = false;
            commondata.StatusString = "";
        }

        /// <summary>
        /// WaveIn_DataAvailableイベント発生時のデータ処理
        /// </summary>
        /// <param name="sample"></param>
        private void Capture(float sample)
        {
            for (int i = 0; i < datanum - 1; i++)
            {
                SoundData[i] = SoundData[i + 1];
            }
            SoundData[datanum - 1] = sample;
        }

        /// <summary>
        /// リアルタイム波形表示
        /// </summary>
        public void PlotRT()
        {
            DataPointsRT.Clear();
            DataPointsRT.AddRange(SoundData.Select((v, index) => new DataPoint(1.0 / 8000 * (index - datanum), v)));
            PlotModelRT.InvalidatePlot(true);
        }

        /// <summary>
        /// FFT波形表示
        /// </summary>
        public void PlotFFT()
        {
            DataPointsFFT.Clear();
            DataPointsFFT.AddRange(ExecuteFFT(SoundData).Select((v, index) => new DataPoint(8000.0 / datanum * index, v)));
            PlotModelFFT.InvalidatePlot(true);
        }

        /// <summary>
        /// スペクトログラム表示
        /// </summary>
        public void PlotSpectrogram()
        {
            float[] data = new float[fftnum];
            Array.Copy(SoundData, data, fftnum);
            AddSpect(ExecuteFFT(data));
            PlotModelSpectrogram.InvalidatePlot(true);
        }

        #region **************** リアルタイム波形　***************

        private PlotModel plotModelRT = new PlotModel();
        /// <summary>
        /// リアルタイム表示用プロットモデル
        /// </summary>
        public PlotModel PlotModelRT
        {
            get { return plotModelRT; }
            set { SetProperty(ref plotModelRT, value); }
        }

        private List<DataPoint> dataPointsRT = new List<DataPoint>();
        public List<DataPoint> DataPointsRT
        {
            get { return dataPointsRT; }
            set { SetProperty(ref dataPointsRT, value); }
        }

        /// <summary>
        /// リアルタイム表示用プロットモデル初期設定
        /// </summary>
        public void InitPlotModelRT()
        {
            PlotModelRT.Title = "Sound Data Realtime Display";
            PlotModelRT.PlotMargins = new OxyThickness(50.0, 0.0, 10.0, 40.0);

            var seriesRT = new LineSeries();
            seriesRT.Title = "line1";
            seriesRT.ItemsSource = DataPointsRT;//ListをImageSourceに設定
            seriesRT.Color = OxyColor.Parse("#4CAF50");
            seriesRT.StrokeThickness = 1;
            PlotModelRT.Series.Add(seriesRT);

            //センターライン描画
            var lineAnnotation = new LineAnnotation();
            lineAnnotation.Type = LineAnnotationType.Horizontal;
            lineAnnotation.Y = 0;
            lineAnnotation.Color = OxyColors.Blue;
            PlotModelRT.Annotations.Add(lineAnnotation);

            var linearAxis1 = new LinearAxis();
            linearAxis1.Minimum = -0.1;
            linearAxis1.Maximum = 0.1;
            linearAxis1.AbsoluteMinimum = -1.0;
            linearAxis1.AbsoluteMaximum = 1.0;
            linearAxis1.Title = "Sound Level";
            PlotModelRT.Axes.Add(linearAxis1);

            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.Title = "Time";
            linearAxis2.Unit = "sec";
            linearAxis2.Minimum = -0.51;
            linearAxis2.Maximum = 0.0;
            linearAxis2.IsZoomEnabled = false;
            PlotModelRT.Axes.Add(linearAxis2);

        }

        #endregion

        #region ***************　FFT波形　***********************

        private PlotModel plotModelFFT = new PlotModel();
        /// <summary>
        /// FFT表示用プロットモデル
        /// </summary>
        public PlotModel PlotModelFFT
        {
            get { return plotModelFFT; }
            set { SetProperty(ref plotModelFFT, value); }
        }

        private List<DataPoint> dataPointsFFT = new List<DataPoint>();
        public List<DataPoint> DataPointsFFT
        {
            get { return dataPointsFFT; }
            set { SetProperty(ref dataPointsFFT, value); }
        }

        /// <summary>
        /// FFT表示用プロットモデルの初期化
        /// </summary>
        public void InitPlotModelFFT()
        {
            PlotModelFFT.Title = "Sound Data Realtime FFT";
            PlotModelFFT.PlotMargins = new OxyThickness(50.0, 0.0, 10.0, 40.0);

            var seriesFFT = new LineSeries();
            seriesFFT.Title = "line1";
            seriesFFT.ItemsSource = DataPointsFFT;//ListをImageSourceに設定
            seriesFFT.Color = OxyColor.Parse("#4CAF50");
            seriesFFT.StrokeThickness = 1;
            PlotModelFFT.Series.Add(seriesFFT);

            var linearAxis1 = new LinearAxis();
            linearAxis1.Minimum = 0;
            linearAxis1.Maximum = 0.01;
            linearAxis1.AbsoluteMinimum = 0.0;
            PlotModelFFT.Axes.Add(linearAxis1);

            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.Minimum = 0.0;
            linearAxis2.Maximum = 4000.0;
            linearAxis2.IsZoomEnabled = false;
            linearAxis2.Title = "Frequency";
            linearAxis2.Unit = "Hz";
            PlotModelFFT.Axes.Add(linearAxis2);


        }

        #endregion

        #region *************** スペクトログラム波形　***********

        private PlotModel plotModelSpectrogram = new PlotModel();
        /// <summary>
        /// FFT表示用プロットモデル
        /// </summary>
        public PlotModel PlotModelSpectrogram
        {
            get { return plotModelSpectrogram; }
            set { SetProperty(ref plotModelSpectrogram, value); }
        }

        float[,] Data = new float[100, fftnum / 2];
        public HeatMapSeries heatMapSeries1 = new HeatMapSeries();

        /// <summary>
        /// スペクトログラムチャートの初期化
        /// </summary>
        public void InitPlotModelSpectrogram()
        {
            PlotModelSpectrogram.Title = "Spectrogram";
            PlotModelSpectrogram.PlotMargins = new OxyThickness(50.0, 0.0, 50.0, 40.0);

            var linearColorAxis1 = new LinearColorAxis();
            linearColorAxis1.HighColor = OxyColors.Gray;
            linearColorAxis1.LowColor = OxyColors.Black;
            linearColorAxis1.Position = AxisPosition.Right;
            linearColorAxis1.Key = "z";
            linearColorAxis1.Minimum = 0.0;
            linearColorAxis1.Maximum = 0.01;
            linearColorAxis1.AbsoluteMinimum = 0.0;
            linearColorAxis1.AbsoluteMaximum = 0.01;
            PlotModelSpectrogram.Axes.Add(linearColorAxis1);

            var linearAxis1 = new LinearAxis();
            linearAxis1.Position = AxisPosition.Bottom;
            linearAxis1.Key = "x";
            linearAxis1.IsAxisVisible = false;
            linearAxis1.IsZoomEnabled = false;
            PlotModelSpectrogram.Axes.Add(linearAxis1);

            //目盛り表示用ダミー
            var linearAxis_a = new LinearAxis();
            linearAxis_a.Position = AxisPosition.Bottom;
            linearAxis_a.Key = "a";
            linearAxis_a.Minimum = -10.0;
            linearAxis_a.Maximum = 0;
            linearAxis_a.Title = "Time";
            linearAxis_a.Unit = "sec";
            linearAxis_a.IsZoomEnabled = false;
            PlotModelSpectrogram.Axes.Add(linearAxis_a);

            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Left;
            linearAxis2.Key = "y";
            linearAxis2.IsAxisVisible = false;
            linearAxis2.IsZoomEnabled = false;
            PlotModelSpectrogram.Axes.Add(linearAxis2);

            //目盛り表示用ダミー
            var linearAxis_b = new LinearAxis();
            linearAxis_b.Position = AxisPosition.Left;
            linearAxis_b.Key = "b";
            linearAxis_b.Minimum = 0.0;
            linearAxis_b.Maximum = 4000.0;
            linearAxis_b.Title = "Frequency";
            linearAxis_b.Unit = "Hz";
            linearAxis_b.IsZoomEnabled = false;
            PlotModelSpectrogram.Axes.Add(linearAxis_b);

            heatMapSeries1.Data = new double[100, fftnum / 2];
            heatMapSeries1.X0 = 0.0;
            heatMapSeries1.X1 = 100.0;
            heatMapSeries1.Y0 = 0.0;
            heatMapSeries1.Y1 = fftnum / 2;

            PlotModelSpectrogram.Series.Add(heatMapSeries1);
            //heatMapSeries1.Interpolate = false;
        }

        /// <summary>
        /// 振幅データをスペクトログラム配列に追加する
        /// </summary>
        /// <param name="data"></param>
        private void AddSpect(float[] data)
        {
            for (int i = 0; i < 99; i++)
            {
                for (int j = 0; j < fftnum / 2; j++)
                {
                    Data[i, j] = Data[i + 1, j];
                }
            }
            for (int j = 0; j < fftnum / 2; j++)
            {
                Data[99, j] = data[j];
            }

            var pldata = new double[100, fftnum / 2];
            Array.Copy(Data, pldata, Data.Length);
            heatMapSeries1.Data = pldata;
        }

        /// <summary>
        /// FFTして振幅を返す（配列数は半分になる）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public float[] ExecuteFFT(float[] data)
        {
            int len = data.Count();
            int m = (int)Math.Log((double)data.Count(), 2);
            var fftSample = data.Select(v => new Complex { X = v, Y = 0.0f }).ToArray();
            FastFourierTransform.FFT(true, m, fftSample);
            var ret = new float[len / 2];
            for (int i = 0; i < len / 2; i++)
            {
                ret[i] = (float)Math.Sqrt(fftSample[i].X * fftSample[i].X + fftSample[i].Y * fftSample[i].Y) * 2.0f;
            }
            return ret;
        }

        #endregion

        public void Dispose()
        {
            SoundCaptureOff();
        }
    }
}
