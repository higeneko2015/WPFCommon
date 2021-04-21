using System.ComponentModel;
using System.Windows.Input;

namespace WPFCommon
{
    public class BasePanelViewModel : BaseViewModel
    {
        /// <summary>
        /// こ
        /// </summary>
        public BasePanelViewModel(IConfigService config, IShowMessageService message)
            : base(config, message)
        {
            this.F01ButtonCommand = new RelayCommand(this.F01Command, this.F01CanExecute);
            this.F02ButtonCommand = new RelayCommand(this.F02Command, this.F02CanExecute);
            this.F03ButtonCommand = new RelayCommand(this.F03Command, this.F03CanExecute);
            this.F04ButtonCommand = new RelayCommand(this.F04Command, this.F04CanExecute);
            this.F05ButtonCommand = new RelayCommand(this.F05Command, this.F05CanExecute);
            this.F06ButtonCommand = new RelayCommand(this.F06Command, this.F06CanExecute);
            this.F07ButtonCommand = new RelayCommand(this.F07Command, this.F07CanExecute);
            this.F08ButtonCommand = new RelayCommand(this.F08Command, this.F08CanExecute);
            this.F09ButtonCommand = new RelayCommand(this.F09Command, this.F09CanExecute);
            this.F10ButtonCommand = new RelayCommand(this.F10Command, this.F10CanExecute);
            this.F11ButtonCommand = new RelayCommand(this.F11Command, this.F11CanExecute);
            this.F12ButtonCommand = new RelayCommand(this.F12Command, this.F12CanExecute);
        }

        /// <summary>
        /// F01ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F01ButtonCommand { get; }

        /// <summary>
        /// F02ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F02ButtonCommand { get; }

        /// <summary>
        /// F03ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F03ButtonCommand { get; }

        /// <summary>
        /// F04ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F04ButtonCommand { get; }

        /// <summary>
        /// F05ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F05ButtonCommand { get; }

        /// <summary>
        /// F06ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F06ButtonCommand { get; }

        /// <summary>
        /// F07ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F07ButtonCommand { get; }

        /// <summary>
        /// F08ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F08ButtonCommand { get; }

        /// <summary>
        /// F09ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F09ButtonCommand { get; }

        /// <summary>
        /// F10ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F10ButtonCommand { get; }

        /// <summary>
        /// F11ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F11ButtonCommand { get; }

        /// <summary>
        /// F12ボタン押下時のコマンドを取得します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand F12ButtonCommand { get; }

        private bool F01CanExecute()
        {
            return true;
        }

        private void F01Command()
        {
            this.F01ButtonOnClick?.Invoke();
        }

        private bool F02CanExecute()
        {
            return true;
        }

        private void F02Command()
        {
            this.F02ButtonOnClick?.Invoke();
        }

        private bool F03CanExecute()
        {
            return true;
        }

        private void F03Command()
        {
            this.F03ButtonOnClick?.Invoke();
        }

        private bool F04CanExecute()
        {
            return true;
        }

        private void F04Command()
        {
            this.F04ButtonOnClick?.Invoke();
        }

        private bool F05CanExecute()
        {
            return true;
        }

        private void F05Command()
        {
            this.F05ButtonOnClick?.Invoke();
        }

        private bool F06CanExecute()
        {
            return true;
        }

        private void F06Command()
        {
            this.F06ButtonOnClick?.Invoke();
        }

        private bool F07CanExecute()
        {
            return true;
        }

        private void F07Command()
        {
            this.F07ButtonOnClick?.Invoke();
        }

        private bool F08CanExecute()
        {
            return true;
        }

        private void F08Command()
        {
            this.F08ButtonOnClick?.Invoke();
        }

        private bool F09CanExecute()
        {
            return true;
        }

        private void F09Command()
        {
            this.F09ButtonOnClick?.Invoke();
        }

        private bool F10CanExecute()
        {
            return true;
        }

        private void F10Command()
        {
            this.F10ButtonOnClick?.Invoke();
        }

        private bool F11CanExecute()
        {
            return true;
        }

        private void F11Command()
        {
            this.F11ButtonOnClick?.Invoke();
        }

        private bool F12CanExecute()
        {
            return true;
        }

        private void F12Command()
        {
            this.F12ButtonOnClick?.Invoke();
        }

        /// <summary>
        /// F01～F12ボタン押下時に実行されるハンドラのデリゲート定義
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate void FunctionButtonClickHandler();

        /// <summary>
        /// F01ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F01ButtonOnClick;

        /// <summary>
        /// F02ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F02ButtonOnClick;

        /// <summary>
        /// F03ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F03ButtonOnClick;

        /// <summary>
        /// F04ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F04ButtonOnClick;

        /// <summary>
        /// F05ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F05ButtonOnClick;

        /// <summary>
        /// F06ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F06ButtonOnClick;

        /// <summary>
        /// F07ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F07ButtonOnClick;

        /// <summary>
        /// F08ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F08ButtonOnClick;

        /// <summary>
        /// F09ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F09ButtonOnClick;

        /// <summary>
        /// F10ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F10ButtonOnClick;

        /// <summary>
        /// F11ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F11ButtonOnClick;

        /// <summary>
        /// F12ボタンクリック時に実行されるイベントハンドラ
        /// </summary>
        public event FunctionButtonClickHandler F12ButtonOnClick;
    }
}