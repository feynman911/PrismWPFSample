using System;
using System.Windows.Data;

namespace CommonModels.Converters
{
    /// <summary>
    /// 数字の大きさでブラシ色を変更
    /// パラメータ　Blue,3.0,Yellow,6.0,Red
    /// 数字が３よりも小さければ青、３以上６よりも小さければ黄色、６よりも大きければ赤となる例
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class DoubleCheck2BrushConverter : IValueConverter
    {
        // データソース(データ)→バインドターゲット(UI)　変換メソッド
        public object Convert(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culter)
        {
            string br = "Blue";
            double val1 = 3.0;
            double val2 = 6.0;

            double data = (double)value;

            string sp = (string)parameter;
            String[] paras = sp.Split(',');

            double.TryParse(paras[1], out val1);
            if (data < val1)
            {
                if (paras[0] != "") br = paras[0];
                return br;
            }

            double.TryParse(paras[3], out val2);
            if (data < val2)
            {
                if (paras[2] != "") br = paras[2];
                return br;
            }

            if (paras[4] != "") br = paras[4];
            return br;
        }

        // バインドターゲット(UI)→データソース(データ)　変換メソッド
        public object ConvertBack(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culter)
        {
            throw new NotImplementedException();
        }
    }

}
