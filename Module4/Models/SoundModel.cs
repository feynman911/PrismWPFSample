using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Events;
using System.Diagnostics;
using CommonModels;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;

//OxyPlot & NAudio & FFT

namespace Module4.Models
{
    //BindableBaseにしておくことで、直接画面とバインドできる
    //画面表示の為のデータ構造にはしない
    //画面表示の為に変換が必要な時にはViewModelでラップする
    //そのまま表示できるものは、わざわざViewModelでリレーするようなことはしない
    public class SoundModel:BindableBase
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
        }

        WaveIn waveIn;
        int count = 0;
        double counttime = 0.0;
        public bool captureFlag = false;

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

        float[] SoundData = new float[1024]; // 音声データ

        /// <summary>
        /// WaveIn_DataAvailableイベント発生時のデータ処理
        /// </summary>
        /// <param name="sample"></param>
        private void Capture(float sample)
        {
            SoundData[count] = sample * (float)FastFourierTransform.HammingWindow(count,1024);
            count++;
            if (count == 1024)
            {
                MakeFFT(SoundData);
                plotModelFFT.InvalidatePlot(true);
                count = 0;
            }

            DataPointsRT.Add(new DataPoint(counttime, sample));
            if (DataPointsRT.Count > 1024) DataPointsRT.RemoveAt(0);
            plotModelRT.InvalidatePlot(true);
            counttime = counttime + 1.0 / 8000.0;
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

        /// <summary>
        /// リアルタイム表示用プロットモデル初期設定
        /// </summary>
        public void InitPlotModelRT()
        {
            PlotModelRT.Title = "Sound Data Realtime Display";

            var seriesRT = new LineSeries
            {
                Title = "line1",
                ItemsSource = DataPointsRT,//ListをImageSourceに設定
                DataFieldX = "X",//ImageSourceのXをXに設定
                DataFieldY = "Y",//ImageSourceのYをYに設定
                Color = OxyColor.Parse("#4CAF50"),
                //MarkerSize = 1,
                //MarkerFill = OxyColor.Parse("#F55FFFFF"),
                //MarkerStroke = OxyColor.Parse("#4CAF50"),
                //MarkerStrokeThickness = 1.5,
                //MarkerType = MarkerType.Cross,
                StrokeThickness = 1,
            };
            PlotModelRT.Series.Add(seriesRT);

            var lineAnnotation = new LineAnnotation();
            lineAnnotation.Type = LineAnnotationType.Horizontal;
            lineAnnotation.Y = 0;
            lineAnnotation.Color = OxyColors.Blue;
            PlotModelRT.Annotations.Add(lineAnnotation);

            var linearAxis1 = new LinearAxis();
            linearAxis1.Minimum = -0.1;
            linearAxis1.Maximum = 0.1;
            linearAxis1.AbsoluteMinimum = -0.1;
            linearAxis1.AbsoluteMaximum = 0.1;
            PlotModelRT.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            PlotModelRT.Axes.Add(linearAxis2);
            PlotModelRT.PlotMargins = new OxyThickness(50.0, 0.0, 10.0, 30.0);
        }

        private List<DataPoint> dataPointsRT = new List<DataPoint>();
        public List<DataPoint> DataPointsRT
        {
            get { return dataPointsRT; }
            set { SetProperty(ref dataPointsRT, value); }
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

        public void InitPlotModelFFT()
        {
            PlotModelFFT.Title = "Sound Data Realtime FFT";

            var seriesFFT = new LineSeries
            {
                Title = "line1",
                ItemsSource = DataPointsFFT,//ListをImageSourceに設定
                DataFieldX = "X",//ImageSourceのXをXに設定
                DataFieldY = "Y",//ImageSourceのYをYに設定
                Color = OxyColor.Parse("#4CAF50"),
                //MarkerSize = 1,
                //MarkerFill = OxyColor.Parse("#F55FFFFF"),
                //MarkerStroke = OxyColor.Parse("#4CAF50"),
                //MarkerStrokeThickness = 1.5,
                //MarkerType = MarkerType.Cross,
                StrokeThickness = 1,
            };
            PlotModelFFT.Series.Add(seriesFFT);
            var linearAxis1 = new LinearAxis();
            linearAxis1.Minimum = 0;
            linearAxis1.Maximum = 0.01;
            linearAxis1.AbsoluteMinimum = 0.0;
            PlotModelFFT.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            PlotModelFFT.Axes.Add(linearAxis2);
            PlotModelFFT.PlotMargins = new OxyThickness(50.0, 0.0, 10.0, 30.0);

        }

        private List<DataPoint> dataPointsFFT = new List<DataPoint>();
        public List<DataPoint> DataPointsFFT
        {
            get { return dataPointsFFT; }
            set { SetProperty(ref dataPointsFFT, value); }
        }

        public void MakeFFT(float[] data)
        {
            var fft = ExecuteFFT(data);
            DataPointsFFT.Clear();
            for (int i = 0; i < 512; i++)
            {
                DataPointsFFT.Add(new DataPoint(i * 4000.0 /512.0, fft[i]));
            }
        }

        /// <summary>
        /// 
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
            for( int i=0 ; i<len/2; i++)
            {
                ret[i] = (float)Math.Sqrt(fftSample[i].X * fftSample[i].X + fftSample[i].Y * fftSample[i].Y) * 2.0f;
            }
            return ret;
        }

        #endregion
    }
}
