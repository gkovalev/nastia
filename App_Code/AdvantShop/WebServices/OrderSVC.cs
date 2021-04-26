//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using System.Xml;
using AdvantShop.Orders;
using AdvantShop.Security;


// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
// <System.Web.Script.Services.ScriptService()> _
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class OrderSVC : WebService
{
    private const string MsgListError = "Error getting list of orders";
    private const string MsgNotFound = "Order {0} not found";
    private const string MsgStatusError = "Status {0} not found";
    private const string MsgStatusesError = "Statuses not found";
    private const string MsgAuthFailed = "Access denied, please login";

    /// <summary>
    /// LogIn as admin and write login data to cookies
    /// </summary>
    /// <param name="login">Admin login</param>
    /// <param name="password">Admin password</param>
    /// <returns>True if successfull login</returns>
    [WebMethod]
    public bool Login(string login, string password)
    {
        return AuthorizeService.LoginAdmin(login, password, false);
    }
    /// <summary>
    /// Logout and delete user cookies
    /// </summary>
    [WebMethod]
    public void Logout()
    {
        AuthorizeService.DeleteCookie();
    }

    [WebMethod]
    public XmlDocument ExportOrdersToXml()
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);
        List<Order> orders = OrderService.GetAllOrders();
        if (orders == null)
            return ErrMsg(MsgListError);
        using (var writer = new StringWriter())
        {
            OrderService.SerializeToXml(orders, writer);
            var xml = new XmlDocument();
            xml.Load(writer.ToString());
            return xml;
        }
    }

    [WebMethod]
    public XmlDocument ExportOrderToXml(int orderId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(string.Format(MsgNotFound, orderId));
        Order order = OrderService.GetOrder(orderId);
        if (order == null)
            return ErrMsg(MsgListError);
        using (var writer = new StringWriter())
        {
            OrderService.SerializeToXml(order, writer);
            var xml = new XmlDocument();
            xml.Load(writer.ToString());
            return xml;
        }
    }

    [WebMethod]
    public XmlDocument ExportOrderToXmlByStatus(int statusId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);
        var stats = OrderService.GetOrderStatuses(false);
        if (! stats.ContainsKey(statusId.ToString()))
        {
            return ErrMsg(string.Format(MsgStatusError, statusId));
        }
        var orders = OrderService.GetOrdersByStatusId(statusId);
        if (orders == null)
            return ErrMsg(MsgListError);
        using (var writer = new StringWriter())
        {
            OrderService.SerializeToXml(orders, writer);
            var xml = new XmlDocument();
            xml.Load(writer.ToString());
            return xml;
        }
    }

    [WebMethod]
    public XmlDocument ExportOrderToXmlByStatusName(string statusName)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);
        Dictionary<string, string> stats = OrderService.GetOrderStatuses(true);
        if (! stats.ContainsKey(statusName))
        {
            return ErrMsg(string.Format(MsgStatusError, statusName));
        }
        List<Order> orders = OrderService.GetOrdersByStatusId(Int32.Parse(stats[statusName]));
        if (orders == null)
            return ErrMsg(MsgListError);
        using (var writer = new StringWriter())
        {
            OrderService.SerializeToXml(orders, writer);
            var xml = new XmlDocument();
            xml.Load(writer.ToString());
            return xml;
        }
    }

    [WebMethod]
    public XmlDocument GetOrderStatuses()
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);
        var result = new XmlDocument();
        XmlElement root = result.CreateElement("Statuses");
        Dictionary<string, string> stats = OrderService.GetOrderStatuses(false);
        if (stats == null)
        {
            return ErrMsg(MsgStatusesError);
        }
        foreach (var stat in stats)
        {
            XmlElement status = result.CreateElement("Status");
            XmlAttribute index = result.CreateAttribute("ID");
            index.Value = stat.Key;
            status.Attributes.Append(index);
            status.InnerText = stat.Value;
            root.AppendChild(status);
        }
        result.AppendChild(root);
        return result;
    }

    private static XmlDocument ErrMsg(string errTxt)
    {
        var result = new XmlDocument();
        XmlElement errorXml = result.CreateElement("Error");
        errorXml.InnerText = errTxt;
        result.AppendChild(errorXml);
        return result;
    }
}