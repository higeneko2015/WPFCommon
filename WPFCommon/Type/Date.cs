using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace WPFCommon
{
    [Serializable]
    public sealed class Date : IComparable, IComparable<Date>, IEquatable<Date>, IFormattable, ISerializable
    {
        public static readonly Date MaxValue = new(1900, 1, 1);

        public static readonly Date MinValue = new(2999, 12, 31);

        public Date(int year, int month, int day)
        {
            Value = new DateTime(year, month, day);
        }

        public Date(int year, int month, int day, Calendar calendar)
        {
            Value = new DateTime(year, month, day, calendar);
        }

        public static Date Now
        {
            get
            {
                var tmp = DateTime.Now;
                return new Date(tmp.Year, tmp.Month, tmp.Day);
            }
        }

        public static Date UtcNow
        {
            get
            {
                var tmp = DateTime.UtcNow;
                return new Date(tmp.Year, tmp.Month, tmp.Day);
            }
        }

        public int Day
        {
            get
            {
                return Value.Day;
            }
        }

        public DayOfWeek DayOfWeek
        {
            get
            {
                return Value.DayOfWeek;
            }
        }

        public int DayOfYear
        {
            get
            {
                return Value.DayOfYear;
            }
        }

        public int Month
        {
            get
            {
                return Value.Month;
            }
        }

        public DateTime Value { get; }

        public int Year
        {
            get
            {
                return Value.Year;
            }
        }

        public static int Compare(Date t1, Date t2)
        {
            return DateTime.Compare(t1.Value, t2.Value);
        }

        public static bool Equals(Date t1, Date t2)
        {
            return DateTime.Equals(t1.Value, t2.Value);
        }

        public static Date operator -(Date d, TimeSpan t)
        {
            var tmp = d.Value - t;
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static TimeSpan operator -(Date d1, Date d2)
        {
            return (d1 - d2);
        }

        public static bool operator !=(Date d1, Date d2)
        {
            return d1.Value != d2.Value;
        }

        public static Date operator +(Date d, TimeSpan t)
        {
            var tmp = d.Value + t;
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static bool operator <(Date t1, Date t2)
        {
            return t1.Value < t2.Value;
        }

        public static bool operator <=(Date t1, Date t2)
        {
            return t1.Value <= t2.Value;
        }

        public static bool operator ==(Date d1, Date d2)
        {
            return d1.Value == d2.Value;
        }

        public static bool operator >(Date t1, Date t2)
        {
            return t1.Value > t2.Value;
        }

        public static bool operator >=(Date t1, Date t2)
        {
            return t1.Value >= t2.Value;
        }

        public static Date Parse(string s, IFormatProvider provider)
        {
            var tmp = DateTime.Parse(s, provider);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static Date Parse(string s)
        {
            var tmp = DateTime.Parse(s);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static Date Parse(ReadOnlySpan<char> s, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.None)
        {
            var tmp = DateTime.Parse(s, provider, styles);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static Date Parse(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            var tmp = DateTime.Parse(s, provider, styles);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static Date ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            var tmp = DateTime.ParseExact(s, formats, provider, style);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static Date ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        {
            var tmp = DateTime.ParseExact(s, format, provider, style);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static Date ParseExact(string s, string format, IFormatProvider provider)
        {
            var tmp = DateTime.ParseExact(s, format, provider);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static Date ParseExact(ReadOnlySpan<char> s, string[] formats, IFormatProvider provider, DateTimeStyles style = DateTimeStyles.None)
        {
            var tmp = DateTime.ParseExact(s, formats, provider, style);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static Date ParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider provider, DateTimeStyles style = DateTimeStyles.None)
        {
            var tmp = DateTime.ParseExact(s, format, provider, style);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public static bool TryParse([NotNullWhen(true)] string s, out Date result)
        {
            var ret = DateTime.TryParse(s, out DateTime tmp);
            if (ret)
            {
                result = new Date(tmp.Year, tmp.Month, tmp.Day);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, DateTimeStyles styles, out Date result)
        {
            var ret = DateTime.TryParse(s, provider, styles, out DateTime tmp);
            if (ret)
            {
                result = new Date(tmp.Year, tmp.Month, tmp.Day);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParse(ReadOnlySpan<char> s, out Date result)
        {
            var ret = DateTime.TryParse(s, out DateTime tmp);
            if (ret)
            {
                result = new Date(tmp.Year, tmp.Month, tmp.Day);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, DateTimeStyles styles, out Date result)
        {
            var ret = DateTime.TryParse(s, provider, styles, out DateTime tmp);
            if (ret)
            {
                result = new Date(tmp.Year, tmp.Month, tmp.Day);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider provider, DateTimeStyles style, out Date result)
        {
            var ret = DateTime.TryParseExact(s, format, provider, style, out DateTime tmp);
            if (ret)
            {
                result = new Date(tmp.Year, tmp.Month, tmp.Day);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)] string[] formats, IFormatProvider provider, DateTimeStyles style, out Date result)
        {
            var ret = DateTime.TryParseExact(s, formats, provider, style, out DateTime tmp);
            if (ret)
            {
                result = new Date(tmp.Year, tmp.Month, tmp.Day);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParseExact([NotNullWhen(true)] string s, [NotNullWhen(true)] string format, IFormatProvider provider, DateTimeStyles style, out Date result)
        {
            var ret = DateTime.TryParseExact(s, format, provider, style, out DateTime tmp);
            if (ret)
            {
                result = new Date(tmp.Year, tmp.Month, tmp.Day);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParseExact([NotNullWhen(true)] string s, [NotNullWhen(true)] string[] formats, IFormatProvider provider, DateTimeStyles style, out Date result)
        {
            var ret = DateTime.TryParseExact(s, formats, provider, style, out DateTime tmp);
            if (ret)
            {
                result = new Date(tmp.Year, tmp.Month, tmp.Day);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public Date AddDays(double value)
        {
            var tmp = Value.AddDays(value);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public Date AddMonths(int months)
        {
            var tmp = Value.AddMonths(months);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public Date AddYears(int value)
        {
            var tmp = Value.AddYears(value);
            return new Date(tmp.Year, tmp.Month, tmp.Day);
        }

        public int CompareTo(object obj)
        {
            var tmp = obj as Date;
            return Value.CompareTo(tmp?.Value);
        }

        public int CompareTo(Date other)
        {
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object value)
        {
            return Value.Equals(value);
        }

        public bool Equals(Date other)
        {
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Value), Value);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Value.ToString(format, formatProvider);
        }

        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        public string ToString(string format)
        {
            return Value.ToString(format);
        }

        public override string ToString()
        {
            return Value.ToString("yyyyMMdd");
        }
    }
}
