using CommonModels;
using Prism.Events;
using Prism.Mvvm;

namespace Module1.Models
{
    //BindableBaseにしておくことで、直接画面とバインドできる
    //画面表示の為のデータ構造にはしない
    //画面表示の為に変換が必要な時にはViewModelでラップする
    //そのまま表示できるものは、わざわざViewModelでリレーするようなことはしない
    public class M1Model:BindableBase
    {
        public M1Model() { }

        public M1Model(CModel commondata, IEventAggregator ea)
        {

        }



    }
}
