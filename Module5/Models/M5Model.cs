using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Events;
using System.Diagnostics;
using CommonModels;

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
