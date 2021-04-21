using System.Windows;
using System.Windows.Controls;

namespace WPFCommon
{
    /// <summary>
    /// チェックボックスの基底クラス
    /// </summary>
    public class CheckBoxEx : CheckBox
    {
        ///// <summary>
        ///// コントロールの入力値にエラーがある時にtrue、エラーがないときにfalseを返却します。
        ///// </summary>
        //public static readonly DependencyProperty HasErrorProperty =
        //    DependencyProperty.Register(
        //        nameof(HasError),
        //        typeof(bool),
        //        typeof(CheckBoxEx));

        ///// <summary>
        ///// コントロールの入力値にエラーがある時にtrue、エラーがないときにfalseを返却します。
        ///// </summary>
        //public bool HasError
        //{
        //    get { return (bool)this.GetValue(HasErrorProperty); }
        //    set { this.SetValue(HasErrorProperty, value); }
        //}

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static CheckBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBoxEx), new FrameworkPropertyMetadata(typeof(CheckBoxEx)));
        }
    }
}