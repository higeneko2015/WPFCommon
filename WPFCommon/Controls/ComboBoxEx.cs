using System.Windows;
using System.Windows.Controls;

namespace WPFCommon
{
    /// <summary>
    /// コンボボックスの基底クラス
    /// </summary>
    public class ComboBoxEx : ComboBox
    {
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static ComboBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxEx), new FrameworkPropertyMetadata(typeof(ComboBoxEx)));
        }
    }
}