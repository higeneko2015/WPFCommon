using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFCommon
{
    /// <summary>
    /// <para>ICommand用の基底クラス</para>
    /// <para>Commandを定義するときには本クラスを基底クラスとして指定してください。</para>
    /// <para>Commandの実行はTaskとして実行されます。</para>
    /// <para>CommandParameterが未設定の時はCommand実行時にWaiting Circleを表示し、
    /// Windowの項目を操作不能にします。</para>
    /// </summary>
    public class RelayCommand : ICommand
    {
        // 呼び出し元クラス名
        private readonly string _calledClassName = null;

        // 呼び出し元のファイル名(フルパス)
        private readonly string _calledFileName = null;

        // コマンドの実行可否を判定するメソッド
        private readonly Func<bool> _canExecute = null;

        // コマンドの実行時に実行されるメソッド
        private readonly Action _execute;

        /// <summary>
        /// <para>引数のActionを実行するためのCommandを生成します。</para>
        /// <para>本コンストラクタを使用したCommandは常に実行可能となります。</para>
        /// </summary>
        /// <param name="execute">Command実行時に実行されるAction</param>
        /// <param name="filePath">呼び出し元クラスのファイル名</param>
        /// <param name="memberName">呼び出し元メソッド名</param>
        /// <example>
        /// <code>
        /// public class SampleViewModel : BaseViewModel
        /// {
        ///     private string _SearchScreenID;
        ///     public string SearchScreenID
        ///     {
        ///         get { return _SearchScreenID; }
        ///         set { SetProperty(ref _SearchScreenID, value); }
        ///     }
        ///
        ///     // Commandを格納する変数定義
        ///     public ICommand SearchButtonCommand { get; }
        ///
        ///     // Command実行時に実行されるActionの定義
        ///     private void SearchButtonExecute()
        ///     {
        ///         this.SearchScreenID = this.SearchScreenID + "x";
        ///     }
        ///
        ///     public SampleViewModel()
        ///     {
        ///         // ActionとCommandの紐付けを行う
        ///         this.SearchButtonCommand = new RelayCommand(SearchButtonExecute);
        ///     }
        /// }
        /// </code>
        /// </example>
        public RelayCommand(Action execute, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
            : this(execute, null, filePath, memberName)
        {
        }

        /// <summary>
        /// <para>引数のActionとFunctionを使用してCommandを生成します。</para>
        /// <para>本コンストラクタを使用したCommandの実行可否はFunctionの判定結果に依存します。</para>
        /// </summary>
        /// <param name="execute">実行するCommandの本体</param>
        /// <param name="canExecute">Commandの実行可否を判定するFunctionの本体</param>
        /// <param name="filePath">呼び出し元クラスのファイル名</param>
        /// <param name="memberName">呼び出し元メソッド名</param>
        /// <example>
        /// <code>
        /// public class SampleViewModel : BaseViewModel
        /// {
        ///     private string _SearchScreenID;
        ///     public string SearchScreenID
        ///     {
        ///         get { return _SearchScreenID; }
        ///         set { SetProperty(ref _SearchScreenID, value); }
        ///     }
        ///
        ///     // Commandを格納する変数定義
        ///     public ICommand SearchButtonCommand { get; }
        ///
        ///     // Command実行時に実行されるActionの定義
        ///     private void SearchButtonExecute()
        ///     {
        ///         this.SearchScreenID = this.SearchScreenID + "x";
        ///     }
        ///
        ///     // Commandの実行可否を判定するFunction定義
        ///     private bool CanExecuteSearchButton()
        ///     {
        ///         if (this.SearchScreenID.IsEmpty() == false)
        ///         {
        ///             return true;
        ///         }
        ///         else
        ///         {
        ///             return false;
        ///         }
        ///     }
        ///
        ///     public SampleViewModel()
        ///     {
        ///         // ActionとCommandの紐付けを行う
        ///         this.SearchButtonCommand = new RelayCommand(SearchButtonExecute, CanExecuteSearchButton);
        ///     }
        /// }
        /// </code>
        /// </example>
        public RelayCommand(Action execute, Func<bool> canExecute, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            if (execute.Target is BasePanel)
            {
                // 呼び出し元がWPFCommonのMainPanelの場合は上位Windowのクラス名をセットする。
                // 名前空間付きで取得されるのでクラス名部分だけを取り出す。
                //var windowName = (execute.Target as BasePanel)?.OwnerWindow?.ToString();
                //this._calledClassName = Path.GetExtension(windowName).AsSpan(1);
            }
            else
            {
                // 呼び出し元がMainPanel以外の場合はVMから呼ばれたものと判断して
                // VMのファイル名(フルパス)をセットする。
                // VMのファイル名＝クラス名という前提
                this._calledFileName = filePath;
            }

            //            this._execute = execute ?? throw new SystemErrorException(filePath, memberName, "00005");
            this._execute = execute;
            this._canExecute = canExecute;
        }

        /// <summary>
        /// <para>RequerySuggestedイベントのハンドラ</para>
        /// <para>キーボードフォーカスの変更など、コマンドを実行する必要があるかどうかを
        /// 変更する条件が満たされたときに発生します。</para>
        /// <para>任意のタイミングで本イベントを発生させる場合は
        /// CommandManager.InvalidateRequerySuggested()を実行する必要があります。</para>
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private string CalledFileOrClassName
        {
            get
            {
                if (this._calledFileName.IsEmpty() == false)
                {
                    return this._calledFileName;
                }
                else
                {
                    return this._calledClassName;
                }
            }
        }

        /// <summary>
        /// <para>フレームワークがCommandの実行可否判定が必要になったと判断した時に
        /// 自動的に呼び出されるメソッド</para>
        /// </summary>
        /// <param name="parameter">Commandの実行可否判定に外部パラメータが必要なときに指定するパラメータ値(未使用)</param>
        /// <returns>
        /// <para>コンストラクタで実行可否判定Functionを指定した場合はそのFunctionの戻り値</para>
        /// <para>Functionを指定しなかった場合は常にtrue</para>
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute();
        }

        /// <summary>
        /// <para>Commandの実行時に本メソッドが呼び出されて</para>
        /// <para>コンストラクタの引数で指定したActionを実行します。</para>
        /// <para>Actionの実行前後に自動でLogを出力します。</para>
        /// </summary>
        /// <param name="parameter">Commandの実行に外部パラメータが必要なときに指定するパラメータ値(未使用)
        /// <para>未指定の場合：ActionをUIスレッドで実行します。</para>
        /// <para>指定した場合：ActionをTaskとして実行します。</para>
        /// <para>また、Action実行後１秒が経過してもActionが完了しない場合はWaitingCircleを表示します。</para>
        /// </param>
        public async void Execute(object parameter)
        {
            //            Logger.Info("[Command]:開始", this.CalledFileOrClassName, this._execute.Method.Name);
            if (parameter.IsEmpty())
            {
                var timer = new DispatcherTimer();
                timer.Tick += this.Timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
                await Task.Run(() => this._execute());
                timer.Stop();
                timer.Tick -= this.Timer_Tick;
                //                AppEx.CurrentWindow?.RaiseWaitingStateChange(Visibility.Collapsed);
            }
            else
            {
                this._execute();
            }

            //            Logger.Info("[Command]:完了", this.CalledFileOrClassName, this._execute.Method.Name);
        }

        /// <summary>
        /// <para>ActionのTask実行時にタイマー処理として実行されるメソッド</para>
        /// <para>WaitingCircleを画面上に表示します。</para>
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            //            AppEx.CurrentWindow?.RaiseWaitingStateChange(Visibility.Visible);
        }
    }
}