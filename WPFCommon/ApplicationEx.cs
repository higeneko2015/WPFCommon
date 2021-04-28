using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WPFCommon
{
    public partial class ApplicationEx : Application
    {
        private static readonly ServiceManager _Service = new();

        public ApplicationEx()
        {
            // CLRのスレッドプールは、スレッドの数を無駄に増やしてしまわないよう、
            // 新しいスレッドが必要になった場合は、500ミリ秒に1つの割合を
            // 超えないペースでスレッドを生成する設計になっている。
            // 500ミリ待つ仕様を回避するために同時生成スレッド数を20に設定してみる。
            ThreadPool.GetMinThreads(out _, out var ioMin);
            ThreadPool.SetMinThreads(20, ioMin);

            _Service.AddSingleton<IConfigService, ConfigService>();
            _Service.AddSingleton<IDatabaseService, DatabaseService>();
            //ServiceList.AddSingleton<ILoggerService, LoggerService>();
            _Service.AddSingleton<IMessageService, MessageService>();
            _Service.AddSingleton<IShowMessageService, ShowMessageService>();
            //srv.AddSingleton<IWindowService, WindowService>();
            //srv.AddSingleton<IProgramService, ProgramService>();
            //srv.AddSingleton<IFileDialogService, FileDialogService>();
            //srv.AddSingleton<ICsvService, CsvService>();
            _Service.Build();
        }

        public static ServiceManager Service
        {
            get { return _Service; }
        }

        public static T GetService<T>()
        {
            return _Service.GetService<T>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DispatcherUnhandledException -= this.ApplicationEx_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException -= this.TaskScheduler_UnobservedTaskException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // UIスレッドで発生した未処理例外
            DispatcherUnhandledException += this.ApplicationEx_DispatcherUnhandledException;
            // バックグラウンドスレッドで発生した未処理例外
            TaskScheduler.UnobservedTaskException += this.TaskScheduler_UnobservedTaskException;

            // 並列実行することで0.16秒→0.08秒へ高速化(デバッグモード時の計測)
            Parallel.Invoke(
                () => this.SetCulture(),
                () => this.SetTooltipTime()
            );
            //// 設定ファイルから言語を読み取って設定する
            //var config = GetService<IConfigService>();
            //var culture = new CultureInfo(config.SystemLanguage, false);
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;

            //// ツールチップの表示時間を無限(約24日)にする
            //ToolTipService.ShowDurationProperty.OverrideMetadata(
            //    typeof(DependencyObject),
            //    new FrameworkPropertyMetadata(int.MaxValue));

            //// 実行時に外部のフォントが読み込まれないので強制的にロードする
            //var location = Assembly.GetExecutingAssembly().Location;
            //var path = Path.GetDirectoryName(location);

            //var fontPath = Path.Combine(path, @"Resources\Font\");
            //var fontFile = $"{fontPath}#{config.DefaultFont}";

            //Current.Resources["Default.FontFamily"] = new FontFamily(fontFile);

            // 派生先クラスのStartupイベントハンドラを実行
            base.OnStartup(e);
        }

        private void ApplicationEx_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.ExceptionHandling(e.Exception);
            // 例外処理済みにする
            e.Handled = true;
        }

        /// <summary>
        /// 例外情報をログに出力しメッセージを表示します。
        /// </summary>
        /// <param name="e">例外内容</param>
        private void ExceptionHandling(Exception e)
        {
            var errMsg = this.GetExceptionInfoString(e);
            var msgBox = GetService<IShowMessageService>();
            var config = GetService<IConfigService>();
            msgBox.Show("00001", config.SystemLanguage, errMsg.Item1);
            //            Logger.Error(this.GetExceptionInfoString(e));
        }

        /// <summary>
        /// 例外の内容を文字列として編集した値を返却します。
        /// </summary>
        /// <param name="ex">例外情報</param>
        /// <returns>例外の内容を文字列に編集した内容</returns>
        private (string, string) GetExceptionInfoString(Exception ex)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Message={0}\n", ex.Message);
            sb.AppendFormat("Source={0}\n", ex.Source);
            sb.AppendFormat("HelpLink={0}\n", ex.HelpLink);
            sb.AppendFormat("TargetSite={0}\n", ex.TargetSite.ToString());
            sb.AppendFormat("StackTrace={0}\n", ex.StackTrace);

            return (ex.Message, sb.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] // 積極的にinline化されるように。
        private void SetCulture()
        {
            var config = GetService<IConfigService>();
            var culture = new CultureInfo(config.SystemLanguage, false);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        // General.xamlにまとまったことでこの設定が不要になった？
        //[MethodImpl(MethodImplOptions.AggressiveInlining)] // 積極的にinline化されるように。
        //private void SetDefaultFont()
        //{
        //    //var config = GetService<IConfigService>();
        //    //// 実行時に外部のフォントが読み込まれないので強制的にロードする
        //    //var location = Assembly.GetExecutingAssembly().Location;
        //    //var path = Path.GetDirectoryName(location);

        //    //var fontPath = Path.Combine(path, @"Resources\Font\");
        //    //var fontFile = $"{fontPath}#{config.DefaultFont}";
        //    //Current.Resources["Default.FontFamily"] = new FontFamily(fontFile);
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)] // 積極的にinline化されるように。
        private void SetTooltipTime()
        {
            // ツールチップの表示時間を無限(約24日)にする
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject),
                new FrameworkPropertyMetadata(int.MaxValue));
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            this.ExceptionHandling(e.Exception);
            // アプリを強制終了させないための記述
            e.SetObserved();
        }
    }
}