using System;
using System.Windows;
using System.Windows.Data;

namespace CommonModels.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolVisiblityConverter : IValueConverter
    {
        /// <summary>
        /// データソース（ViewModel）　→　バインドターゲット（View）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            bool state = (bool)value;

            if (state)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
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
