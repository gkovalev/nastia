//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Orders;

namespace AdvantShop.ExportImport
{
    public class ImportOrdersRitmZ
    {
        // it work with using file to mimimaze memory used
        private enum RitmZErrors
        {
            StartTimeInvalid,
            EndTimeInvalid
        }

        //� �����������;
        //� �������;
        //� ��������� ������;
        //� ����������� �������;
        //� �� �����������;
        //� �������;
        //� ���������;
        //� ��������;
        //� �������;
        //� ��������� �������;
        //� ������;
        //� ��������� ��������;
        //� ��������� ������.

        public static void Import(DateTime? start, DateTime? end, string exportWay)
        {
            var listErr = new List<RitmZErrors>();
            if (start == null) listErr.Add(RitmZErrors.StartTimeInvalid);
            if (end == null) listErr.Add(RitmZErrors.EndTimeInvalid);

            if (listErr.Count > 0)
            {
                WriteError(listErr, exportWay);
                return;
            }

            GetOrders(start, end);
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
                    {
                        writer.WriteLine(string.Format("<error>{0}</error>", GetErrorString(list[i])));
                    }
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

        public static void GetOrders(DateTime? start, DateTime? end)
        {

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            var endPointAddr = new EndpointAddress("http://cc.Ritm-Z.com:8008/RitmZ_GM82/ws/GetOrders.1cws");

            var getOrders = new RitmZOrders.WSGetOrdersPortTypeClient(binding, endPointAddr);

            getOrders.ClientCredentials.UserName.UserName = SettingsRitmz.RitmzLogin;
            getOrders.ClientCredentials.UserName.Password = SettingsRitmz.RitmzPassword;
            if (start == null || end == null)
                return;
            var ritmzOrders = getOrders.��������������(start.Value.ToString(), end.Value.ToString());
            if (ritmzOrders.orders != null)
            {
                for (int i = 0; i < ritmzOrders.orders.Length; ++i)
                {
                    var order = OrderService.GetOrder(Convert.ToInt32(ritmzOrders.orders[i].id));
                    if (order != null)
                    {
                        var customerName = ritmzOrders.orders[i].c_name.Split(new[] { " " },
                                                                              StringSplitOptions.RemoveEmptyEntries);
                        order.OrderCustomer.FirstName = customerName[0];
                        order.OrderCustomer.LastName = customerName.Length > 1 ? customerName[0] : string.Empty;
                        if (order.ShippingContact != null)
                        {
                            order.ShippingContact.Address = ritmzOrders.orders[i].c_address + " " + ritmzOrders.orders[i].c_contacts;
                        }
                        else
                        {
                            order.ShippingContact = new OrderContact
                                                        {
                                                            Address = ritmzOrders.orders[i].c_address + " " + ritmzOrders.orders[i].c_contacts
                                                        };
                        }
                        order.Sum = Convert.ToDecimal(ritmzOrders.orders[i].incl_deliv_sum.Replace(".", ","));
                        var oldStatus = order.OrderStatus;
                        order.OrderStatus = OrderService.GetOrderStatusByName(ritmzOrders.orders[i].o_state);
                        order.CustomerComment = ritmzOrders.orders[i].descriptions;
                        order.OrderCustomer.Email = ritmzOrders.orders[i].email;
                        foreach (var ritmzOrderItem in ritmzOrders.orders[i].items)
                        {
                            OrderItem orderItem = null;
                            orderItem = order.OrderItems.FirstOrDefault(item => item.ArtNo == ritmzOrderItem.id);
                            if(orderItem!= null)
                            {
                                orderItem.Amount = Convert.ToInt32(ritmzOrderItem.quantity);
                                orderItem.Price = Convert.ToDecimal(ritmzOrderItem.price);
                            }
                        }

                        OrderService.UpdateOrderMain(order);
                        OrderService.AddUpdateOrderItems(order.OrderItems, order.OrderID);

                        if (oldStatus != order.OrderStatus)
                        {
                            OrderService.UpdateStatusComment(order.OrderID, string.Empty);
                            Modules.ModulesRenderer.OrderChangeStatus(order.OrderID);
                        }

                        Modules.ModulesRenderer.OrderUpdated(order.OrderID);
                    }
                }
            }
        }
    }
}