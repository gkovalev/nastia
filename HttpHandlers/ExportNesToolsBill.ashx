<%@ WebHandler Language="C#" Class="ExportNesToolsBill" %>

using System;
using System.Web;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport.Excel;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;

public class ExportNesToolsBill : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request["OrderID"] != null)
        {
            string strPath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(strPath);
            try
            {
                var ord = OrderService.GetOrder(Convert.ToInt32(context.Request["OrderID"]));
                var filename = String.Format("NesToolsOrder{0}.xls", ord.Number);
                var wrt = new ExcelSingleNesToolsBillWriter();
                wrt.Generate(strPath + filename, ord);
                CommonHelper.WriteResponseXls(strPath + filename, filename);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error on creating xls document");
            }
        }
        else
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Error on creating xls document");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}