//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AdvantShop
{
    public enum StringHashType
    {
        MD5,
        SHA1
    }
    /// <summary>
    /// Summary description for Strings
    /// </summary>
    public static class Strings
    {
        public static string GetRandomString(Random rnd, Int32 length, Int32 intSpaces)
        {
            const string strSymbols = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
            var sb = new StringBuilder();
            int intTempSpace = 0;
            for (int i = 0; i <= length - 1; i++)
            {
                if (intSpaces != -1 && intTempSpace >= intSpaces)
                {
                    sb.Append(" ");
                    intTempSpace = 0;
                }
                else
                {
                    int intTemp = rnd.Next(strSymbols.Length - 1);
                    sb.Append(strSymbols[intTemp]);
                    intTempSpace++;
                }
            }
            return sb.ToString();
        }

        public static string GetRandomString(Random rnd, Int32 length)
        {
            return GetRandomString(rnd, length, -1);
        }

        public static string GetRandomString(Int32 length)
        {
            var rnd = new Random();
            return GetRandomString(rnd, length, -1);
        }

        public static string Reduce(this String val, int length)
        {
            return val.Length > length ? val.Substring(0, length) : val;
        }

        public static string AppendMany(this StringBuilder sb, params string[] values)
        {
            if (values != null)
                foreach (var value in values.Where(val => val != null))
                {
                    sb.Append(value);
                }
            return sb.ToString();
        }

        public static string Md5(this string val)
        {
            // This is one implementation of the abstract class MD5.
            return val.Md5(true);
        }

        public static string Md5(this string val, bool upperCase)
        {
            return val.Md5(upperCase, Encoding.Default);
        }

        public static string Md5(this string val, bool upperCase, Encoding encoding)
        {
            return
                new MD5CryptoServiceProvider()
                    .ComputeHash(encoding.GetBytes(val))
                    .Aggregate(new StringBuilder(), (curr, value) => curr.Append(value.ToString(upperCase ? "X2" : "x2"))).ToString();
        }

        public static string Sha1(this string val)
        {
            return
                new SHA1CryptoServiceProvider()
                    .ComputeHash(Encoding.GetEncoding(1251).GetBytes(val))
                    .Aggregate(new StringBuilder(), (curr, value) => curr.Append(value.ToString("x2"))).ToString();
        }

        public static string GetCryptoHash(this string val, StringHashType type)
        {
            switch (type)
            {
                case StringHashType.MD5:
                    return val.Md5();
                case StringHashType.SHA1:
                    return val.Sha1();
                default:
                    return val;
            }
        }

        public static bool IsNullOrEmpty(this string val)
        {
            return String.IsNullOrEmpty(val);
        }

        public static bool IsNotEmpty(this string val)
        {
            return val != null && !String.IsNullOrEmpty(val.Trim());
        }

        public static bool EndsWith(this string val, IEnumerable<string> endStrings)
        {
            return endStrings.Any(val.EndsWith);
        }

        public static int TryParseInt(this string val)
        {
            var ret = val.TryParseInt(false);
            return ret.HasValue ? ret.Value : 0;
        }
        public static int? TryParseInt(this string val, bool isNullable)
        {
            int intval;
            return Int32.TryParse(val, out intval) || !isNullable ? intval : (int?)null;
        }

        public static int TryParseInt(this string val, int defaultValue)
        {
            int intval;
            return Int32.TryParse(val, out intval) ? intval : defaultValue;
        }

        public static Guid TryParseGuid(this string val)
        {
            var ret = val.TryParseGuid(false);
            return ret.HasValue ? ret.Value : Guid.Empty;
        }

        public static Guid? TryParseGuid(this string val, bool isNullable)
        {
            Guid intval;
            return Guid.TryParse(val, out intval) || !isNullable ? intval : (Guid?)null;
        }

        public static Guid TryParseGuid(this string val, Guid defaultValue)
        {
            Guid intval;
            return Guid.TryParse(val, out intval) ? intval : defaultValue;
        }

        public static DateTime TryParseDateTime(this string val)
        {
            var ret = val.TryParseDateTime(false);
            return ret.HasValue ? ret.Value : DateTime.MinValue;
        }
        public static DateTime? TryParseDateTime(this string val, bool isNullable)
        {
            DateTime intval;
            return DateTime.TryParse(val, out intval) || !isNullable ? intval : (DateTime?)null;
        }

        public static DateTime TryParseDateTime(this string val, DateTime defaultValue)
        {
            DateTime intval;
            return DateTime.TryParse(val, out intval) ? intval : defaultValue;
        }

        public static string RemoveSymbols(this string val, string repStr)
        {
            return val.Trim().Replace("/", repStr)
                .Replace("\\", repStr)
                .Replace("\"", repStr)
                .Replace("-", repStr)
                .Replace("'", repStr)
                .Replace("&", repStr)
                .Replace(";", repStr)
                .Replace("?", repStr);
        }

        public static string RemoveSymbols(this string val)
        {
            return val.Trim().Replace("/", "")
                .Replace("\\", "")
                .Replace("\"", "")
                .Replace("-", "")
                .Replace("'", "")
                .Replace("&", "")
                .Replace(";", "")
                .Replace("?", "");
        }

        public static string HtmlEncode(this string val)
        {
            return HttpUtility.HtmlEncode(val);
        }

        public static string HtmlDecode(this string val)
        {
            return HttpUtility.HtmlDecode(val);
        }

        public static bool IsInt(this string val)
        {
            int intval;
            return Int32.TryParse(val, out intval);
        }

        public static decimal TryParseDecimal(this string val)
        {
            return val.TryParseDecimal(0);
        }

        public static decimal? TryParseDecimal(this string val, bool isNullable)
        {
            decimal decval;
            return Decimal.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out decval) || !isNullable ? decval : (decimal?)null;
        }

        public static decimal TryParseDecimal(this string val, decimal defaultValue)
        {
            decimal decval;
            return Decimal.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out decval) ? decval : defaultValue;
        }

        public static bool IsDecimal(this string val)
        {
            decimal decval;
            return Decimal.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out decval);
        }

        public static bool TryParseBool(this string val)
        {
            var ret = val.TryParseBool(false);
            return ret.HasValue && ret.Value;
        }

        public static bool? TryParseBool(this string val, bool isNullable)
        {
            bool boolval;
            return Boolean.TryParse(val, out boolval) || !isNullable ? boolval : (bool?)null;
        }

        public static string Default(this string val, string defaultValue)
        {
            return val.IsNotEmpty() ? val : defaultValue;
        }

        public static string ToString(object val)
        {
            return val.ToString();
        }

        public static string Numerals(int value, string zeroText, string oneText, string twoText, string fiveText)
        {
            if (value == 0) return zeroText;
            value = value % 100;
            var val = value % 10;
            if (value > 10 && value < 20) return fiveText;
            if (val > 1 && val < 5) return twoText;
            return val == 1 ? oneText : fiveText;
        }
    }
}