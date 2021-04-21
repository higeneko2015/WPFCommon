using System.Windows;
using System.Windows.Controls;

namespace WPFCommon
{
    /// <summary>
    /// ラベルの基底クラス
    /// </summary>
    public class LabelEx : Label
    {
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static LabelEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelEx), new FrameworkPropertyMetadata(typeof(LabelEx)));
        }
    }
}
