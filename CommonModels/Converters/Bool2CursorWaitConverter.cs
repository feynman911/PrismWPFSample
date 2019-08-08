using System;
using System.Windows.Data;
using System.Windows.Input;

namespace CommonModels.Converters
{
    /// <summary>
    /// trueの時にCursorType.Waitを設定する
    /// </summary>
    [ValueConversion(typeof(bool), typeof(string))]
    public class Bool2CursorWaitConverter : IValueConverter
    {
        // データソース(データ)→バインドターゲット(UI)　変換メソッド
        public object Convert(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            var st = (string)parameter;
            if (true == (bool)value)
            {
                return CursorType.Wait.ToString();
            }
            else
            {
                return CursorType.Arrow.ToString();
            }
        }

        // バインドターゲット(UI)→データソース(データ)　変換メソッド
        public object ConvertBack(object value, Type targetType,
                object parameter, System.Globalization.CultureInfo culture)
        {
            if (CursorType.Wait.ToString() == (string)value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
