//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Repository
{
    public class MeasureUnits
    {
        public enum WeightUnit
        {
            Kilogramm = 0,
            Pound
        }

        public static Dictionary<WeightUnit, decimal> WeightRates = new Dictionary<WeightUnit, decimal>
                                                                   {
                                                                       {WeightUnit.Kilogramm, 1},
                                                                       {WeightUnit.Pound, 0.45359237M}
                                                                   };

        public static decimal ConvertWeight(decimal value, WeightUnit from, WeightUnit to)
        {
            return value * WeightRates[from] / WeightRates[to];
        }
    }
}