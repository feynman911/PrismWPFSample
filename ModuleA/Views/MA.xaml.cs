using System.Windows.Controls;

namespace ModuleA.Views
{
    [Prism.Regions.ViewSortHint("9100")]
    /// <summary>
    /// Interaction logic for MA
    /// </summary>
    public partial class MA : UserControl
    {
        public MA()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var binding = ExTextBox.GetBindingExpression(TextBox.TextProperty);

            if (binding != null)
            {
                binding.UpdateSource();
            }
        }
    }
}
