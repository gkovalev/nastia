//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport.Excel;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.SaasData;
using AdvantShop.Statistic;
using Resources;

public partial class Admin_ExportOrdersExcel : Page
{
    private readonly string _strFilePath;
    private readonly string _strFullPath;
    public string NotDoPost = "";
    public string Link = "";
    private const string StrFileName = "orders.xls";

    public Admin_ExportOrdersExcel()
    {
        _strFilePath = FoldersHelper.GetPathAbsolut(FolderType .PriceTemp);
        _strFullPath = string.Format("{0}{1}", _strFilePath, StrFileName);
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HaveExcel))
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        lError.Visible = false;
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportOrdersExcel_Title);
        if (IsPostBack) return;
        OutDiv.Visible = ExportStatistic.IsRun;
        linkCancel.Visible = ExportStatistic.IsRun;
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        if (ExportStatistic.IsRun == false)
        {
            List<Order> orders = null;
            orders = chkStatus.Checked ? OrderService.GetOrdersByStatusId(Convert.ToInt32(ddlStatus.SelectedValue)) : OrderService.GetAllOrders();
            if (chkDate.Checked)
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtDateFrom.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    orders.RemoveAll(o => o.OrderDate < d);
                }
                d = null;
                try
                {
                    d = DateTime.Parse(txtDateTo.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    orders.RemoveAll(o => o.OrderDate > d);
                }
            }
            if (orders.Count != 0)
            {
                ExportStatistic.Init();
                ExportStatistic.IsRun = true;
                ExportStatistic.TotalRow = orders.Count;


                linkCancel.Visible = true;
                OutDiv.Visible = true;
                btnDownload.Visible = false;
                pnSearch.Visible = false;
                try
                {
                    // Directory
                    if (!Directory.Exists(_strFilePath))
                        Directory.CreateDirectory(_strFilePath);

                    var tr = new Thread(Save);
                    ExportStatistic.ThreadImport = tr;
                    tr.Start(orders);
                }
                catch (Exception ex)
                {
                    //Debug.LogError(ex, sender, e);
                    Debug.LogError(ex);
                    lError.Text = ex.Message;
                    lError.Visible = true;
                }
            }
        }
        else
        {
            lError.Visible = true;
            lError.Text = Resource.Admin_Restrict_Action_In_demo;
        }
    }

    protected void Save(object arg)
    {
        var orders = (List<Order>)arg;
        var wrt = new ExcelXmlWriter();
        wrt.SaveOrdersToXml(_strFullPath, orders);
        ExportStatistic.IsRun = false;
        //ExportStatistic.ThreadImport.Abort();
    }

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (!ExportStatistic.ThreadImport.IsAlive) return;
        ExportStatistic.IsRun = false;
        ExportStatistic.Init();
    }

    protected void btnAsyncLoad_Click(object sender, EventArgs e)
    {
        linkCancel.Visible = false;
        NotDoPost = "true";
        var flag = true;
        while (flag)
        {
            try
            {
                using (FileStream f = File.Open(_strFullPath, FileMode.Open))
                {
                    flag = false;
                    f.Close();
                }
            }
            catch (Exception)
            {
                flag = true;
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (File.Exists(_strFullPath))
        {
            var f = new FileInfo(_strFullPath);
            const double size = 0;
            double sizeM = (Double)f.Length / 1048576;//1024 * 1024

            string sizeMesage;
            if ((int)sizeM > 0)
            {
                sizeMesage = ((int)sizeM) + " MB";
            }
            else
            {
                double sizeK = (Double)f.Length / 1024;
                if ((int)sizeK > 0)
                {
                    sizeMesage = ((int)sizeK) + " KB";
                }
                else
                {
                    sizeMesage = ((int)size) + " B";
                }
            }

            Link = "<a href='../" + FoldersHelper.GetPath(FolderType.PriceTemp, StrFileName,false)+ "' >" + Resource.Admin_ExportOrdersExcel_DownloadFile +
                   "</a><span> " + Resource.Admin_ExportOrdersExcel_FileSize + ": " + sizeMesage + "</span>";
            Link += "<span>" + ", " + AdvantShop.Localization.Culture.ConvertDate(File.GetLastWriteTime(_strFullPath)) + "</span>";
        }
        else
        {
            Link = "<span>" + Resource.Admin_ExportOrdersExcel_NotExistDownloadFile + "</span>";
        }
    }
    protected void sdsStatus_Init(object sender, EventArgs e)
    {
        sdsStatus.ConnectionString = Connection.GetConnectionString();
    }
}