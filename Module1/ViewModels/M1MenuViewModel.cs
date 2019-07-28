using Prism.Mvvm;

namespace Module1.ViewModels
{
    public class M1MenuViewModel : BindableBase
    {
        public M1MenuViewModel()
        {

        }

        public M1MenuViewModel(M1ViewModel VM)
        {
            VM1 = VM;
        }

        private M1ViewModel vM1;
        /// <summary>
        /// M9ViewModel
        /// </summary>
        public M1ViewModel VM1
        {
            get { return vM1; }
            set { SetProperty(ref vM1, value); }
        }
    }
}
