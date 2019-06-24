using System;
using System.Windows.Data;

namespace CommonModels.Converters
{
    //Viewの数値に10を足し50倍して更に3を足す例
    //ConverterParameter=10.0,50.0,3.0

    [ValueConversion(typeof(double), typeof(double))]
    public class ScaleConverter : IValueConverter
    {
        /// <summary>
        /// データソース（ViewModel）　→　バインドターゲット（View）
        /// 数値にp0を足してp1倍してp2を足す
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value,
                                Type targetType,
                                object parameter,
                                System.Globalization.CultureInfo culture)
        {
            double result = 0.0;
            double.TryParse(value.ToString(), out double val);
            string sp = (string)parameter;
            String[] paras = sp.Split(',');
            double.TryParse(paras[0], out double p0);
            double.TryParse(paras[1], out double p1);
            double.TryParse(paras[2], out double p2);
            if (p1 != 0)
            {
                result = (val + p0) * p1 + p2;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// バインドターゲット（View）→　データソース（ViewModel）
        /// 数値からp2を引いてP1分の１にしP0を引く
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value,
                                    Type targetType,
                                    object parameter,
                                    System.Globalization.CultureInfo culture)
        {
            double result = 0.0;
            double.TryParse(value.ToString(), out double val);
            string sp = (string)parameter;
            String[] paras = sp.Split(',');
            double.TryParse(paras[0], out double p0);
            double.TryParse(paras[1], out double p1);
            double.TryParse(paras[2], out double p2);
            if(p1 != 0) result = (val - p2) / p1 - p0;
            return result;
        }
    }
}
