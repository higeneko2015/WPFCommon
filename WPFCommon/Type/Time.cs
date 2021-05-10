using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace WPFCommon
{
    [Serializable]
    public sealed class Time : IComparable, IComparable<Time>, IEquatable<Time>, IFormattable, ISerializable
    {
        public static readonly Time MaxValue = new(0, 0, 0, 0);

        public static readonly Time MinValue = new(23, 59, 59, 999);

        public Time(int hour)
            : this(hour, 0, 0, 0)
        {
        }

        public Time(int hour, int minute)
            : this(hour, minute, 0, 0)
        {
        }

        public Time(int hour, int minute, int second)
            : this(hour, minute, second, 0)
        {
        }

        public Time(int hour, int minute, int second, int millisecond)
        {
            var now = DateTime.Now;
            this.Value = new DateTime(now.Year, now.Month, now.Day, hour, minute, second, millisecond);
        }

        public static Time Now
        {
            get
            {
                var tmp = DateTime.Now;
                return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
        }

        public static Time UtcNow
        {
            get
            {
                var tmp = DateTime.UtcNow;
                return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
        }

        public int Hour
        {
            get { return this.Value.Hour; }
        }

        public int Millisecond
        {
            get { return this.Value.Millisecond; }
        }

        public int Minute
        {
            get { return this.Value.Minute; }
        }

        public int Second
        {
            get { return this.Value.Second; }
        }

        public TimeSpan TimeOfDay
        {
            get { return this.Value.TimeOfDay; }
        }

        public DateTime Value { get; private set; }

        public static int Compare(Time t1, Time t2)
        {
            return DateTime.Compare(t1.Value, t2.Value);
        }

        public static bool Equals(Time t1, Time t2)
        {
            return DateTime.Equals(t1.Value, t2.Value);
        }

        public static Time operator -(Time d, TimeSpan t)
        {
            var tmp = d.Value - t;
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static TimeSpan operator -(Time d1, Time d2)
        {
            return (d1 - d2);
        }

        public static bool operator !=(Time d1, Time d2)
        {
            return d1.Value != d2.Value;
        }

        public static Time operator +(Time d, TimeSpan t)
        {
            var tmp = d.Value + t;
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static bool operator <(Time t1, Time t2)
        {
            return t1.Value < t2.Value;
        }

        public static bool operator <=(Time t1, Time t2)
        {
            return t1.Value <= t2.Value;
        }

        public static bool operator ==(Time d1, Time d2)
        {
            return d1.Value == d2.Value;
        }

        public static bool operator >(Time t1, Time t2)
        {
            return t1.Value > t2.Value;
        }

        public static bool operator >=(Time t1, Time t2)
        {
            return t1.Value >= t2.Value;
        }

        public static Time Parse(string s, IFormatProvider provider)
        {
            var tmp = DateTime.Parse(s, provider);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static Time Parse(string s)
        {
            var tmp = DateTime.Parse(s);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static Time Parse(ReadOnlySpan<char> s, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.None)
        {
            var tmp = DateTime.Parse(s, provider, styles);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static Time Parse(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            var tmp = DateTime.Parse(s, provider, styles);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static Time ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            var tmp = DateTime.ParseExact(s, formats, provider, style);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static Time ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        {
            var tmp = DateTime.ParseExact(s, format, provider, style);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static Time ParseExact(string s, string format, IFormatProvider provider)
        {
            var tmp = DateTime.ParseExact(s, format, provider);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static Time ParseExact(ReadOnlySpan<char> s, string[] formats, IFormatProvider provider, DateTimeStyles style = DateTimeStyles.None)
        {
            var tmp = DateTime.ParseExact(s, formats, provider, style);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static Time ParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider provider, DateTimeStyles style = DateTimeStyles.None)
        {
            var tmp = DateTime.ParseExact(s, format, provider, style);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public static bool TryParse([NotNullWhen(true)] string s, out Time result)
        {
            var ret = DateTime.TryParse(s, out DateTime tmp);
            if (ret)
            {
                result = new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, DateTimeStyles styles, out Time result)
        {
            var ret = DateTime.TryParse(s, provider, styles, out DateTime tmp);
            if (ret)
            {
                result = new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParse(ReadOnlySpan<char> s, out Time result)
        {
            var ret = DateTime.TryParse(s, out DateTime tmp);
            if (ret)
            {
                result = new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, DateTimeStyles styles, out Time result)
        {
            var ret = DateTime.TryParse(s, provider, styles, out DateTime tmp);
            if (ret)
            {
                result = new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider provider, DateTimeStyles style, out Time result)
        {
            var ret = DateTime.TryParseExact(s, format, provider, style, out DateTime tmp);
            if (ret)
            {
                result = new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)] string[] formats, IFormatProvider provider, DateTimeStyles style, out Time result)
        {
            var ret = DateTime.TryParseExact(s, formats, provider, style, out DateTime tmp);
            if (ret)
            {
                result = new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParseExact([NotNullWhen(true)] string s, [NotNullWhen(true)] string format, IFormatProvider provider, DateTimeStyles style, out Time result)
        {
            var ret = DateTime.TryParseExact(s, format, provider, style, out DateTime tmp);
            if (ret)
            {
                result = new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public static bool TryParseExact([NotNullWhen(true)] string s, [NotNullWhen(true)] string[] formats, IFormatProvider provider, DateTimeStyles style, out Time result)
        {
            var ret = DateTime.TryParseExact(s, formats, provider, style, out DateTime tmp);
            if (ret)
            {
                result = new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
            }
            else
            {
                result = null;
            }
            return ret;
        }

        public Time AddHours(double value)
        {
            var tmp = this.Value.AddHours(value);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public Time AddMinutes(int value)
        {
            var tmp = this.Value.AddMinutes(value);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public Time AddSeconds(int value)
        {
            var tmp = this.Value.AddSeconds(value);
            return new Time(tmp.Hour, tmp.Minute, tmp.Second, tmp.Millisecond);
        }

        public int CompareTo(object obj)
        {
            var tmp = obj as Time;
            return this.Value.CompareTo(tmp?.Value);
        }

        public int CompareTo(Time other)
        {
            return this.Value.CompareTo(other.Value);
        }

        public bool Equals(Time other)
        {
            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object value)
        {
            return this.Value.Equals(value);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.Value), this.Value);
        }

        public override string ToString()
        {
            return this.Value.ToString("HHmmss");
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.Value.ToString(format, formatProvider);
        }

        public string ToString(IFormatProvider provider)
        {
            return this.Value.ToString(provider);
        }

        public string ToString(string format)
        {
            return this.Value.ToString(format);
        }
    }
}
