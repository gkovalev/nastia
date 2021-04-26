//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;

namespace AdvantShop.Shipping
{
    public class Edost : IShippingMethod
    {
        private const string Url = "http://www.edost.ru/edost_calc_kln.php";
        public string ShopId { get; set; }
        public string Password { get; set; }
        public bool Insurance { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public decimal TotalPrice { get; set; }
        public string CityTo { get; set; }
        public string Zip { get; set; }
        public decimal Rate { get; set; }

        public Edost(Dictionary<string, string> parameters)
        {
            ShopId = parameters.ElementOrDefault(EdostTemplate.ShopId);
            Password = parameters.ElementOrDefault(EdostTemplate.Password);
            Rate = parameters.ElementOrDefault(EdostTemplate.Rate).TryParseDecimal();
        }

        /// <summary>
        /// Don't use this for Edost
        /// </summary>
        /// <returns></returns>
        public decimal GetRate()
        {
            throw new Exception("GetRate method isnot avalible for Edost");
        }

        public List<ShippingOption> GetShippingOptions()
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                var request = WebRequest.Create(Url);
                request.Method = "POST";
                string postData = GetParam();
                byte[] byteArray = Encoding.GetEncoding("windows-1251").GetBytes(postData);

                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                using (Stream dataStream = request.GetRequestStream())
                {
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();
                }
                using (var response = request.GetResponse())
                {
                    // Get the stream containing all content returned by the requested server.
                    using (var dataStream = response.GetResponseStream())
                    {
                        if (dataStream == null) return new List<ShippingOption>();
                        // Open the stream using a StreamReader for easy access.
                        using (var reader = new StreamReader(dataStream))
                        {
                            // Read the content fully up to the end.
                            string responseFromServer = reader.ReadToEnd();
                            return ParseAnswer(responseFromServer);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return new List<ShippingOption>();
            }
        }

        private string GetParam()
        {
            var a = new StringBuilder();
            a.Append("to_city=" + CityTo);
            a.Append("&zip=" + Zip);
            a.Append("&weight=" + ShoppingCart.TotalShippingWeight.ToString("F3"));
            a.Append("&strah=" + (TotalPrice / Rate).ToString("F2"));
            a.Append("&id=" + ShopId);
            a.Append("&p=" + Password);
            return a.ToString();
        }

        private List<ShippingOption> ParseAnswer(string responseFromServer)
        {
            var shippingOptions = new List<ShippingOption>();
            if (String.IsNullOrEmpty(responseFromServer))
                return shippingOptions;

            using (var sr = new StringReader(responseFromServer))
            using (var reader = new XmlTextReader(sr))
            {
                bool flagStartTagStat = false;
                bool flagStartTagTarif = false;
                bool flagStartTagPrice = false;
                bool flagStartTagPriceCash = false;
                bool flagStartTagTransfer = false;
                bool flagStartTagDay = false;
                bool flagStartTagCompany = false;
                bool flagStartTagName = false;
                bool flagStartTagPickpointmap = false;


                string price = string.Empty;
                string priceCash = string.Empty;
                string transfer = string.Empty;
                string company = string.Empty;
                string name = string.Empty;
                string day = string.Empty;
                string pickpointmap = string.Empty;


                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "stat")
                                flagStartTagStat = true;

                            if (reader.Name == "tarif")
                                flagStartTagTarif = true;

                            if (reader.Name == "price")
                                flagStartTagPrice = true;

                            if (reader.Name == "pricecash")
                                flagStartTagPriceCash = true;

                            if (reader.Name == "transfer")
                                flagStartTagTransfer = true;
                            if (reader.Name == "pickpointmap")
                                flagStartTagPickpointmap = true;

                            if (reader.Name == "day")
                                flagStartTagDay = true;

                            if (reader.Name == "company")
                                flagStartTagCompany = true;

                            if (reader.Name == "name")
                                flagStartTagName = true;
                            break;
                        case XmlNodeType.Text:
                            if (flagStartTagStat)
                            {
                                flagStartTagStat = false;
                                if (reader.Value != "1")
                                {
                                    GetErrorEdost(reader.Value);
                                    return shippingOptions;
                                }
                            }
                            if (flagStartTagTarif && flagStartTagPrice)
                                price = reader.Value;

                            if (flagStartTagTarif && flagStartTagPriceCash)
                                priceCash = reader.Value;

                            if (flagStartTagTarif && flagStartTagTransfer)
                                transfer = reader.Value;

                            if (flagStartTagTarif && flagStartTagPickpointmap)
                                pickpointmap = reader.Value;

                            if (flagStartTagTarif && flagStartTagDay)
                                day = reader.Value;

                            if (flagStartTagTarif && flagStartTagCompany)
                                company = reader.Value;

                            if (flagStartTagTarif && flagStartTagName)
                                name = reader.Value;

                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name == "stat")
                                flagStartTagStat = false;

                            if (reader.Name == "tarif")
                            {
                                flagStartTagTarif = false;
                                decimal rate = price.TryParseDecimal() * Rate;
                                decimal cash = priceCash.TryParseDecimal() * Rate;
                                decimal cashTransfer = transfer.TryParseDecimal() * Rate;

                                var shippingOption = new ShippingOption
                                             {
                                                 Name = company + (string.IsNullOrWhiteSpace(name) ? string.Empty : " (" + name + ")"),
                                                 Rate = rate,
                                                 DeliveryTime = day,
                                                 // extension наложеный платеж или пикпойнт
                                                 Extend = priceCash.IsNotEmpty() && transfer.IsNotEmpty() ? GetExtended(rate, cash, cashTransfer, pickpointmap) : null
                                             };
                                if (String.IsNullOrEmpty(pickpointmap))
                                    shippingOptions.Add(shippingOption);
                                else
                                    shippingOptions.Insert(0, shippingOption);

                                price = string.Empty;
                                priceCash = string.Empty;
                                transfer = string.Empty;
                                company = string.Empty;
                                name = string.Empty;
                                day = string.Empty;
                                pickpointmap = string.Empty;
                            }

                            if (reader.Name == "price")
                                flagStartTagPrice = false;

                            if (reader.Name == "pricecash")
                                flagStartTagPriceCash = false;

                            if (reader.Name == "transfer")
                                flagStartTagTransfer = false;

                            if (reader.Name == "pickpointmap")
                                flagStartTagPickpointmap = false;

                            if (reader.Name == "day")
                                flagStartTagDay = false;

                            if (reader.Name == "company")
                                flagStartTagCompany = false;

                            if (reader.Name == "name")
                                flagStartTagName = false;
                            break;
                    }
                }
            }
            return shippingOptions;
        }

        public static ShippingOptionEx GetExtended(decimal bacePice, decimal pricecash, decimal transfer, string pickpointmap)
        {
            if (!string.IsNullOrEmpty(pickpointmap))
                return new ShippingOptionEx
                           {
                               Pickpointmap = pickpointmap,
                               Type = ExtendedType.Pickpoint
                           };

            else
            {
                return new ShippingOptionEx
                    {
                        BasePrice = bacePice,
                        PriceCash = pricecash,
                        Transfer = transfer,
                        Type = ExtendedType.CashOnDelivery
                    };
            }
        }

        //1 - успех
        //2 - доступ к расчету заблокирован
        //3 - неверные данные магазина (пароль или идентификатор)
        //4 - неверные входные параметры
        //5 - неверный город или страна
        //6 - внутренняя ошибка сервера расчетов
        //7 - не заданы компании доставки в настройках магазина
        //8 - сервер расчета не отвечает
        //9 - превышен лимит расчетов за день
        //11 - не указан вес
        //12 - не заданы данные магазина (пароль или идентификатор)
        private static void GetErrorEdost(string str)
        {
            try
            {
                switch (str)
                {
                    case "2":
                        throw new Exception("доступ к расчету заблокирован");

                    case "3":
                        throw new Exception("неверные данные магазина (пароль или идентификатор)");

                    case "4":
                        throw new Exception("неверные входные параметры");

                    case "5":
                        throw new Exception("неверный город или страна");

                    case "6":
                        throw new Exception("внутренняя ошибка сервера расчетов");

                    case "7":
                        throw new Exception("не заданы компании доставки в настройках магазина");

                    case "8":
                        throw new Exception("сервер расчета не отвечает");

                    case "9":
                        throw new Exception("превышен лимит расчетов за день");

                    case "11":
                        throw new Exception("не указан вес");

                    case "12":
                        throw new Exception("не заданы данные магазина (пароль или идентификатор)");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex, false);
            }
        }

    }
}