using CommonModels;
using Prism.Events;
using Prism.Mvvm;

namespace Module5.Models
{
    //BindableBaseにしておくことで、直接画面とバインドできる
    //画面表示の為のデータ構造にはしない
    //画面表示の為に変換が必要な時にはViewModelでラップする
    //そのまま表示できるものは、わざわざViewModelでリレーするようなことはしない
    public class M5Model:BindableBase
    {
        public M5Model() { }

        public M5Model(CModel commondata, IEventAggregator ea)
        {

        }



    }
}
