using System;
using System.Text;

namespace SiteWatchman.Persistance
{
    public static class StringExtensions
    {
        private const int LowerCaseOffset = 'a' - 'A';
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var len = value.Length;
            var newValue = new char[len];
            var firstPart = true;

            for (var i = 0; i < len; ++i)
            {
                var c0 = value[i];
                var c1 = i < len - 1 ? value[i + 1] : 'A';
                var c0isUpper = c0 >= 'A' && c0 <= 'Z';
                var c1isUpper = c1 >= 'A' && c1 <= 'Z';

                if (firstPart && c0isUpper && (c1isUpper || i == 0))
                    c0 = (char)(c0 + LowerCaseOffset);
                else
                    firstPart = false;

                newValue[i] = c0;
            }

            return new string(newValue);
        }
        public static string SafeSubstring(this string value, int startIndex)
        {
            return SafeSubstring(value, startIndex, value.Length);
        }

        public static string SafeSubstring(this string value, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (value.Length >= (startIndex + length))
                return value.Substring(startIndex, length);

            return value.Length > startIndex ? value.Substring(startIndex) : string.Empty;
        }
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (value.IndexOf('_') >= 0)
            {
                var parts = value.Split('_');
                var sb = StringBuilderThreadStatic.Allocate();
                foreach (var part in parts)
                {
                    var str = part.ToCamelCase();
                    sb.Append(char.ToUpper(str[0]) + str.SafeSubstring(1, str.Length));
                }

                return StringBuilderThreadStatic.ReturnAndFree(sb);
            }

            var camelCase = value.ToCamelCase();
            return char.ToUpper(camelCase[0]) + camelCase.SafeSubstring(1, camelCase.Length);
        }
    }

    //Use separate cache internally to avoid reallocations and cache misses
    internal static class StringBuilderThreadStatic
    {
        [ThreadStatic] private static StringBuilder _cache;

        public static StringBuilder Allocate()
        {
            var ret = _cache;
            if (ret == null)
                return new StringBuilder();

            ret.Length = 0;
            _cache = null;  //don't re-issue cached instance until it's freed
            return ret;
        }

        public static void Free(StringBuilder sb)
        {
            _cache = sb;
        }

        public static string ReturnAndFree(StringBuilder sb)
        {
            var ret = sb.ToString();
            _cache = sb;
            return ret;
        }
    }
}