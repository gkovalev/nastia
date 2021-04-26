//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.SaasData;
using Resources;
using System.Web;

public partial class Admin_ImportPropertiesElbuzCsv : Page
{

    #region ImportLogMessageType enum

    public enum ImportLogMessageType
    {
        ProductAdded,
        ProductUpdated,
        InvalidData,
        SuccessImport,
        ImportedWithErrors
    }

    #endregion

    private readonly string _filePath;
    private readonly string _fullPath;

    public Admin_ImportPropertiesElbuzCsv()
    {
        _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
        _fullPath = string.Format("{0}{1}", _filePath, "products.csv");
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lblError.Visible = false;
            lblError.Text = "";
        }
        else
        {
            lblError.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        lblError.Visible = true;
        lblError.Text = @"<br/>" + messageText;
    }


    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            if (fupZipPhotos.HasFile)
            {
                fupZipPhotos.SaveAs(HttpContext.Current.Server.MapPath("~/App_Data/elbuzZipPhotos.zip"));
                if(File.Exists(HttpContext.Current.Server.MapPath("~/App_Data/elbuzZipPhotos.zip")))
                {
                    FileHelpers.UnZipFile(HttpContext.Current.Server.MapPath("~/App_Data/elbuzZipPhotos.zip"),
                                          HttpContext.Current.Server.MapPath("~/pictures_elbuz"));
                }
            }
            if (!FileUpload1.HasFile)
            {
                MsgErr(Resource.Admin_ImportXLS_ChooseFile);
                return;
            }

            if (ImportPropertiesElbuzCsvStatistic.IsRun == false)
            {
                ImportPropertiesElbuzCsvStatistic.Init();
                ImportPropertiesElbuzCsvStatistic.IsRun = true;
                linkCancel.Visible = true;
                MsgErr(true);

                lblRes.Text = "";

                var boolSuccess = true;

                try
                {
                    if (Directory.Exists(_filePath) == false)
                    {
                        Directory.CreateDirectory(_filePath);
                    }

                    if (File.Exists(_fullPath))
                    {
                        File.Delete(_fullPath);
                    }
                }
                catch (Exception ex)
                {
                    MsgErr(ex.Message + " at Files");
                    boolSuccess = false;
                }

                if (boolSuccess == false)
                {
                    return;
                }
                pUploadExcel.Visible = false;
                // Save file

                FileUpload1.SaveAs(_fullPath);
                var tr = new Thread(ProcessData);
                ImportPropertiesElbuzCsvStatistic.ThreadImport = tr;
                tr.Start();

                pUploadExcel.Visible = false;

                OutDiv.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private static void Log(string message, ImportLogMessageType type)
    {
        try
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "INSERT INTO [Catalog].[ImportLog] (message, mtype) VALUES (@message, @mtype)";
                db.cmd.CommandType = CommandType.Text;

                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@mtype", Convert.ToInt32(type));
                db.cmd.Parameters.AddWithValue("@message", message);

                db.cnOpen();
                db.cmd.ExecuteNonQuery();
                db.cnClose();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExcel)
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ImportXLS_Title);

        if (!IsPostBack)
        {
            OutDiv.Visible = ImportPropertiesElbuzCsvStatistic.IsRun;
            linkCancel.Visible = ImportPropertiesElbuzCsvStatistic.IsRun;

            // Delete ImportLog data
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "DELETE [Catalog].[ImportLog]";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();
                    db.cnOpen();
                    db.cmd.ExecuteNonQuery();
                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }

    private void ProcessData()
    {
        using (var streamReaderCount = new StreamReader(_fullPath, Encoding.GetEncoding(1251)))
        {
            int count = 0;
            while (!streamReaderCount.EndOfStream)
            {
                streamReaderCount.ReadLine();
                count++;
            }
            streamReaderCount.Close();
            ImportPropertiesElbuzCsvStatistic.TotalRowExcel = count - 1;
        }

        var photos = Directory.GetFiles(Server.MapPath("~/pictures_elbuz/"));

        using (var streamReader = new StreamReader(_fullPath, Encoding.GetEncoding(1251)))
        {
            var headerString = streamReader.ReadLine();
            if (!string.IsNullOrEmpty(headerString))
            {
                var header = headerString.Split(new[] { "\t" }, StringSplitOptions.None);
                while (!streamReader.EndOfStream)
                {
                    var temp = streamReader.ReadLine();

                    if (!String.IsNullOrEmpty(temp))
                    {
                        var tempValue = temp.Split(new[] { "\t" }, StringSplitOptions.None);
                        ProcessProduct(tempValue, header, photos);
                    }
                    ImportPropertiesElbuzCsvStatistic.RowPosition++;
                }
            }
            streamReader.Close();
            ImportPropertiesElbuzCsvStatistic.IsRun = false;
            LuceneSearch.CreateAllIndexInBackground();
        }
    }

