using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFCommon
{
    /// <summary>
    /// 拡張メソッドを定義する静的クラス
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 対象のオブジェクトが空かどうかを判定します。
        /// 空文字列は空と判定します。
        /// 配列、Listは要素数が0の場合も空と判定します。
        /// </summary>
        /// <param name="target">判定対象のオブジェクト</param>
        /// <returns>空の場合はtrue、それ以外はfalse</returns>
        /// <example>
        /// <code>
        /// var sample = string.Empty;
        /// if (sample.IsEmpty())
        /// {
        ///     // sampleの値がカラと判定された場合のロジックを記述
        /// }
        /// </code>
        /// </example>
        public static bool IsEmpty(this object target)
        {
            if (target == null)
            {
                return true;
            }

            if (target.GetType() == typeof(string))
            {
                if ((target as string).Length == 0)
                {
                    return true;
                }

                return false;
            }

            if (target.GetType().IsArray)
            {
                var items = target as IEnumerable<object>;
                if (!items.Any())
                {
                    return true;
                }

                return false;
            }

            // 文字でも配列でもないオブジェクトに対して下記のチェックを
            // 行うと例外が発生するので、その場合はfalseを返却する。
            if (!(target is IEnumerable<object>))
            {
                return false;
            }

            try
            {
                if (typeof(List<>).IsAssignableFrom(target.GetType().GetGenericTypeDefinition()))
                {
                    var items = target as IEnumerable<object>;
                    if (!items.Any())
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}