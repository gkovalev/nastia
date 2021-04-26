//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;

namespace AdvantShop.Helpers.CsvHelper
{
    public class Encodings
    {
        public enum EncodingsEnum
        {
            Utf8,
            Utf16,
            Windows1251,
            Koi8R
        }

        public static Encoding GetEncoding(EncodingsEnum item)
        {
            switch (item)
            {
                case EncodingsEnum.Utf8:
                    return Encoding.GetEncoding("UTF-8");
                case EncodingsEnum.Utf16:
                    return Encoding.GetEncoding("UTF-16");
                case EncodingsEnum.Windows1251:
                    return Encoding.GetEncoding("Windows-1251");
                case EncodingsEnum.Koi8R:
                    return Encoding.GetEncoding("KOI8-R");
                default:
                    throw new Exception("Unknow encoding");
            }
        }

        public static Encoding GetEncoding()
        {
            switch (CsvEnconing)
            {
                case EncodingsEnum.Utf8:
                    return Encoding.GetEncoding("UTF-8");
                case EncodingsEnum.Utf16:
                    return Encoding.GetEncoding("UTF-16");
                case EncodingsEnum.Windows1251:
                    return Encoding.GetEncoding("Windows-1251");
                case EncodingsEnum.Koi8R:
                    return Encoding.GetEncoding("KOI8-R");
                default:
                    throw new Exception("Unknow encoding");
            }
        }

        public static EncodingsEnum CsvEnconing
        {
            get
            {
                try
                {
                    var res = EncodingsEnum.Windows1251;
                    if (Enum.IsDefined(typeof(EncodingsEnum), SettingsGeneral.CsvEnconing))
                        res = (EncodingsEnum)Enum.Parse(typeof(EncodingsEnum), SettingsGeneral.CsvEnconing, true);
                    return res;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex, "Problem with CsvEnconing");
                    return EncodingsEnum.Windows1251;
                }
            }
            set
            {
                SettingsGeneral.CsvEnconing = value.ToString();
            }
        }
    }
}