using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFCommon
{
    /// <summary>
    /// MessageWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageWindow : Window
    {
        /// <summary>
        /// メッセージボックスのボタンのタイプ。
        /// メッセージボックスを閉じるときに戻り値を決定するために使用する。
        /// </summary>
        private readonly MessageBoxButton _buttonType = MessageBoxButton.OK;

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        /// <param name="imageType">アイコン種別</param>
        /// <param name="buttonType">ボタン種別</param>
        public MessageWindow(Window owner, string msg, string msgCd, MessageBoxImage imageType, MessageBoxButton buttonType)
        {
            this.InitializeComponent();
            this.Loaded += this.MessageBoxBase_Loaded;
            this.Closed += this.MessageWindow_Closed;
            this.Owner = owner;
            this.LblMessage.Content = msg;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this._buttonType = buttonType;
            this.Icon = null;

            var img = Imaging.LoadImageResource(@"Resources\Icon\icon.png");
            this.IconImage.Source = img;

            // ボタンの状態を設定
            switch (buttonType)
            {
                case MessageBoxButton.YesNo:
                    this.Btn3.IsEnabled = false;
                    this.Btn3.Visibility = Visibility.Hidden;
                    this.Btn1.Margin = new Thickness(this.Btn1.Margin.Left, this.Btn1.Margin.Top, 120, this.Btn1.Margin.Bottom);
                    this.Btn2.Margin = new Thickness(this.Btn2.Margin.Left, this.Btn2.Margin.Top, 10, this.Btn2.Margin.Bottom);
                    this.Btn1.Content = "はい(_Y)";
                    this.Btn2.Content = "いいえ(_N)";
                    this.Btn2.Focus();
                    break;

                case MessageBoxButton.YesNoCancel:
                    this.Btn1.Content = "はい(_Y)";
                    this.Btn2.Content = "いいえ(_N)";
                    this.Btn3.Content = "CANCEL(_C)";
                    this.Btn3.Focus();
                    break;

                case MessageBoxButton.OKCancel:
                    this.Btn2.IsEnabled = false;
                    this.Btn2.Visibility = Visibility.Hidden;
                    this.Btn1.Margin = new Thickness(this.Btn1.Margin.Left, this.Btn1.Margin.Top, 120, this.Btn1.Margin.Bottom);
                    this.Btn1.Content = "OK(_O)";
                    this.Btn3.Content = "CANCEL(_C)";
                    this.Btn3.Focus();
                    break;

                default:
                    // OKボタンのみ
                    this.Btn2.IsEnabled = false;
                    this.Btn2.Visibility = Visibility.Hidden;
                    this.Btn3.IsEnabled = false;
                    this.Btn3.Visibility = Visibility.Hidden;
                    this.Btn1.Margin = new Thickness(this.Btn1.Margin.Left, this.Btn1.Margin.Top, 10, this.Btn1.Margin.Bottom);
                    this.Btn1.Content = "OK(_O)";
                    this.Btn1.Focus();
                    break;
            }

            // アイコン画像を設定
            switch (imageType)
            {
                case MessageBoxImage.Error:
                    this.ImgIcon.Source = Imaging.LoadResource(@"Resources\Icon\MessageBox.Error.svg");
                    this.LblTitle.Content = GetTitleMessage(MessageBoxImage.Error, msgCd);
                    break;

                case MessageBoxImage.Information:
                    this.ImgIcon.Source = Imaging.LoadResource(@"Resources\Icon\MessageBox.Information.svg");
                    this.LblTitle.Content = GetTitleMessage(MessageBoxImage.Information, msgCd);
                    break;

                case MessageBoxImage.Question:
                    this.ImgIcon.Source = Imaging.LoadResource(@"Resources\Icon\MessageBox.Question.svg");
                    this.LblTitle.Content = GetTitleMessage(MessageBoxImage.Question, msgCd);
                    break;

                case MessageBoxImage.Warning:
                    this.ImgIcon.Source = Imaging.LoadResource(@"Resources\Icon\MessageBox.Warning.svg");
                    this.LblTitle.Content = GetTitleMessage(MessageBoxImage.Warning, msgCd);
                    break;
            }
        }

        /// <summary>
        /// メッセージボックスの戻り値を取得または設定します。
        /// </summary>
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

        private static string GetMessageTypeString(MessageBoxImage imageType)
        {
            switch (imageType)
            {
                case MessageBoxImage.Error:
                    return "エラー";

                case MessageBoxImage.Information:
                    return "情報";

                case MessageBoxImage.Question:
                    return "確認";

                case MessageBoxImage.Warning:
                    return "警告";

                default:
                    return "";
            }
        }

        private static string GetTitleMessage(MessageBoxImage imageType, string msgCd)
        {
            if (msgCd.IsEmpty())
            {
                return GetMessageTypeString(imageType);
            }
            return string.Format("{0}[{1}]", GetMessageTypeString(imageType), msgCd);
        }

        /// <summary>
        /// 「OK/はい」ボタン押下時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            this.Result = (this._buttonType == MessageBoxButton.OK || this._buttonType == MessageBoxButton.OKCancel) ? MessageBoxResult.OK : MessageBoxResult.Yes;
            this.Close();
        }

        /// <summary>
        /// 「いいえ」ボタン押下時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.No;
            this.Close();
        }

        /// <summary>
        /// 「キャンセル」ボタン押下時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Loadedイベント発生時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void MessageBoxBase_Loaded(object sender, RoutedEventArgs e)
        {
            var content = this.Content as Grid;
            this.RenderSize = new Size(content.ActualWidth + 2, content.ActualHeight + 2);
        }

        /// <summary>
        /// メッセージボックス閉じるときに呼び出し元Windowをアクティブにする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageWindow_Closed(object sender, EventArgs e)
        {
            this.Owner.Activate();
        }
    }
}