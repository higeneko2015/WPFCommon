using System;
using System.Collections.Generic;
using System.Reflection;

namespace WPFCommon
{
    /// <summary>
    /// Windowの表示を行うクラス
    /// </summary>
    public class WindowService //: IWindowService
    {
        private static readonly Dictionary<string, Type> _WindowList = new Dictionary<string, Type>();

        /// <inheritdoc/>
        public void Add(string windowName)
        {
            if (_WindowList.ContainsKey(windowName))
            {
                return;
            }

            var targetType = Assembly.GetEntryAssembly().GetType(windowName);
            _WindowList.Add(windowName, targetType);
        }

        /// <inheritdoc/>
        public void Add(string windowName, string assmblyName)
        {
            if (_WindowList.ContainsKey(windowName))
            {
                return;
            }

            var asm = Assembly.LoadFrom(assmblyName);
            var targetType = asm.GetType(windowName);
            _WindowList.Add(windowName, targetType);
        }

        /// <inheritdoc/>
        //public void Show(string windowName, object[] param = null)
        //{
        //    var window = this.CreateInstance(windowName, param);
        //    window.Show();
        //}

        /// <inheritdoc/>
        //public bool? ShowDialog(string windowName, object[] param = null)
        //{
        //    var window = this.CreateInstance(windowName, param);
        //    return window.ShowDialog();
        //}

        /// <summary>
        /// Windowのインスタンスを生成して返却します。
        /// </summary>
        /// <param name="windowName">インスタンスを生成するWindow名</param>
        /// <param name="param">Windowに渡す起動引数</param>
        /// <returns>作成されたWindowのインスタンス</returns>
        //private BaseWindow CreateInstance(string windowName, object[] param = null)
        //{
        //    if (_WindowList.ContainsKey(windowName) == false)
        //    {
        //        // TODO エラー処理が必要
        //        return null;
        //    }

        //    var windowType = _WindowList[windowName];
        //    var window = (BaseWindow)Activator.CreateInstance(windowType, param);
        //    return window;
        //}
    }
}