
using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace CommonModels.Converters
{
    //ラジオボタン選択コンバーター
    //パラメーターとして自分の名前（RB0とか）を設定し、同じ変数にバインドする
    //    private EnumDefines.RBEnum myType;
    //    public EnumDefines.RBEnum MyType
    //    {
    //        get { return this.myType; }
    //        set
    //        {
    //            this.myType = value;
    //            RaisePropertyChanged("MyType");
    //       }
    //    }

    public class EnumRadioConverter : IValueConverter
    {
        /// <summary>
        /// データソース（ViewModel）　→　バインドターゲット（View)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string paramString = parameter as string;
            if (paramString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (!Enum.IsDefined(value.GetType(), paramString))
            {
                return DependencyProperty.UnsetValue;
            }

            var paramParsed = Enum.Parse(value.GetType(), paramString);

            return (value.Equals(paramParsed));
        }

        /// <summary>
        /// バインドターゲット（View）→　データソース（ViewModel）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var paramString = parameter as string;
            if (paramString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if ((bool)value != true) return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, paramString);
        }
    }
    public class EnumDefines
    {
        public enum RBEnum
        {
            RB0,
            RB1,
            RB2,
            RB3,
            RB4,
            RB5,
            RB6,
            RB7,
            RB8,
            RB9,
        }
    }
}
