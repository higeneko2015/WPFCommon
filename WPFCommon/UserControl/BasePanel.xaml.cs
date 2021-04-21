using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFCommon
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BasePanel : UserControl
    {
        /// <summary>
        /// 閉じるボタンコマンドを管理する依存プロパティ
        /// </summary>
        public static readonly DependencyProperty CommandCloseProperty =
            DependencyProperty.Register(nameof(CommandClose), typeof(ICommand), typeof(BasePanel));

        /// <summary>
        /// 最大化ボタンコマンドを管理する依存プロパティ
        /// </summary>
        public static readonly DependencyProperty CommandMaximizeProperty =
            DependencyProperty.Register(nameof(CommandMaximize), typeof(ICommand), typeof(BasePanel));

        /// <summary>
        /// 最小化ボタンコマンドを管理する依存プロパティ
        /// </summary>
        public static readonly DependencyProperty CommandMinimizeProperty =
            DependencyProperty.Register(nameof(CommandMinimize), typeof(ICommand), typeof(BasePanel));

        private Window _OwnerWindow = null;

        public BasePanel()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            this.CommandMinimize = new RelayCommand(this.Window_Minimize);
            this.CommandMaximize = new RelayCommand(this.Window_Maximize);
            this.CommandClose = new RelayCommand(this.Window_Close);

            this.Loaded += this.BasePanel_Loaded;
            this.PreviewKeyDown += this.BasePanel_PreviewKeyDown;
            this.Unloaded += this.BasePanel_Unloaded;
        }

        /// <summary>
        /// 閉じるボタンコマンドを取得または設定します。
        /// </summary>
        public ICommand CommandClose
        {
            get { return (ICommand)this.GetValue(CommandCloseProperty); }
            set { this.SetValue(CommandCloseProperty, value); }
        }

        /// <summary>
        /// 最大化ボタンコマンドを取得または設定します。
        /// </summary>
        public ICommand CommandMaximize
        {
            get { return (ICommand)this.GetValue(CommandMaximizeProperty); }
            set { this.SetValue(CommandMaximizeProperty, value); }
        }

        /// <summary>
        /// 最小化ボタンコマンドを取得または設定します。
        /// </summary>
        public ICommand CommandMinimize
        {
            get { return (ICommand)this.GetValue(CommandMinimizeProperty); }
            set { this.SetValue(CommandMinimizeProperty, value); }
        }

        public Window OwnerWindow
        {
            get { return _OwnerWindow; }
        }

        private void BasePanel_Loaded(object sender, RoutedEventArgs e)
        {
            this._OwnerWindow = Window.GetWindow(this);
            this._OwnerWindow.StateChanged += this.OwnerWindow_StateChanged;
            this._OwnerWindow.Closing += this.OwnerWindow_Closing;

            // タスクバーアイコンの設定
            var img = Imaging.LoadImageResource(@"Resources\Icon\icon.png");
            this._OwnerWindow.Icon = img;

            // WindowChromeを使用して、かつSizeToContentにWidthAndHeightを指定したときに
            // Windowサイズが正しく計算されないWPFのバグに対応するために
            // Windowサイズの調整を行う
            // +2を行っているのはBorderThickness = 1の分を加算するため。
            this._OwnerWindow.RenderSize = new Size(this.ActualWidth, this.ActualHeight);
        }

        /// <summary>
        /// ENTERキー押下で次のコントロールへフォーカス移動させる。
        /// PreviewKeyDownはトンネリングイベントのためボタンコントロールの
        /// KeyDownより先にハンドリングできるため。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasePanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var direction = Keyboard.Modifiers == ModifierKeys.Shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next;
            (FocusManager.GetFocusedElement(this._OwnerWindow) as FrameworkElement)?.MoveFocus(new TraversalRequest(direction));
            e.Handled = true;
        }

        private void BasePanel_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.BasePanel_Loaded;
            this.PreviewKeyDown -= this.BasePanel_PreviewKeyDown;
            this.Unloaded -= this.BasePanel_Unloaded;
        }

        private void OwnerWindow_Closing(object sender, CancelEventArgs e)
        {
            var msg = ApplicationEx.GetService<IShowMessageService>();
            var ret = msg.Show("00009", CultureInfo.CurrentCulture.Name);
            if (ret == MessageBoxResult.Yes)
            {
                ((Window)sender).DataContext = null;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void OwnerWindow_StateChanged(object sender, System.EventArgs e)
        {
            var window = sender as Window;
            if (window.IsEmpty())
            {
                return;
            }
            if (window.WindowState == WindowState.Maximized)
            {
                // WindowChromeを使用すると画面の最大化を行ったときに
                // Windowの外枠部分が画面外にはみ出てしまう。
                // それの調整を行う。
                this.Margin = new Thickness(4);
            }
            else
            {
                this.Margin = new Thickness(0);
            }
        }

        /// <summary>
        /// 閉じるボタン押下時に実行されるメソッド
        /// </summary>
        private void Window_Close()
        {
            SystemCommands.CloseWindow(this._OwnerWindow);
        }

        /// <summary>
        /// 最大化ボタン押下時に実行されるメソッド
        /// </summary>
        private void Window_Maximize()
        {
            if (this._OwnerWindow.WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(this._OwnerWindow);
            }
            else
            {
                SystemCommands.MaximizeWindow(this._OwnerWindow);
            }
        }

        /// <summary>
        /// 最小化ボタン押下時に実行されるメソッド
        /// </summary>
        private void Window_Minimize()
        {
            SystemCommands.MinimizeWindow(this._OwnerWindow);
        }
    }
}