using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFCommon
{
    /// <summary>
    /// ボタンの基底クラス
    /// </summary>
    public class ButtonEx : Button
    {
        // ボタン押下処理実行中判定用フラグ(true:実行中, false:実行中でない)
        private bool IsProcessing = false;

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static ButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx)));
        }

        /// <summary>
        /// Clickイベントハンドラ
        /// ボタンクリック時の処理を実行します。
        /// ボタンを連続して押下したときにクリック処理が連続して発生しないように制御しています。
        /// </summary>
        protected override void OnClick()
        {
            if (this.IsProcessing)
            {
                return;
            }

            this.IsProcessing = true;

            // ボタン押下処理が終了(Idleになった)したときにフラグを戻す
            this.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    this.IsProcessing = false;
                }), DispatcherPriority.ApplicationIdle);

            base.OnClick();
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnMouseDoubleClick(e);
        }

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnPreviewMouseDoubleClick(e);
        }
    }
}