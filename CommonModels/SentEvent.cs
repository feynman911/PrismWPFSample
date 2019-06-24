using Prism.Events;

namespace CommonModels
{
    public class MessageSentEvent : PubSubEvent<string>
    {
    }

    public class DoubleSentEvent : PubSubEvent<double>
    {
    }

    //言語設定が変わったことの通知
    //XAMLに{lex:Loc MESSAGE}で埋め込まれた言葉は即変わるが
    //codeで設定されたところは変わらないので、このイベントで再設定する
    public class LanguageChangeEvent : PubSubEvent
    {

    }
}
