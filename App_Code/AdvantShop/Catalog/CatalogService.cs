//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text;
using AdvantShop.Repository.Currencies;
using Resources;
using AdvantShop.Customers;

namespace AdvantShop.Catalog
{
    public enum SortOrder
    {
        NoSorting,
        AscByName,
        DescByName,
        AscByPrice,
        DescByPrice,
        AscByRatio,
        DescByRatio,
    }

    public class CatalogService
    {
        #region  GetStringPrice

        public static string GetStringPrice(decimal price)
        {
            return GetStringPrice(price, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Symbol, 0, 1, CurrencyService.CurrentCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, null, false);
        }

        //Added by Evgeni
        public static string GetStringPrice(decimal price, string zeroPriceMessage)
        {
            return GetStringPrice(price, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Symbol, 0, 1, CurrencyService.CurrentCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, zeroPriceMessage, false);
        }

        public static string GetStringPrice(decimal price, bool isWrap)
        {
            return GetStringPrice(price, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Symbol, 0, 1, CurrencyService.CurrentCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, null, isWrap);
        }

        public static string GetStringPrice(decimal price, decimal currentCurrencyRate, string currentCurrencyIso3)
        {
            return GetStringPrice(price, 1, currentCurrencyIso3, currentCurrencyRate);
        }

        public static string GetStringPrice(decimal price, Currency currency)
        {
            return GetStringPrice(price, currency.Value, currency.Symbol, 0, 1, currency.IsCodeBefore, currency.PriceFormat, null, false);
        }

        public static string GetStringPrice(decimal price, decimal discount)
        {
            return GetStringPrice(price, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Symbol, discount, 1, CurrencyService.CurrentCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, null, false);
        }

        public static string GetStringPrice(decimal price, decimal discount, int amount)
        {
            return GetStringPrice(price, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Symbol, discount, amount, CurrencyService.CurrentCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, null, false);
        }

        public static string GetStringPrice(decimal price, decimal discount, int amount, string zeroPriceMsg)
        {
            return GetStringPrice(price, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Symbol, discount, amount, CurrencyService.CurrentCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, zeroPriceMsg, false);
        }

        public static string GetStringPrice(decimal price, int qty, string currencyCode, decimal currencyRate)
        {
            Currency cur = CurrencyService.Currency(currencyCode);
            if (cur == null)
                return GetStringPrice(price, currencyRate, currencyCode, 0, qty, false, CurrencyService.DefaultPriceFormat, null, false);
            return GetStringPrice(price, currencyRate, cur.Symbol, 0, qty, cur.IsCodeBefore, cur.PriceFormat, null, false);
        }

        private static string GetStringPrice(decimal price, decimal currentCurrencyRate, string currentCurrencyCode, decimal discount, int amount, bool isCodeBefore, string priceFormat, string zeroPriceMsg, bool isWrap)
        {
            if ((price == 0 || amount == 0) && !string.IsNullOrEmpty(zeroPriceMsg))
            {
                return zeroPriceMsg;
            }

            string strPriceRes;
            if (discount == 0)
            {
                strPriceRes = string.IsNullOrEmpty(priceFormat) ? Math.Round((price * amount) / currentCurrencyRate, 2).ToString() : String.Format("{0:" + priceFormat + "}", Math.Round((price * amount) / currentCurrencyRate, 2));
            }
            else
            {
                decimal dblTemp = (price * amount) / currentCurrencyRate;
                strPriceRes = string.IsNullOrEmpty(priceFormat) ? Math.Round(dblTemp - ((dblTemp / 100) * discount), 2).ToString() : String.Format("{0:" + priceFormat + "}", Math.Round(dblTemp - ((dblTemp / 100) * discount), 2));
            }

            string strCurrencyFormat;

            if (isWrap)
            {
                strCurrencyFormat = isCodeBefore ? "<span class=\"curr\">{1}</span> <span class=\"price-num\">{0}</span>" : "<span class=\"price-num\">{0}</span> <span class=\"curr\">{1}</span>";
            }
            else
            {
                strCurrencyFormat = isCodeBefore ? "{1} {0}" : "{0} {1}";
            }

            return string.Format(strCurrencyFormat, strPriceRes, currentCurrencyCode);
        }


        public static string GetStringDiscountPercent(decimal price, decimal discount, bool boolAddMinus)
        {
            return GetStringDiscountPercent(price, discount, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Symbol, CurrencyService.CurrentCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, boolAddMinus);
        }

        public static string GetStringDiscountPercent(decimal price, decimal discount, decimal currentCurrencyRate, string currentCurrencyCode, bool boolAddMinus)
        {
            return GetStringDiscountPercent(price, discount, currentCurrencyRate, currentCurrencyCode, CurrencyService.CurrentCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, boolAddMinus);
        }

        public static string GetStringDiscountPercent(decimal price, decimal discount, decimal currentCurrencyRate, string currentCurrencyCode, bool isCodeBefore, string priceFormat, bool boolAddMinus)
        {
            var strFormat = string.Empty;
            var dblRes = Math.Round(((price / 100) * discount) / currentCurrencyRate, 2);

            string strFormatedPrice = priceFormat == "" ? dblRes.ToString() : String.Format("{0:" + priceFormat + "}", dblRes);

            if (boolAddMinus)
            {
                strFormat = "-";
            }

            if (isCodeBefore)
            {
                strFormat += "{1}{0} ({2}%)";
            }
            else
            {
                strFormat += "{0}{1} ({2}%)";
            }

            return string.Format(strFormat, strFormatedPrice, currentCurrencyCode, discount);
        }

        #endregion

