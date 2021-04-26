//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Tools;
using Resources;

public partial class Tools_PhotoResizer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportExcel_Title);
        if (IsPostBack) return;
        OutDiv.Visible = ResizePhotoStatistic.IsRun;
        linkCancel.Visible = ResizePhotoStatistic.IsRun;
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        if (ResizePhotoStatistic.IsRun == false)
        {
            ResizePhotoStatistic.Init();
            ResizePhotoStatistic.IsRun = true;
            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnDownload.Visible = false;
            try
            {
                ResizePhotoStatistic.Count = PhotoService.GetCountPhotos(0, PhotoType.Product);
                var tr = new Thread(Resize);
                ResizePhotoStatistic.ThreadImport = tr;
                tr.Start();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                lError.Text = ex.Message;
                lError.Visible = true;
            }
        }
    }

    //get file withous lock file
    public static Image ImageFromFile(string fileName)
    {
        if (fileName == null) throw new ArgumentNullException("fileName");
        using (var lStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            return Image.FromStream(lStream);
    }

    protected void Resize()
    {
        try
        {
            var photos = PhotoService.GetNamePhotos(0, PhotoType.Product);
            foreach (var photo in photos)
            {
                var path = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Original, photo);
                if (File.Exists(path))
                {
                    using (var image = ImageFromFile(path))
                    {
                        FileHelpers.SaveProductImageUseCompress(path, image,true);
                    }
                }

                ResizePhotoStatistic.Index++;
            }
            
            ResizePhotoStatistic.IsRun = false;
            ResizePhotoStatistic.ThreadImport.Abort();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (!ResizePhotoStatistic.ThreadImport.IsAlive) return;
        ResizePhotoStatistic.ThreadImport.Abort();
        ResizePhotoStatistic.Init();
    }
}