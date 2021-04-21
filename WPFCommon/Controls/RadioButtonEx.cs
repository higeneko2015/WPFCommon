using System.Windows;
using System.Windows.Controls;

namespace WPFCommon
{
    /// <summary>
    /// ラジオボタンの基底クラス
    /// </summary>
    public class RadioButtonEx : RadioButton
    {
        ///// <summary>
        ///// エラーの有無を管理する依存プロパティ
        ///// </summary>
        //public static readonly DependencyProperty HasErrorProperty =
        //    DependencyProperty.Register(
        //        nameof(HasError),
        //        typeof(bool),
        //        typeof(RadioButtonEx));

        ///// <summary>
        ///// エラーの有無を取得または設定します。
        ///// </summary>
        //public bool HasError
        //{
        //    get { return (bool)this.GetValue(HasErrorProperty); }
        //    set { this.SetValue(HasErrorProperty, value); }
        //}

        /// <summary>
        /// クラスへの初回アクセス時に１度だけ実行される静的コンストラクタ
        /// </summary>
        static RadioButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonEx), new FrameworkPropertyMetadata(typeof(RadioButtonEx)));
        }
    }
}