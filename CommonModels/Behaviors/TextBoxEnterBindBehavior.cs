using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Data;
namespace CommonModels.Behaviors
{
    /// <summary>
    /// TextBoxでEnterが押された時にバインドを更新します
    /// </summary>
    [TypeConstraint(typeof(TextBox))]
    public class TextBoxEnterBindBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// 要素にアタッチされた時にイベントハンドラを登録
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
        }

        /// <summary>
        /// 要素にデタッチされた時にイベントハンドラを解除
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
        }

        /// <summary>
        /// キーダウン時のプレビューでKey.Enterを検出してUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // エンターキーが押されたら BindingをUpdateする
                TextBox tBox = (TextBox)sender;
                DependencyProperty prop = TextBox.TextProperty;
                BindingExpression binding
                 = BindingOperations.GetBindingExpression(tBox, prop);
                if (binding != null) { binding.UpdateSource(); }
            }
        }
    }
}