    private bool ProcessProduct(IList<string> temp, IList<string> header, IList<string> photos)
    {
        //0. Код товара	
        //1. Код категории	
        //2. Производитель	
        //3. Артикул	
        //4. Наименование	
        //5. Фото маленькое	
        //6. Фото большое	
        //7. Производитель	
        //8. Модель	
        //9. Описание	

        int productId = 0;
        if (!Int32.TryParse(temp[0], out productId))
        {
            ImportPropertiesElbuzCsvStatistic.TotalErrorRow++;
            Log("InvalidData in column 1 (ProductId)", ImportLogMessageType.InvalidData);
            return false;
        }
        var product = ProductService.GetProduct(temp[0]);
        if (product == null)
        {
            ImportPropertiesElbuzCsvStatistic.TotalErrorRow++;
            Log("Product not found (ProductId = " + temp[0] + ")", ImportLogMessageType.InvalidData);
            return false;
        }

        product.Description = temp[9];
        ProductService.UpdateProduct(product, false);

        //mainphoto
        if (photos.Contains(Server.MapPath("~/pictures_elbuz/") + "p" + product.ProductId))
            ProcessProductFoto(product.ProductId, Server.MapPath("~/pictures_elbuz/") + "p" + product.ProductId);

        //addonphotos
        foreach (var photo in photos.Where(item => item.Contains("p" + product.ArtNo) && item.Contains("addon")))
        {
            ProcessProductFoto(product.ProductId, photo);
        }

        for (int i = 10; i < header.Count; i++)
        {
            if (!string.IsNullOrEmpty(temp[i]) && !header[i].Contains("Foto№"))
            {
                PropertyService.UpdateOrInsertProductProperty(product.ProductId, header[i], temp[i], 0);
            }
        }

        Log(string.Format(Resource.Admin_Import1C_Updated, product.Name, product.ArtNo), ImportLogMessageType.ProductUpdated);
        ImportPropertiesElbuzCsvStatistic.TotalUpdateRow++;

        return true;
    }

    private void ProcessProductFoto(int productId, string photoPath)
    {
        if (File.Exists(photoPath))
        {
            var tempName = PhotoService.AddPhoto(new Photo(0, productId, PhotoType.Product) { OriginName = photoPath.Split('/').Last() });
            if (string.IsNullOrWhiteSpace(tempName))
            {
                try
                {
                    using (var image = Image.FromFile(photoPath))
                    {
                        FileHelpers.SaveProductImageUseCompress(tempName, image);
                        if (!string.IsNullOrWhiteSpace(tempName))
                        {
                            FileHelpers.SaveProductImageUseCompress(tempName, image);
                        }
                    }
                    File.Delete(photoPath);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }
    }

    #region  Save Photo
    public static void SaveResizePhotoFile(ProductImageType type, Image image, string destName, string productPathAbsolut)
    {
        var size = PhotoService.GetImageMaxSize(type);
        SaveResizePhotoFile(productPathAbsolut + type + "\\" + destName + "_" + type + ".jpg", size.Width, size.Height, image);
    }

    public static void SaveResizePhotoFile(string resultPath, double maxWidth, double maxHeight, Image image)
    {
        //UpdateDirectories();

        double resultWidth = image.Width;  // 0;
        double resultHeight = image.Height; // 0;

        if ((maxHeight != 0) && (image.Height > maxHeight))
        {
            resultHeight = maxHeight;
            resultWidth = (image.Width * resultHeight) / image.Height;
        }

        if ((maxWidth != 0) && (resultWidth > maxWidth))
        {
            resultHeight = (resultHeight * maxWidth) / resultWidth; // (resultHeight * resultWidth) / resultHeight;
            resultWidth = maxWidth;
        }
        try
        {
            using (var result = new Bitmap((int)resultWidth, (int)resultHeight))
            {
                result.MakeTransparent();
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    //graphics.FillRectangle(Brushes.White, 0, 0, (int)maxWidth, (int)maxHeight);
                    graphics.DrawImage(image, 0, 0, (int)resultWidth, (int)resultHeight);
                    //graphics.DrawImage(image, 0, Convert.ToInt32((maxHeight - resultHeight) / 2.0), (int)resultWidth, (int)resultHeight);

                    graphics.Flush();

                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    var myEncoderParameters = new EncoderParameters(1);
                    var myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                    using (var stream = new FileStream(resultPath, FileMode.Create))
                    {
                        result.Save(stream, jgpEncoder, myEncoderParameters);
                        stream.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex, "Error on upload " + resultPath);
        }
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
    }
    #endregion

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (ImportPropertiesElbuzCsvStatistic.ThreadImport.IsAlive)
        {
            ImportPropertiesElbuzCsvStatistic.ThreadImport.Abort();
            ImportPropertiesElbuzCsvStatistic.Init();
            hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
        }
    }
}