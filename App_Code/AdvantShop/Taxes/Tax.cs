//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;

namespace AdvantShop.Taxes
{
    #region TypeRate enum

    public enum RateType
    {
        LumpSum = 0, //фиксированный размер налога без зависимости суммы
        Proportional = 1, //фиксированнй процент от суммы
        //Progressive = 2, //процент, увеличивающийся при увеличении суммы
        Any
    }

    #endregion

    #region TypeRateDepends enum

    public enum TypeRateDepends
    {
        Default = 0,
        ByShippingAddress = 1,
        ByBillingAddress = 2        
    }

    #endregion

    public class RegionalRate
    {
        public int RegionId { get; set; }
        public decimal Rate { get; set; }
    }

    /// <summary>
    /// Сводное описание для Tax
    /// </summary>
    [Serializable]
    public class TaxElement
    {
        public int TaxId { get; set; }
        public string Name { get; set; }
        public bool  Enabled { get; set; }
        public int Priority { get; set; }
        public TypeRateDepends DependsOnAddress { get; set; }
        public RateType Type { get; set; }
        public decimal FederalRate { get; set; }
        public int CountryID { get; set; }
        private ICollection<RegionalRate> _regionalRates;
        public ICollection<RegionalRate> RegionalRates 
        { 
            get
            {
                if (_regionalRates == null)
                {
                    _regionalRates = TaxServices.GetRegionalRatesForTax(TaxId);
                }
                return _regionalRates;
            }
            set
            {
                _regionalRates = value;
            }
        }
        public bool ShowInPrice { get; set; }
        public string RegNumber { get; set; }
        public static bool operator == (TaxElement first, TaxElement second)
        {
            return first.TaxId == second.TaxId;
        }

        public static bool operator !=(TaxElement first, TaxElement second)
        {
            return !(first == second);
        }

        public bool Equals(TaxElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.TaxId == TaxId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (TaxElement) && Equals((TaxElement) obj);
        }

        public override int GetHashCode()
        {
            return TaxId;
        }
    }
}