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
        /// ����{�^���R�}���h���Ǘ�����ˑ��v���p�e�B
        /// </summary>
        public static readonly DependencyProperty CommandCloseProperty =
            DependencyProperty.Register(nameof(CommandClose), typeof(ICommand), typeof(BasePanel));

        /// <summary>
        /// �ő剻�{�^���R�}���h���Ǘ�����ˑ��v���p�e�B
        /// </summary>
        public static readonly DependencyProperty CommandMaximizeProperty =
            DependencyProperty.Register(nameof(CommandMaximize), typeof(ICommand), typeof(BasePanel));

        /// <summary>
        /// �ŏ����{�^���R�}���h���Ǘ�����ˑ��v���p�e�B
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
        /// ����{�^���R�}���h���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public ICommand CommandClose
        {
            get { return (ICommand)this.GetValue(CommandCloseProperty); }
            set { this.SetValue(CommandCloseProperty, value); }
        }

        /// <summary>
        /// �ő剻�{�^���R�}���h���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public ICommand CommandMaximize
        {
            get { return (ICommand)this.GetValue(CommandMaximizeProperty); }
            set { this.SetValue(CommandMaximizeProperty, value); }
        }

        /// <summary>
        /// �ŏ����{�^���R�}���h���擾�܂��͐ݒ肵�܂��B
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

            // �^�X�N�o�[�A�C�R���̐ݒ�
            var img = Imaging.LoadImageResource(@"Resources\Icon\icon.png");
            this._OwnerWindow.Icon = img;

            // WindowChrome���g�p���āA����SizeToContent��WidthAndHeight���w�肵���Ƃ���
            // Window�T�C�Y���������v�Z����Ȃ�WPF�̃o�O�ɑΉ����邽�߂�
            // Window�T�C�Y�̒������s��
            // +2���s���Ă���̂�BorderThickness = 1�̕������Z���邽�߁B
            this._OwnerWindow.RenderSize = new Size(this.ActualWidth, this.ActualHeight);
        }

        /// <summary>
        /// ENTER�L�[�����Ŏ��̃R���g���[���փt�H�[�J�X�ړ�������B
        /// PreviewKeyDown�̓g���l�����O�C�x���g�̂��߃{�^���R���g���[����
        /// KeyDown����Ƀn���h�����O�ł��邽�߁B
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
                // WindowChrome���g�p����Ɖ�ʂ̍ő剻���s�����Ƃ���
                // Window�̊O�g��������ʊO�ɂ͂ݏo�Ă��܂��B
                // ����̒������s���B
                this.Margin = new Thickness(4);
            }
            else
            {
                this.Margin = new Thickness(0);
            }
        }

        /// <summary>
        /// ����{�^���������Ɏ��s����郁�\�b�h
        /// </summary>
        private void Window_Close()
        {
            SystemCommands.CloseWindow(this._OwnerWindow);
        }

        /// <summary>
        /// �ő剻�{�^���������Ɏ��s����郁�\�b�h
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
        /// �ŏ����{�^���������Ɏ��s����郁�\�b�h
        /// </summary>
        private void Window_Minimize()
        {
            SystemCommands.MinimizeWindow(this._OwnerWindow);
        }
    }
}