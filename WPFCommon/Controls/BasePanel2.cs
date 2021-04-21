using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFCommon
{
    public class BasePanel2 : ContentControl
    {
        public static readonly DependencyProperty AppVersionProperty =
            DependencyProperty.Register(nameof(AppVersion), typeof(string), typeof(BasePanel2), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

        public static readonly DependencyProperty F01ButtonCommandProperty =
                                    DependencyProperty.Register(nameof(F01ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F02ButtonCommandProperty =
            DependencyProperty.Register(nameof(F02ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F03ButtonCommandProperty =
            DependencyProperty.Register(nameof(F03ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F04ButtonCommandProperty =
            DependencyProperty.Register(nameof(F04ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F05ButtonCommandProperty =
            DependencyProperty.Register(nameof(F05ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F06ButtonCommandProperty =
            DependencyProperty.Register(nameof(F06ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F07ButtonCommandProperty =
            DependencyProperty.Register(nameof(F07ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F08ButtonCommandProperty =
            DependencyProperty.Register(nameof(F08ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F09ButtonCommandProperty =
            DependencyProperty.Register(nameof(F09ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F10ButtonCommandProperty =
            DependencyProperty.Register(nameof(F10ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F11ButtonCommandProperty =
            DependencyProperty.Register(nameof(F11ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty F12ButtonCommandProperty =
            DependencyProperty.Register(nameof(F12ButtonCommand), typeof(ICommand), typeof(BasePanel2), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ScreenNameProperty =
            DependencyProperty.Register(nameof(ScreenName), typeof(string), typeof(BasePanel2), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private Window _OwnerWindow = null;

        static BasePanel2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BasePanel2), new FrameworkPropertyMetadata(typeof(BasePanel2)));
        }

        public BasePanel2()
        {
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

        [Category("拡張プロパティ"), Description("タイトルバーに表示する画面のバージョンを取得または設定します。")]
        public string AppVersion
        {
            get { return (string)this.GetValue(AppVersionProperty); }
            set { this.SetValue(AppVersionProperty, value); }
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

        [Category("拡張プロパティ"), Description("F01ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F01ButtonCommand
        {
            get { return (ICommand)this.GetValue(F01ButtonCommandProperty); }
            set { this.SetValue(F01ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F02ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F02ButtonCommand
        {
            get { return (ICommand)this.GetValue(F02ButtonCommandProperty); }
            set { this.SetValue(F02ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F03ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F03ButtonCommand
        {
            get { return (ICommand)this.GetValue(F03ButtonCommandProperty); }
            set { this.SetValue(F03ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F04ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F04ButtonCommand
        {
            get { return (ICommand)this.GetValue(F04ButtonCommandProperty); }
            set { this.SetValue(F04ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F05ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F05ButtonCommand
        {
            get { return (ICommand)this.GetValue(F05ButtonCommandProperty); }
            set { this.SetValue(F05ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F06ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F06ButtonCommand
        {
            get { return (ICommand)this.GetValue(F06ButtonCommandProperty); }
            set { this.SetValue(F06ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F07ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F07ButtonCommand
        {
            get { return (ICommand)this.GetValue(F07ButtonCommandProperty); }
            set { this.SetValue(F07ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F08ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F08ButtonCommand
        {
            get { return (ICommand)this.GetValue(F08ButtonCommandProperty); }
            set { this.SetValue(F08ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F09ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F09ButtonCommand
        {
            get { return (ICommand)this.GetValue(F09ButtonCommandProperty); }
            set { this.SetValue(F09ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F10ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F10ButtonCommand
        {
            get { return (ICommand)this.GetValue(F10ButtonCommandProperty); }
            set { this.SetValue(F10ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F11ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F11ButtonCommand
        {
            get { return (ICommand)this.GetValue(F11ButtonCommandProperty); }
            set { this.SetValue(F11ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("F12ボタン押下時に実行するコマンドを取得または設定します。")]
        public ICommand F12ButtonCommand
        {
            get { return (ICommand)this.GetValue(F12ButtonCommandProperty); }
            set { this.SetValue(F12ButtonCommandProperty, value); }
        }

        [Category("拡張プロパティ"), Description("タイトルバーに表示する画面名を取得または設定します。")]
        public string ScreenName
        {
            get { return (string)this.GetValue(ScreenNameProperty); }
            set { this.SetValue(ScreenNameProperty, value); }
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
