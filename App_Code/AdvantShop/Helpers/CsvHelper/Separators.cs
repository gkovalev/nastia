//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;

namespace AdvantShop.Helpers.CsvHelper
{
    public class Separators
    {
        public enum SeparatorsEnum
        {
            CommaSeparated,
            TabSeparated,
            SemicolonSeparated
        }

        public static char GetCharSeparator(SeparatorsEnum item)
        {
            switch (item)
            {
                case SeparatorsEnum.CommaSeparated:
                    return ',';
                case SeparatorsEnum.TabSeparated:
                    return '\t';
                case SeparatorsEnum.SemicolonSeparated:
                    return ';';
                default:
                    throw new Exception("Unknow separator");
            }
        }

        public static char GetCharSeparator()
        {
            switch (CsvSeparator)
            {
                case SeparatorsEnum.CommaSeparated:
                    return ',';
                case SeparatorsEnum.TabSeparated:
                    return '\t';
                case SeparatorsEnum.SemicolonSeparated:
                    return ';';
                default:
                    throw new Exception("Unknow separator");
            }
        }

        public static SeparatorsEnum CsvSeparator
        {
            get
            {
                try
                {
                    var res = SeparatorsEnum.SemicolonSeparated;
                    if (Enum.IsDefined(typeof(SeparatorsEnum), SettingsGeneral.CsvSeparator))
                        res = (SeparatorsEnum)Enum.Parse(typeof(SeparatorsEnum), SettingsGeneral.CsvSeparator, true);
                    return res;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex, "Problem with CsvSeparator");
                    return SeparatorsEnum.SemicolonSeparated;
                }
            }
            set
            {
                SettingsGeneral.CsvSeparator = value.ToString();
            }
        }
    }
}