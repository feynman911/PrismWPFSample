using Prism.Mvvm;

namespace Module2.ViewModels
{
    public class M2MenuViewModel : BindableBase
    {
        public M2MenuViewModel()
        {

        }

        public M2MenuViewModel(M2ViewModel VM)
        {
            VM2 = VM;
        }

        private M2ViewModel vM2;
        /// <summary>
        /// M9ViewModel
        /// </summary>
        public M2ViewModel VM2
        {
            get { return vM2; }
            set { SetProperty(ref vM2, value); }
        }

    }
}
