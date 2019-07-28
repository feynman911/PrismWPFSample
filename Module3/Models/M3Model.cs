using CommonModels;
using Prism.Events;
using Prism.Mvvm;

namespace Module3.Models
{
    //BindableBaseにしておくことで、直接画面とバインドできる
    //画面表示の為のデータ構造にはしない
    //画面表示の為に変換が必要な時にはViewModelでラップする
    //そのまま表示できるものは、わざわざViewModelでリレーするようなことはしない
    public class M3Model:BindableBase
    {
        public M3Model() { }

        public M3Model(CModel commondata, IEventAggregator ea)
        {

        }



    }
}
