using System;
using System.Windows.Data;

namespace CommonModels.Converters
{
    /// <summary>
    /// boolを反転する
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolInvertConverter : IValueConverter
    {
        // データソース(データ)→バインドターゲット(UI)　変換メソッド
        public object Convert(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            var temp = (bool)value;
            return !temp;
        }

        // バインドターゲット(UI)→データソース(データ)　変換メソッド
        public object ConvertBack(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            var temp = (bool)value;
            return !temp;
        }
    }
}
