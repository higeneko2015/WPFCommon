using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;

namespace WPFCommon
{
    /// <summary>
    /// Common.configファイル内を定義値を照会するためのクラス
    /// </summary>
    public class ConfigService : IConfigService
    {
        // configファイル名
        private const string _ConfigFile = @"Resources\common.config";

        // 独自のconfigファイル読み込み用設定
        private readonly ExeConfigurationFileMap _ExeFileMap = null;

        private readonly string _FullPathConfigFile = default;

        // ConnectionStringsプロパティのためのパッキング変数
        private string _ConnectionStrings = default;

        private string _DefaultFont = default;

        // configファイルの最終更新日(再読み込み判定用)
        private DateTime _LastUpdateDate = default;

        // システムのデフォルト言語用のパッキング変数
        private string _SystemLanguage = default;

        /// <summary>
        /// configファイルから各種設定情報をプロパティにセットします。
        /// </summary>
        public ConfigService()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(location);
            this._FullPathConfigFile = Path.Combine(path, _ConfigFile);
            this._ExeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = this._FullPathConfigFile };
            ReloadConfigFile();
        }

        /// <summary>
        /// <para>データベース接続文字列を取得します。</para>
        /// <para>常に最新の定義内容を取得します。</para>
        /// </summary>
        /// <example>
        /// <code>
        /// var connString = CommonConfig.ConnectionStrings;
        /// </code>
        /// </example>
        public string ConnectionStrings
        {
            get
            {
                ReloadConfigFile();
                return _ConnectionStrings;
            }
        }

        public string DefaultFont
        {
            get
            {
                ReloadConfigFile();
                return _DefaultFont;
            }
        }

        /// <summary>
        /// <para>システムのデフォルト言語を取得します。</para>
        /// <para>常に最新の定義内容を取得します。</para>
        /// </summary>
        /// <example>
        /// <code>
        /// var systemLang = CommonConfig.SystemLanguage;
        /// </code>
        /// </example>
        public string SystemLanguage
        {
            get
            {
                ReloadConfigFile();
                return _SystemLanguage;
            }
        }

        /// <summary>
        /// ファイルが更新されていた場合に各種設定情報を再取得してプロパティにセットし直します。
        /// </summary>
        private void ReloadConfigFile()
        {
            var checkDate = File.GetLastWriteTime(this._FullPathConfigFile);
            if (checkDate <= _LastUpdateDate)
            {
                return;
            }

            _LastUpdateDate = checkDate;

            try
            {
                var config = ConfigurationManager.OpenMappedExeConfiguration(_ExeFileMap, ConfigurationUserLevel.None);

                // Configファイルに値をを追加した場合はここに取得ロジックを記載する
                _ConnectionStrings = config?.AppSettings?.Settings[nameof(ConnectionStrings)]?.Value;
                _SystemLanguage = config?.AppSettings?.Settings[nameof(SystemLanguage)]?.Value;
                _DefaultFont = config?.AppSettings?.Settings[nameof(DefaultFont)]?.Value;
            }
            catch (ConfigurationErrorsException e)
            {
                var msg = new StringBuilder();
                msg.AppendLine("ファイルの読み込みに失敗しました。");
                msg.AppendLine(_ExeFileMap.ExeConfigFilename);
                msg.AppendLine("ファイルが正常に読み込める状態になっているか確認してください。");
                msg.AppendLine("エラー内容：");
                msg.AppendLine(e.Message);
                //                MessageBoxEx.DirectShow(msg.ToString());
                Environment.Exit(-1);
            }
        }
    }
}