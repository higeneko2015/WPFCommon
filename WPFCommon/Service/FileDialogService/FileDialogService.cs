using Microsoft.Win32;

namespace WPFCommon
{
    /// <summary>
    /// ファイル選択/保存ダイアログサービスクラス
    /// </summary>
    public class FileDialogService : IFileDialogService
    {
        /// <inheritdoc/>
        public string Load(string filter, string defaultPath = null)
        {
            var dlg = new OpenFileDialog
            {
                Filter = filter,
                FilterIndex = 0,
                InitialDirectory = defaultPath
            };
            //TODO
            //var result = dlg.ShowDialog(AppEx.CurrentWindow) ?? false;
            //if (!result)
            //{
            //    return string.Empty;
            //}

            return dlg.FileName;
        }

        /// <inheritdoc/>
        public string Save(string filter, string defaultPath = null, string defaultFileName = null)
        {
            var dlg = new SaveFileDialog
            {
                Filter = filter,
                FilterIndex = 0,
                InitialDirectory = defaultPath,
                FileName = defaultFileName
            };
            // TODO
            //var result = dlg.ShowDialog(AppEx.CurrentWindow) ?? false;
            //if (!result)
            //{
            //    return string.Empty;
            //}

            return dlg.FileName;
        }
    }
}