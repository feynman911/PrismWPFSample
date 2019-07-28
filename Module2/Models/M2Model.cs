using CommonModels;
using Prism.Events;
using Prism.Mvvm;

namespace Module2.Models
{
    //BindableBaseにしておくことで、直接画面とバインドできる
    //画面表示の為のデータ構造にはしない
    //画面表示の為に変換が必要な時にはViewModelでラップする
    //そのまま表示できるものは、わざわざViewModelでリレーするようなことはしない
    //Modelなしでの画面作成（コードと画面の同時作成）を行う時には、すべてViewModelにある必要がある

    public class M2Model:BindableBase
    {
        public M2Model() { }

        public M2Model(CModel cModel, IEventAggregator ea)
        {

        }



    }
}
