//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;

public partial class CleanUp : Page
{
    protected void btnCleanUpPictureFolder_Click(object sender, EventArgs e)
    {
        var fileNames = new List<string>();

        foreach (var photo in PhotoService.GetNamePhotos(0, PhotoType.Product))
        {
            fileNames.Add(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photo));
            fileNames.Add(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, photo));
            fileNames.Add(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, photo));
            fileNames.Add(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.XSmall, photo));
        }

        foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.CategoryBig))
        {
            fileNames.Add(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, photo));
        }

        foreach (string photo in PhotoService.GetNamePhotos(0, PhotoType.CategorySmall))
        {
            fileNames.Add(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, photo));
        }

        //Add Logo image in exceptions
        fileNames.Add(SettingsMain.LogoImageName);

        var files = new List<string>();
        files.AddRange(Directory.GetFiles(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, string.Empty)));
        files.AddRange(Directory.GetFiles(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, string.Empty)));
        files.AddRange(Directory.GetFiles(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, string.Empty)));
        files.AddRange(Directory.GetFiles(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.XSmall, string.Empty)));

        files.AddRange(Directory.GetFiles(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, string.Empty)));// GetImageCategoryPathAbsolut(false)));
        files.AddRange(Directory.GetFiles(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, string.Empty)));// GetImageCategoryPathAbsolut(false)));

        var deleted = new List<string>();

        foreach (string file in files)
        {
            if (!fileNames.Contains(file) || (file.Trim().Length == 0))
            {
                if (chboxDeleteFiles.Checked)
                {
                    File.Delete(file);
                }
                deleted.Add(file);
            }
        }

        if (!chboxDeleteFiles.Checked)
        {
            lCompleted.Text = @"Analysis successfully completed";
        }

        lCompleted.Visible = true;

        var res = new StringBuilder();

        foreach (string del in deleted)
        {
            res.Append(del);
            res.Append("<br />");
        }

        if (deleted.Count > 0)
        {
            if (!chboxDeleteFiles.Checked)
            {
                lResultHeader.Text = @"Files to delete";
            }
            lResultHeader.Visible = true;
            lResult.Text = res.ToString();
        }
        else
        {
            lResultHeader.Text = @"No unnecessary files";
            lResultHeader.Visible = true;
        }
    }

    protected void btnCleanUpBD_Click(object sender, EventArgs e)
    {
        var res = new StringBuilder();

        try
        {
            foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.Product))
            {
                if ((((!File.Exists(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photoName))) ||
                      (!File.Exists(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, photoName)))) ||
                     (!File.Exists(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, photoName)))) ||
                    (!File.Exists(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.XSmall, photoName))))
                {
                    if (chboxMakeNull.Checked)
                    {
                        PhotoService.DeleteProductPhotoWithPath(photoName);
                        res.AppendFormat("Link to product photo {0} was deleted<br />", photoName);
                    }
                    else
                    {
                        res.AppendFormat("No product photos {0}<br />", photoName);
                    }

                    res.Append(CheckProductFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photoName)));
                    res.Append(CheckProductFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, photoName)));
                    res.Append(CheckProductFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, photoName)));
                    res.Append(CheckProductFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.XSmall, photoName)));
                }
            }

            foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.CategoryBig))
            {
                res.Append(CheckCategoryFile( photoName));
                res.Append(CheckCategoryFile(photoName));
            }

            foreach (var photoName in PhotoService.GetNamePhotos(0, PhotoType.CategorySmall))
            {
                res.Append(CheckCategoryFile(photoName));
                res.Append(CheckCategoryFile(photoName));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

        if (!chboxMakeNull.Checked)
        {
            lDBCleanupCompleted.Text = @"Analysis successfully completed";
        }

        lDBCleanupCompleted.Visible = true;

        lDBResult.Text = string.IsNullOrEmpty(res.ToString()) ? @"No items to correct" : res.ToString();
    }

    private string CheckCategoryFile(string filename)
    {
        if (!File.Exists(filename))
        {
            if (chboxMakeNull.Checked)
            {
                File.Delete(filename);
                return string.Format("Link to category image for category {0} was deleted<br />", Path.GetFileName(filename));
            }

            return string.Format("Category {0} has broken link to {1} category image<br />", Path.GetFileName(filename));
        }
        return "";
    }

    private string CheckProductFile(string photoName)
    {
        if (!File.Exists(photoName))
        {
            if (chboxMakeNull.Checked)
            {
                File.Delete(photoName);
                return string.Format("File {0}{1} deleted<br />", photoName, Path.GetFileName(photoName));
            }

            return string.Format("File {0}{1} will be deleted<br />", photoName, Path.GetFileName(photoName));
        }

        return "";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        lblDeleteSessionResult.Visible = true;

        try
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "DELETE FROM [dbo].[ASPStateTempApplications]";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.Parameters.Clear();
                db.cnOpen();
                db.cmd.ExecuteReader();
                db.cnClose();

                db.cmd.CommandText = "DELETE FROM [dbo].[ASPStateTempSessions]";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.Parameters.Clear();
                db.cnOpen();
                db.cmd.ExecuteReader();
                db.cnClose();

                lblDeleteSessionResult.Text = "Done!";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            lblDeleteSessionResult.Text = "Error!";
        }
    }
}