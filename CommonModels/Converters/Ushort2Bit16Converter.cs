using System;
using System.Globalization;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace CommonModels.Converters
{
    [ValueConversion(typeof(ushort), typeof(string))]
    public class Ushort2Bit16Converter : IValueConverter
    {
        /// <summary>
        /// データソース（ViewModel）　→　バインドターゲット（View）
        /// 0A28 -> 0000 1010 0010 1000
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ret = System.Convert.ToString((ushort)value, 2).PadLeft(16, '0');
            ret = ret.Insert(4, " ");
            ret = ret.Insert(9, " ");
            ret = ret.Insert(14, " ");
            return ret;
        }

        /// <summary>
        /// バインドターゲット（View）→　データソース（ViewModel）
        /// 0000 1010 0010 1000 -> 0A28
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = Regex.Replace((string)value, @"[^0-1]", "");
            ushort ret = System.Convert.ToUInt16(temp, 2);
            return ret;
        }
    }
}
