//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.SaasData;
using Resources;

public partial class Admin_Export1C : Page
{
    private readonly string _strFilePath;
    private readonly string _strFullPath;
    private const string StrFileName = "orders.xml";

    public Admin_Export1C()
    {
        //strFilePath = Server.MapPath("~/price_temp/");
        //strFullPath = Server.MapPath("~/price_temp/") + "products.xml";

        _strFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
        _strFullPath = string.Format("{0}{1}", _strFilePath, StrFileName);
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
     
            List<Order> orders;
            try
            {
                FileHelpers.CreateDirectory(_strFilePath);
                // Data
                orders = OrderService.GetOrdersByStatusId(OrderService.DefaultOrderStatus);
            }
            catch (Exception ex)
            {
                //Debug.LogError(ex, sender, e);
                Debug.LogError(ex);
                lError.Text = ex.Message;
                lError.Visible = true;
                return;
            }

            // File transfer
            try
            {
                SaveXml(orders, _strFullPath);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                lError.Text = ex.Message;
                lError.Visible = true;
                return;
           }
            Response.Redirect("~/HttpHandlers/FileDownloader.ashx?file=~/price_temp/orders.xml");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.Have1C)
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Export1C_Title);
    }

    private static void SaveXml(List<Order> orders, string file)
    {
        using (var writer = new StreamWriter(file))
        {
            OrderService.SerializeToXml(orders, writer);
        }
    }
}