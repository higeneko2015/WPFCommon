using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace WPFCommon
{
    /// <summary>
    /// 数値のみ入力可能なテキストボックスの基底クラス
    /// </summary>
    public class NumberBoxEx : TextBoxBase
    {
        /// <summary>
        /// クラスへの初回アクセス時に１度だけ実行される静的コンストラクタ
        /// </summary>
        static NumberBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBoxEx), new FrameworkPropertyMetadata(typeof(NumberBoxEx)));
        }

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        public NumberBoxEx()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // IMEを無効にする
            InputMethod.SetIsInputMethodEnabled(this, false);

            // 貼付時にカンマを除去してから入力可否を判定するための設定
            this.RemovePastingCharacters = new string[] { ",", "\r" };

            this.Loaded += this.NumberBoxEx_Loaded;

            //this.CheckInputCharacterHandler += this.CheckInputText;
            //this.CheckPasteCharacterHandler += this.CheckPasteText;
            //this.GotKeybordFocusInvokeHandler += this.GotKeyboardFocusInvoke;
            //this.Unloaded += this.NumberBoxEx_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            var expression = BindingOperations.GetBindingExpression(this, TextProperty);
            if (expression.IsEmpty())
            {
                return;
            }

            var binding = BindingOperations.GetBinding(this, TextProperty);
            if (binding.IsEmpty())
            {
                return;
            }

            var newBinding = new Binding(binding.Path.Path)
            {
                Mode = binding.Mode,
                NotifyOnValidationError = true,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                ConverterCulture = CultureInfo.CurrentCulture,
            };

            var x = this.DataContext?.GetType().GetProperty(expression.ParentBinding?.Path.Path);
            if (x == null)
            {
                return;
            }
            switch (x.PropertyType)
            {
                case Type intType when intType == typeof(int):
                    this.Format = expression.ParentBinding.StringFormat;
                    if (this.Format.IsEmpty())
                    {
                        this.Format = "#,0";
                    }
                    newBinding.Converter = new StringToIntegerConverter();
                    break;

                case Type intType when intType == typeof(int?):
                    this.Format = expression.ParentBinding.StringFormat;
                    if (this.Format.IsEmpty())
                    {
                        this.Format = "#,0";
                    }
                    newBinding.Converter = new StringToNullableIntegerConverter();
                    break;

                case Type intType when intType == typeof(decimal):
                    this.Format = expression.ParentBinding.StringFormat;
                    if (this.Format.IsEmpty())
                    {
                        this.Format = "#,0.00";
                    }
                    newBinding.Converter = new StringToDecimalConverter();
                    break;

                case Type intType when intType == typeof(decimal?):
                    this.Format = expression.ParentBinding.StringFormat;
                    if (this.Format.IsEmpty())
                    {
                        this.Format = "#,0.00";
                    }
                    newBinding.Converter = new StringToNullableDecimalConverter();
                    break;

                default:
                    break;
            }
            newBinding.ConverterParameter = this.Format;
            newBinding.StringFormat = this.Format;
            BindingOperations.SetBinding(this, TextProperty, newBinding);

            base.OnApplyTemplate();
        }

        /// <summary>
        /// キー押下時に入力可能な文字種かどうか判定を行うメソッド
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="inputText">入力文字</param>
        /// <returns>入力可能(True)、入力不可(False)</returns>
        private bool CheckInputText(object sender, string inputText)
        {
            var target = sender as NumberBoxEx;

            // テキストが全選択されている場合は今回の入力は１文字目として
            // 扱うためにフラグを立てる
            var allSelectedFlg = false;

            if (target.SelectedText == target.Text)
            {
                allSelectedFlg = true;
            }

            // 半角スペースとカンマが混ざっている状態Decimal.TryPerseで
            // エラーにならないため個別に判定する。
            if (inputText == " " || inputText == ",")
            {
                return false;
            }

            var generateText = target.Text;

            // 2文字目以降に"-","+"が入力されるとdecimal.TryParseでエラーにならないため
            // 個別に判定する
            if (allSelectedFlg == false && (inputText == "-" || inputText == "+"))
            {
                return false;
            }

            // すでに入力済みの文字列と今回押下されたキー文字を合成して
            // チェック対象の文字列を生成する。
            if (target.SelectedText.Length > 0)
            {
                generateText = generateText.Remove(target.SelectionStart, target.SelectionLength);
            }

            // 入力キーからENTERとTABを除去する
            var newInputText = inputText.Replace("\r", "");
            newInputText = newInputText.Replace("\t", "");
            generateText = generateText.Insert(target.CaretIndex, newInputText);

            if (generateText == "-")
            {
                return true;
            }

            // 未入力はＯＫとする
            if (generateText.IsEmpty())
            {
                return true;
            }

            // 表示書式に小数点が含まれていない場合は小数点の入力は不可
            if (target.Format.Contains(".") == false && inputText == ".")
            {
                return false;
            }

            // 小数以下の値が入力されている場合は、フォーマットで指定された小数点以下桁数を
            // 超えていないかチェックする
            if (generateText.AsSpan().IndexOf(".") > 0)
            {
                // 入力値から小数点以下の文字数を取得
                var x = generateText[generateText.AsSpan().IndexOf(".")..].Length;
                // StringFormatから小数点以下の文字数を取得
                var y = target.Format[target.Format.AsSpan().IndexOf(".")..].Length;

                if (x > y)
                {
                    return false;
                }
            }

            // 数字に変換できなかった場合はエラーとする。
            var expression = target.GetBindingExpression(TextProperty);
            var z = target.DataContext?.GetType().GetProperty(expression.ParentBinding?.Path.Path);
            var ret = false;
            switch (z.PropertyType)
            {
                case Type intType when intType == typeof(int):
                    ret = int.TryParse(generateText, out _);
                    break;

                case Type intType when intType == typeof(decimal):
                    ret = decimal.TryParse(generateText, out _);
                    break;
            }

            if (ret == false)
            {
                return false;
            }

            // 正の小数の入力チェックを行う
            if (generateText.Length >= 2)
            {
                if (generateText.AsSpan(0, 1) == "0" && generateText.AsSpan(1, 1) != ".")
                {
                    return false;
                }
            }

            // 負の小数の入力チェックを行う
            if (generateText.Length >= 3)
            {
                if (generateText.AsSpan(0, 1) == "-" && generateText.AsSpan(1, 1) == "0" && generateText.AsSpan(2, 1) != ".")
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckPasteText(object sender, string pasteText)
        {
            var target = sender as NumberBoxEx;

            var generateText = target.Text;

            // すでに入力済みの文字列と今回押下されたキー文字を合成して
            // チェック対象の文字列を生成する。
            if (target.SelectedText.Length > 0)
            {
                generateText = generateText.Remove(target.SelectionStart, target.SelectionLength);
            }

            // 入力済みの文字と貼り付けしようとしている文字を合成する
            generateText = generateText.Insert(target.CaretIndex, pasteText);

            // 数字と小数点、マイナス記号以外が入力されていたらNG
            if (Regex.IsMatch(generateText, @"^[0123456789.-]+$") == false)
            {
                return false;
            }

            // ２文字目以降にマイナス記号が見つかった場合はエラー
            if (generateText.AsSpan().IndexOf("-") > 0)
            {
                return false;
            }

            // 未入力はＯＫとする
            if (generateText.IsEmpty())
            {
                return true;
            }

            // 表示書式に小数点が含まれていない場合は小数点の入力は不可
            if (target.Format.Contains(".") == false && pasteText.Contains("."))
            {
                return false;
            }

            // 小数以下の値が入力されている場合は、フォーマットで指定された小数点以下桁数を
            // 超えていないかチェックする
            if (generateText.AsSpan().IndexOf(".") > 0)
            {
                // 入力値から小数点以下の文字数を取得
                var x = generateText.AsSpan()[generateText.AsSpan().IndexOf(".")..].Length;
                // StringFormatから小数点以下の文字数を取得
                var y = target.Format.AsSpan()[target.Format.AsSpan().IndexOf(".")..].Length;

                if (x > y)
                {
                    return false;
                }
            }

            // 数字に変換できなかった場合はエラーとする。
            var expression = target.GetBindingExpression(TextProperty);
            var z = target.DataContext?.GetType().GetProperty(expression.ParentBinding?.Path.Path);
            var ret = false;
            switch (z.PropertyType)
            {
                case Type intType when intType == typeof(int):
                    ret = int.TryParse(generateText, out _);
                    break;

                case Type intType when intType == typeof(decimal):
                    ret = decimal.TryParse(generateText, out _);
                    break;
            }

            if (ret == false)
            {
                return false;
            }

            // 正の小数の入力チェックを行う
            if (generateText.Length >= 2)
            {
                if (generateText.AsSpan(0, 1) == "0" && generateText.AsSpan(1, 1) != ".")
                {
                    return false;
                }
            }

            // 負の小数の入力チェックを行う
            if (generateText.Length >= 3)
            {
                if (generateText.AsSpan(0, 1) == "-" && generateText.AsSpan(1, 1) == "0" && generateText.AsSpan(2, 1) != ".")
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// キーボードフォーカス取得時の最後に実行されるメソッド
        /// カンマ編集を解除してテキストを全選択する。
        /// </summary>
        private void GotKeyboardFocusInvoke()
        {
            this.Text = this.Text.Replace(",", string.Empty);
            this.SelectAll();
        }

        private void NumberBoxEx_Closed(object sender, EventArgs e)
        {
            this.Loaded -= this.NumberBoxEx_Loaded;
            this.NumberBoxEx_Unloaded(sender, new RoutedEventArgs());
        }

        private void NumberBoxEx_Loaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded += this.NumberBoxEx_Unloaded;
            this.CheckInputCharacterHandler += this.CheckInputText;
            this.CheckPasteCharacterHandler += this.CheckPasteText;
            this.GotKeybordFocusInvokeHandler += this.GotKeyboardFocusInvoke;
            Window.GetWindow(this).Closed += this.NumberBoxEx_Closed;
        }

        private void NumberBoxEx_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= this.NumberBoxEx_Unloaded;
            this.CheckInputCharacterHandler -= this.CheckInputText;
            this.CheckPasteCharacterHandler -= this.CheckPasteText;
            this.GotKeybordFocusInvokeHandler -= this.GotKeyboardFocusInvoke;
            Window.GetWindow(this).Closed -= this.NumberBoxEx_Closed;
        }

        /// <summary>
        /// Unloadedイベント発生時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        //private void NumberBoxEx_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    this.CheckInputCharacterHandler -= CheckInputText;
        //    this.GotKeybordFocusInvokeHandler -= this.GotKeyboardFocusInvoke;
        //    this.Unloaded -= this.NumberBoxEx_Unloaded;
        //}
    }
}