//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using AdvantShop.Core;
using AdvantShop.Orders;

namespace AdvantShop.ExportImport
{
    public class ExportOrderRitmZ
    {
        public const string ExportFile = "exportRitmZ.xml";
        public const string ExporDir = "~/Export";

        // it work with using file to mimimaze memory used
        private enum RitmZErrors
        {
            StartTimeInvalid,
            EndTimeInvalid
        }

        public static void Export(DateTime? start, DateTime? end, string exportWay)
        {
            var listErr = new List<RitmZErrors>();
            if (start == null) listErr.Add(RitmZErrors.StartTimeInvalid);
            if (end == null) listErr.Add(RitmZErrors.EndTimeInvalid);

            if (listErr.Count > 0)
            {
                WriteError(listErr, exportWay);
                return;
            }

            var listOrder = SQLDataAccess.ExecuteReadList<Order>("Select * from [Order].[Order] where OrderDate > @OrderStart and OrderDate < @OrderEnd  ",
                                                           CommandType.Text,
                                                           OrderService.GetOrderFromReader,
                                                           new SqlParameter { ParameterName = "@OrderStart", Value = start },
                                                           new SqlParameter { ParameterName = "@OrderEnd", Value = end }
                                                           );

            using (var fs = new FileStream(exportWay, FileMode.Create))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                if (listOrder.Count > 0)
                {
                    writer.WriteLine("<orders>");
                    foreach (Order t in listOrder)
                    {
                        if (t.OrderCustomer == null)
                        {
                            continue;
                        }

                        writer.WriteLine(string.Format("<order id=\"{0}\">", t.OrderID));
                        writer.WriteLine(string.Format("<c_name>{0}</c_name>", t.OrderCustomer.FirstName + " " + t.OrderCustomer.LastName));
                        writer.WriteLine(string.Format("<c_contacts>{0}</c_contacts>", t.OrderCustomer.MobilePhone));
                        writer.WriteLine(string.Format("<c_address>{0}</c_address>", t.ShippingContact.Zip + " " + t.ShippingContact.City + " " + t.ShippingContact.Address));
                        writer.WriteLine(string.Format("<d_date>{0}</d_date>", string.Empty));
                        writer.WriteLine(string.Format("<b_time>{0}</b_time>", string.Empty));
                        writer.WriteLine(string.Format("<e_time>{0}</e_time>", string.Empty));
                        writer.WriteLine(string.Format("<incl_deliv_sum>{0}</incl_deliv_sum>", t.Sum.ToString("F2").Replace(",", ".")));
                        writer.WriteLine(string.Format("<d_type>{0}</d_type>", 1));//ShippingType(t.ShippingMethodName)));
                        writer.WriteLine(string.Format("<e_mail>{0}</e_mail>", t.OrderCustomer.Email));
                        writer.WriteLine(string.Format("<descriptions>{0}</descriptions>", 
                            t.OrderDiscount != 0 
                                ? string.Format("������:{0} ; ����������� ����������:{1}",t.OrderDiscount,t.CustomerComment)
                                : t.CustomerComment));
                        writer.WriteLine("<items>");
                        var items = t.OrderItems;
                        WriteItems(writer, (List<OrderItem>)items);
                        writer.WriteLine("</items>");
                        writer.WriteLine("</order>");
                    }
                    writer.WriteLine("</orders>");
                }
            }
        }

        private static int ShippingType(string str)
        {
            if (str.ToLower().Contains("������".ToLower()))
                return 1;
            if (str.ToLower().Contains("����� ��c���".ToLower()))
                return 2;
            if (str.ToLower().Contains("EMS;".ToLower()))
                return 3;
            if (str.ToLower().Contains("���������;".ToLower()))
                return 4;
            return -1;
        }

        private static void WriteItems(StreamWriter writer, List<OrderItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                writer.WriteLine("<item>");
                writer.WriteLine(string.Format("<id>{0}</id>", items[i].ArtNo));
                writer.WriteLine(string.Format("<name>{0}</name>", items[i].Name));
                writer.WriteLine(string.Format("<quantity>{0}</quantity>", items[i].Amount.ToString("F2").Replace(",", ".")));
                writer.WriteLine(string.Format("<price>{0}</price>", items[i].Price.ToString("F2").Replace(",", ".")));
                writer.WriteLine("<chars>");
                foreach (var option in items[i].SelectedOptions)
                {
                    writer.WriteLine(string.Format("<char name=\"{0}\" val=\"{1}\"></char>", option.CustomOptionTitle, option.OptionTitle));
                }
                writer.WriteLine("</chars>");
                writer.WriteLine("</item>");
            }
        }

        private static void WriteError(List<RitmZErrors> list, string exportWay)
        {
            using (var fs = new FileStream(exportWay, FileMode.Create))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                if (list.Count > 0)
                {
                    writer.WriteLine("<errors>");
                    for (int i = 0; i < list.Count; i++)
                        writer.WriteLine(string.Format("<error>{0}</error>", GetErrorString(list[i])));
                    writer.WriteLine("</errors>");
                }
            }
        }

        private static string GetErrorString(RitmZErrors item)
        {
            switch (item)
            {
                case RitmZErrors.StartTimeInvalid:
                    return "�� ������� ���� ������ ��������";
                case RitmZErrors.EndTimeInvalid:
                    return "�� ������� ���� ��������� ��������";
                default:
                    return string.Empty;
            }
        }


        public static void WriteToResponce(System.Web.HttpResponse httpResponse, string exportWay)
        {
            using (var fs = new FileStream(exportWay, FileMode.Open))
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    httpResponse.Write(line);
                }
            }
        }
    }
}