
using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace CommonModels.Converters
{
    //���W�I�{�^���I���R���o�[�^�[
    //�p�����[�^�[�Ƃ��Ď����̖��O�iRB0�Ƃ��j��ݒ肵�A�����ϐ��Ƀo�C���h����
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
        /// �f�[�^�\�[�X�iViewModel�j�@���@�o�C���h�^�[�Q�b�g�iView)
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
        /// �o�C���h�^�[�Q�b�g�iView�j���@�f�[�^�\�[�X�iViewModel�j
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
