using System;
using System.Globalization;
using System.Windows.Data;

namespace CommonModels.Converters
{
    [ValueConversion(typeof(ushort),typeof(string))]
    public class Ushort2HexConverter : IValueConverter
    {
        /// <summary>
        /// データソース（ViewModel）　→　バインドターゲット（View）
        /// ushort型数字を16進４桁文字に変換
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ushort)value).ToString("X4");
        }

        /// <summary>
        /// バインドターゲット（View）→　データソース（ViewModel）
        /// １６進４桁文字をushort型数字に変換
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ushort.TryParse((string)value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ushort result);
            return result;
        }
    }
}