        /// <summary>
        /// Return offerListID. Temporary return constant 6
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int DefaultOfferListId
        {
            get { return 6; }
        }

        public static int CurrentOfferListId
        {
            get
            {
                //TODO add customer groups logic here
                return DefaultOfferListId;
            }
        }


        public static decimal CalculatePrice(decimal price, decimal discount)
        {
            return (price - ((price / 100) * discount));
        }

        public static string FormatPriceInvariant(object price)
        {
            return String.Format("{0:##,##0.##}", price);
        }


        public static string RenderLabels(bool recomend, bool sales, bool best, bool news, decimal discount, int labelCount = 5)
        {
            var labels = new StringBuilder();
            labels.Append("<span class=\"label-p\">");

            if (discount > 0 && labelCount-- > 0)
                labels.AppendFormat("<span class=\"disc\">{0} {1}%</span>", Resource.Client_Catalog_Discount, FormatPriceInvariant(discount));
            if (recomend && labelCount-- > 0)
                labels.AppendFormat("<span class=\"recommend\">{0}</span>", Resource.Client_Catalog_LabelRecomend);
            if (sales && labelCount-- > 0)
                labels.AppendFormat("<span class=\"sales\">{0}</span>", Resource.Client_Catalog_LabelSales);
            if (best && labelCount-- > 0)
                labels.AppendFormat("<span class=\"best\">{0}</span>", Resource.Client_Catalog_LabelBest);
            if (news && labelCount > 0)
                labels.AppendFormat("<span class=\"new\">{0}</span>", Resource.Client_Catalog_LabelNew);

            labels.Append("</span>");

            return labels.ToString();
        }

        public static string RenderPrice(decimal productPrice, decimal discount, bool showDiscount, CustomerGroup customerGroup, string customOptions = null)
        {
            if (productPrice == 0)
            {
               
                return string.Format("<div class=\'price\'>{0}</div>", Resource.Client_Catalog_ContactWithUs);
               
            }
            string res;


            decimal price = ProductService.CalculateProductPrice(productPrice, discount, customerGroup, CustomOptionsService.DeserializeFromXml(customOptions), false);
            decimal priceWithDiscount = ProductService.CalculateProductPrice(productPrice, discount, customerGroup, CustomOptionsService.DeserializeFromXml(customOptions), true);

            // Added By Evgeni for new price
            string priceForConvertString = CatalogService.GetStringPrice(priceWithDiscount).Replace(CurrencyService.CurrentCurrency.Symbol, "").Replace(" ", "");
            var newPrice = GenerateNewBelarusPrice(priceForConvertString);

            //

            //Changed by Evgeni to calculate final discount
            decimal groupDiscount = customerGroup.CustomerGroupId == 0 ? 0 : customerGroup.GroupDiscount;
            discount = Math.Max(discount, groupDiscount);
            //


            if (price == priceWithDiscount || !showDiscount)
            {
                res = string.Format("<div class=\'price\'>{0}</div>", (CatalogService.GetStringPrice(priceWithDiscount)));
            }
            else
            {
                res = string.Format("<div class=\"price-old\">{0}</div><div class=\"price\">{1}</div><div class=\"price-benefit\">{2} {3} {4} {5}% </div>",
                                   (CatalogService.GetStringPrice(productPrice)),
                                   (CatalogService.GetStringPrice(priceWithDiscount)),
                                    Resource.Client_Catalog_Discount_Benefit,
                                   (CatalogService.GetStringPrice(price - priceWithDiscount)),
                                    Resource.Client_Catalog_Discount_Or,
                                    CatalogService.FormatPriceInvariant(discount));
            }
            //Added by Evgeni
            res = res + string.Format("<div class=\"price-newformat\">{0}</div>", GenerateNewBelarusPrice(CatalogService.GetStringPrice(priceWithDiscount))) ;
            return res;
        }

        public static string GenerateNewBelarusPrice(string priceForConvertString)
        {
            //TO DO REMOVE
           // return priceForConvertString;
            //

            if (priceForConvertString.Contains("BYR") || priceForConvertString.Contains("руб."))
            {
                priceForConvertString = priceForConvertString.Replace("BYR", "").Replace("руб.", "").Trim();
            }

            decimal priceforConvert = 0;
            decimal.TryParse(priceForConvertString, out priceforConvert);

            var newPrice = (priceforConvert * 10000).ToString();
            //if (newPrice.Contains(","))
            //{
            //    var newKop = newPrice.Remove(0, newPrice.IndexOf(","));
            //    if (newKop.Length > 3)
            //    {
            //        newKop = newKop.Remove(3);
            //    }
            //    if (newKop.Length == 2)
            //        newKop += "0";
            //    newPrice = newPrice.Remove(newPrice.IndexOf(",")) + newKop;

            //}

            if (newPrice.Contains(","))
            {
                newPrice = newPrice.Remove(newPrice.IndexOf(","));
            }

            newPrice = newPrice + " BYR";
            return newPrice;
        }

        public static string RenderSelectedOptions(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return string.Empty;
            }
            var result = new StringBuilder("<div class=\"customoptions\">");

            foreach (var item in CustomOptionsService.DeserializeFromXml(xml))
            {
                result.Append(item.CustomOptionTitle);
                result.Append(": ");
                result.Append(item.OptionTitle);
                if (item.OptionPriceBc != 0)
                {
                    result.Append(" ");
                    if (item.OptionPriceBc > 0)
                    {
                        result.Append("+");
                    }
                    result.Append(CatalogService.GetStringPrice(item.OptionPriceBc));
                }
                result.Append("<br />");
            }

            result.Append("</div>");

            return result.ToString();
        }

    }
}