using System;
using System.Windows.Data;

namespace CommonModels.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringCheck2BrushConverter : IValueConverter
    {
        /// <summary>
        /// データソース（ViewModel）　→　バインドターゲット（View）
        /// 例：パラメータ　STRING, Red, Black
        /// 文字列に"STRING"が含まれていた時に赤、それ以外は黒
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            string val = (string)value;

            string sp = (string)parameter;
            String[] paras = sp.Split(',');

            if (val.Contains(paras[0]))
            {
                return paras[1];
            }
            return paras[2];
        }

        /// <summary>
        /// バインドターゲット（View）→　データソース（ViewModel）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
