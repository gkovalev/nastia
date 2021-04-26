//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.itmcompany.ru
//--------------------------------------------------

using System;
using System.IO;
using System.Drawing;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

public partial class Admin_UploadImages : System.Web.UI.Page
{

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    public enum eStates
    {
        err = 0,
        norm = 1
    }

    private void msgErr(bool clean)
    {
        if (clean == true)
        {
            lError.Visible = false;
            lError.Text = "";
        }
        else
        {
            lError.Visible = false;
        }

    }

    private void msgErr(string lErrorText)
    {
        lError.Visible = true;
        lError.Text += "<br/>" + lErrorText;

    }

    private void msgErr(string lErrorText, eStates state)
    {
        lError.Visible = true;
        if (state == eStates.err)
        {
            lError.Text += "<br/>" + "<span style='Color:red'>" + lErrorText + "</span>";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = SettingsMain.ShopName + @" - Upload images";
    }

    /// <summary>
    /// «агружает фото из одной папки в другую и обновл€ет данные в базе
    /// </summary>
    /// <returns>количество загруженных файлов, -1 если загрузка не удалась</returns>
    /// <remarks></remarks>
    private int UploadImages()
    {
        string sourceFolder = (Server.MapPath(txtSource.Text));

        int upCounter = 0;

        if (!Directory.Exists(sourceFolder))
        {
            msgErr(Resources.Resource.Admin_UploadImages_SourceFolderNotFound);
            return -1;
        }

        var di = new DirectoryInfo(sourceFolder);
        FileInfo[] files = di.GetFiles("*.jpg");

        foreach (FileInfo fi in files)
        {
            var productId = PhotoService.CheckImageInDataBase(fi.Name);
            if (productId == 0) // -1
            {
                try
                {
                    if (fi.Name.StartsWith("no_product_") == false)
                    {
                        fi.CopyTo(fi.DirectoryName + "\\no_product_" + fi.Name);
                        fi.Delete();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    msgErr(string.Format(Resources.Resource.Admin_UploadImages_ProductNotFound, fi.Name), eStates.err);
                }
            }
            else
            {
                try
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, productId, PhotoType.Product) { OriginName = fi.Name });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (Image image = Image.FromFile(sourceFolder + "\\" + fi.Name))
                        {
                            FileHelpers.SaveProductImageUseCompress(tempName, image);
                        }
                        msgErr(string.Format(Resources.Resource.Admin_UploadImages_4Photo, fi.Name));
                        upCounter += 1;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    msgErr(string.Format(Resources.Resource.Admin_UploadImages_AddImageError, fi.Name), eStates.err);
                }
            }
        }
        msgErr(string.Format(Resources.Resource.Admin_UploadImages_UploadFinal, upCounter));
        return upCounter;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        UploadImages();
    }
}