using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPFCommon
{
    /// <summary>
    /// コメント
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> Errors = new();

        private readonly IServiceProvider Service = null;

        private readonly ValidationContext ValidationContext = null;

        private readonly List<ValidationResult> ValidationResultList = new();

        public BaseViewModel(IConfigService config, IShowMessageService message)
        {
            this.Config = config;
            this.MessageBox = message;
            this.Service = ApplicationEx.Service.GetProvider();
            this.ValidationContext = new ValidationContext(this, this.Service, null);
        }

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        private BaseViewModel()
        {
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// プロパティ値の変更時に実行されるハンドラ
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public event PropertyChangedEventHandler PropertyChanged;

        public IConfigService Config { get; }

        // INotifyDataErrorInfoの実装
        public bool HasErrors
        {
            get { return this.Errors.Values.Any(x => x != null); }
        }

        public IShowMessageService MessageBox { get; }

        public void AddError(string propertyName, string error)
        {
            if (this.Errors.ContainsKey(propertyName) == false)
            {
                this.Errors[propertyName] = new List<string>();
            }

            if (this.Errors[propertyName].Contains(error) == false)
            {
                this.Errors[propertyName].Add(error);
                this.OnErrorsChanged(propertyName);
            }
        }

        // INotifyDataErrorInfoの実装
        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName.IsEmpty())
            {
                return null;
            }
            if (this.Errors.ContainsKey(propertyName) == false)
            {
                return null;
            }
            return this.Errors[propertyName];
        }

        public bool HasError(string propertyName)
        {
            if (this.Errors.IsEmpty() || this.Errors.Count == 0)
            {
                return false;
            }
            if (this.Errors[propertyName]?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// プロパティ値の変更時にフレームワークに対して変更通知を行うハンドラ
        /// </summary>
        /// <param name="propName">変更されたプロパティ名</param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void RemoveError(string propertyName)
        {
            if (this.Errors.ContainsKey(propertyName))
            {
                this.Errors.Remove(propertyName);
            }

            this.OnErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName, string error)
        {
            if (this.Errors.ContainsKey(propertyName))
            {
                var errors = this.Errors[propertyName];
                if (errors.Contains(error))
                {
                    errors.Remove(error);
                }
                if (errors.Count == 0)
                {
                    this.Errors.Remove(propertyName);
                }
            }

            this.OnErrorsChanged(propertyName);
        }

        /// <summary>
        /// プロパティ値を設定します。
        /// </summary>
        /// <typeparam name="T">プロパティ値の型</typeparam>
        /// <param name="storage">プロパティ値の現在値</param>
        /// <param name="value">プロパティ値の変更後の値</param>
        /// <param name="propName">プロパティ名</param>
        /// <returns>プロパティ値が変更された場合：True
        /// プロパティ値が変更されなかった場合：False</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propName = "")
        {
            // 入力値に変化が無い場合は処理しない
            if (Equals(storage, value))
            {
                return false;
            }

            // コントロールに設定されているエラーをクリアしてからチェックを行う
            this.RemoveError(propName);

            // Validator内で各種サービスを使用するためにプロバイダーを取得
            // 動作速度を優先させるためにインスタンスメンバに変更
            //var service = ApplicationEx.Service.GetProvider();
            //var context = new ValidationContext(this, service, null) { MemberName = propName };
            this.ValidationContext.MemberName = propName;

            // 毎回インスタンスを生成するのでは無く最初に作成したものをクリアして使い回す
            this.ValidationResultList.Clear();
            //var validationErrors = ValidationResultList;

            // Validationを実行(各プロパティに定義されているValidationAttributeで指定されたチェックロジックが実行される)
            if (Validator.TryValidateProperty(value, this.ValidationContext, this.ValidationResultList) == false)
            {
                // エラーがあった場合
                var errors = this.ValidationResultList.Select(error => error.ErrorMessage);
                foreach (var error in errors)
                {
                    this.AddError(propName, error);
                }
            }
            else
            {
                // エラーが無かった場合
                this.RemoveError(propName);
            }

            // TODO ログ出力

            storage = value;
            this.NotifyPropertyChanged(propName);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnErrorsChanged(string propertyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}