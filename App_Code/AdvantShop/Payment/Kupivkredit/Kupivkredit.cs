using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Orders;
using System.Text;
using System.Security.Cryptography;
using AdvantShop.Diagnostics;
using System.Xml.Linq;
using Newtonsoft.Json;
using AdvantShop.Configuration;
using AdvantShop.Taxes;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Repository;

namespace AdvantShop.Payment
{
    public class Kupivkredit: PaymentMethod
    {
        /*
         * Примечание:
         * Заказы на сумму менее 3000 руб. не обарабатываются
         * 
         * Тестовые данные:
         * Id партнера: 1-178YO4Z
         * API key: 123qwe
         * API secret ($salt или "соль" подписи сообщения): 321ewq
         * СМС код подтверждения - 1010
         * 
         */

        public string PartnerId { get; set; }
        public string SecretKey { get; set; }
        public bool Sandbox { get; set; }

        public decimal OrderSum { get; set; }

        public override PaymentType Type
        {
            get { return PaymentType.Kupivkredit; }
        }

        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {KupivkreditTemplate.PartnerId, PartnerId},
                               {KupivkreditTemplate.SecretKey, SecretKey},
                               {KupivkreditTemplate.Sandbox, Sandbox.ToString()}                             
                           };
            }
            set
            {
                PartnerId = value.ElementOrDefault(KupivkreditTemplate.PartnerId);
                SecretKey = value.ElementOrDefault(KupivkreditTemplate.SecretKey);
                Sandbox = value.ElementOrDefault(KupivkreditTemplate.Sandbox).TryParseBool();
            }
        }

        public override string ProcessJavascript(Order order)
        {
            byte[] orderJson = Encoding.UTF8.GetBytes(GetOrderJson(order, PartnerId));
            string base64 = Convert.ToBase64String(orderJson);

            var sb = new StringBuilder();

            sb.Append("<script type=\"text/javascript\"> ");
            sb.AppendFormat("$.getScript(\"https://{0}/widget/vkredit.js\", function() {{ \r\n",
                                        Sandbox ? "kupivkredit-test-fe.tcsbank.ru:8100" : "www.kupivkredit.ru");

            sb.AppendFormat("vkredit = new VkreditWidget({0}, {1}, ", 1, OrderSum.ToString("F2").Replace(",", "."));
            sb.AppendFormat("{{ order:\"{0}\", sig: \"{1}\" }}); ", base64, GetSign(base64, SecretKey));


            sb.Append("}); ");
            sb.Append("</script>");

            return sb.ToString();
        }

        public override string ProcessJavascriptButton(Order order)
        {
            return "vkredit.openWidget();";
        }

        public static string GetSign(string message, string secretKey)
        {
            int iterationCount = 1102;
            message += secretKey;
            string result = SymmetricEncryptionUtility.getMd5Hash(message, Encoding.UTF8) + SymmetricEncryptionUtility.getSHA1Hash(message, Encoding.UTF8);
            byte[] data = Encoding.UTF8.GetBytes(result);
            for (int i = 0; i < iterationCount; i++)
            {
                result = SymmetricEncryptionUtility.getMd5Hash(result, Encoding.UTF8);
            }
            return result;
        }

        /// <summary>
        /// Генерирует json с заказами
        /// </summary>
        private string GetOrderJson(Order order, string partnerId)
        {
            var orderItems = order.OrderItems;

            decimal subtotal = order.OrderItems.Sum(item => item.Amount * item.Price);

            // сумма налогов не включенных в стоимость товара
            var taxTotal = TaxServices.GetOrderTaxes(order.OrderID).Where(t => !t.TaxShowInPrice).Sum(t => t.TaxSum);
            var totalDiscount = Math.Round(subtotal / 100 * order.OrderDiscount, 2);

            foreach (var item in orderItems)
            {
                var percent = item.Price * item.Amount * 100 / subtotal;

                item.Price += percent * taxTotal / (100 * item.Amount);
                item.Price -= percent * totalDiscount / (100 * item.Amount);
            }

            var shopCartItems = orderItems.Select(item => new
                                                        {
                                                            title = item.Name,
                                                            category = item.ItemType == EnumItemType.Product ?
                                                                            ProductService.GetCategoriesByProductId(item.EntityId).FirstOrDefault().Name
                                                                            : "",
                                                            qty = item.Amount,
                                                            price = Math.Round(item.Price)
                                                        }).ToList();


            var shippingCost = order.ShippingCost / order.OrderCurrency.CurrencyValue;

            if (shippingCost > 0)
            {
                shopCartItems.Add(new
                                    {
                                        title = "Доставка",
                                        category = "",
                                        qty = 1,
                                        price = Math.Round(shippingCost)
                                    });
            }
            
            var details = new
            {
                firstname = order.OrderCustomer.FirstName,
                lastname = order.OrderCustomer.LastName,
                middlename = "",
                email = order.OrderCustomer.Email != "admin" ? order.OrderCustomer.Email : "",
                cellphone = order.OrderCustomer.MobilePhone
            };

            var array = new
            {
                items = shopCartItems,
                details,
                partnerId = partnerId,
                partnerName = SettingsMain.ShopName.Replace("\"", "").Replace("'", ""),
                partnerOrderId = Sandbox ? (order.Number.ToString() + "-kvk-test") : order.OrderID.ToString(),
                deliveryType = ""
            };

            OrderSum = shopCartItems.Sum(item => item.price * item.qty);

            return JsonConvert.SerializeObject(array);
        }
    }

    #region Help class SymmetricEncryptionUtility

    public static class SymmetricEncryptionUtility
    {
        public static string getMd5Hash(string input)
        {
            return getMd5Hash(input, Encoding.Default);
        }

        public static byte[] getMd5Hash(byte[] input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(input);
            return data;
        }

        public static string getMd5Hash(string input, Encoding enc)
        {
            byte[] dataIn = enc.GetBytes(input);
            byte[] data = getMd5Hash(dataIn);
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string getSHA1Hash(string input)
        {
            return getSHA1Hash(input, Encoding.Default);
        }

        public static string getSHA1Hash(string input, Encoding enc)
        {
            SHA1 sha1hasher = SHA1.Create();
            byte[] dataIn = enc.GetBytes(input);
            byte[] data = sha1hasher.ComputeHash(dataIn);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }

    #endregion
}