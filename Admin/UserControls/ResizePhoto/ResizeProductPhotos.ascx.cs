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
using AdvantShop.Modules;
using AdvantShop.Tools;

public partial class Admin_UserControls_ResizePhoto_Product : System.Web.UI.UserControl
{
    public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidBank;
    public bool IsChanged = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        LoadData();
        OutDiv.Visible = ResizePhotoStatistic.IsRun;
        linkCancel.Visible = ResizePhotoStatistic.IsRun;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!SaveData())
            return;

        if (ResizePhotoStatistic.IsRun == false)
        {
            ResizePhotoStatistic.Init();
            ResizePhotoStatistic.IsRun = true;
            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnSave.Visible = false;
            try
            {
                //todo rewrite
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
            foreach (var photo in PhotoService.GetNamePhotos(0, PhotoType.Product))
            {
                var originalPath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Original, photo);
                if (File.Exists(originalPath))
                {
                    using (var image = ImageFromFile(originalPath))
                    {
                        ResizeCurrentPhoto(image, photo);
                    }
                }
                else
                {
                    var bigPath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photo);
                    if (File.Exists(originalPath))
                    {
                        File.Copy(bigPath, originalPath);
                        if (File.Exists(originalPath))
                        {
                            using (var image = ImageFromFile(originalPath))
                            {
                                ResizeCurrentPhoto(image, photo);
                            }
                        }
                    }
                }
                ResizePhotoStatistic.Index++;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        ResizePhotoStatistic.IsRun = false;
    }

    private void ResizeCurrentPhoto(Image image, string destName)
    {
        FileHelpers.UpdateDirectories();

        ModulesRenderer.ProcessPhoto(image);

        if (SettingsCatalog.CompressBigImage)
        {
            FileHelpers.SaveResizePhotoFile(ProductImageType.Big, image, destName);
        }
        else
        {
            //не удалять, создаем еще один imaga из-за багов в формате файла, если сохранять напрямую, выдает исключение GDI+
            using (Image img = new Bitmap(image))
            {
                img.Save(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, destName));
            }
        }

        FileHelpers.SaveResizePhotoFile(ProductImageType.Middle, image, destName);
        FileHelpers.SaveResizePhotoFile(ProductImageType.Small, image, destName);
        FileHelpers.SaveResizePhotoFile(ProductImageType.XSmall, image, destName);
    }
    

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (!ResizePhotoStatistic.ThreadImport.IsAlive) return;
        ResizePhotoStatistic.ThreadImport.Abort();
        ResizePhotoStatistic.Init();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    private void LoadData()
    {
        txtBigHight.Text = SettingsPictureSize.BigProductImageHeight.ToString();
        txtBigWidth.Text = SettingsPictureSize.BigProductImageWidth.ToString();

        txtMiddleHeight.Text = SettingsPictureSize.MiddleProductImageHeight.ToString();
        txtMiddleWidth.Text = SettingsPictureSize.MiddleProductImageWidth.ToString();

        txtSmallHeight.Text = SettingsPictureSize.SmallProductImageHeight.ToString();
        txtSmallWidth.Text = SettingsPictureSize.SmallProductImageWidth.ToString();

        txtXSmallHeight.Text = SettingsPictureSize.XSmallProductImageHeight.ToString();
        txtXSmallWidth.Text = SettingsPictureSize.XSmallProductImageWidth.ToString();
    }

    public bool SaveData()
    {
        if (SettingsPictureSize.BigProductImageHeight != Convert.ToInt32(txtBigHight.Text))
        {
            SettingsPictureSize.BigProductImageHeight = Convert.ToInt32(txtBigHight.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.BigProductImageWidth != Convert.ToInt32(txtBigWidth.Text))
        {
            SettingsPictureSize.BigProductImageWidth = Convert.ToInt32(txtBigWidth.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.MiddleProductImageHeight != Convert.ToInt32(txtMiddleHeight.Text))
        {
            SettingsPictureSize.MiddleProductImageHeight = Convert.ToInt32(txtMiddleHeight.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.MiddleProductImageWidth != Convert.ToInt32(txtMiddleWidth.Text))
        {
            SettingsPictureSize.MiddleProductImageWidth = Convert.ToInt32(txtMiddleWidth.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.SmallProductImageHeight != Convert.ToInt32(txtSmallHeight.Text))
        {
            SettingsPictureSize.SmallProductImageHeight = Convert.ToInt32(txtSmallHeight.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.SmallProductImageWidth != Convert.ToInt32(txtSmallWidth.Text))
        {
            SettingsPictureSize.SmallProductImageWidth = Convert.ToInt32(txtSmallWidth.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.XSmallProductImageHeight != Convert.ToInt32(txtXSmallHeight.Text))
        {
            SettingsPictureSize.XSmallProductImageHeight = Convert.ToInt32(txtXSmallHeight.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.XSmallProductImageWidth != Convert.ToInt32(txtXSmallWidth.Text))
        {
            SettingsPictureSize.XSmallProductImageWidth = Convert.ToInt32(txtXSmallWidth.Text);
            IsChanged = true;
        }

        LoadData();
        return true;
    }

    private bool ValidateData()
    {
        return true;
    }
}