using Prism.Mvvm;
using Prism.Events;
using CommonModels;

namespace Module9.ViewModels
{
    public class M9MenuViewModel : BindableBase
    {
        public M9MenuViewModel()
        {

        }

        //CModel commonModel, IEventAggregator ea,
        public M9MenuViewModel(M9ViewModel VM)
        {
            VM9 = VM;
        }

        private M9ViewModel vM9 ;
        /// <summary>
        /// M9ViewModel
        /// </summary>
        public M9ViewModel VM9
        {
            get { return vM9; }
            set { SetProperty(ref vM9, value); }
        }


    }
}
