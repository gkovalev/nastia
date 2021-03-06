//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using Resources;

public partial class ImportBillion : Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        AdvantShop.Localization.Culture.InitializeCulture();
        Page.Title = string.Format("{0}", Resource.Admin_ImportXLS_Title);

        if (!IsPostBack)
        {
            OutDiv.Visible = ImportStatistic.IsRun;
            linkCancel.Visible = ImportStatistic.IsRun;

            // Delete ImportLog data
            ExcelLog.DeleteLog();
        }
    }

    #region " Basic functions "

    private readonly string _filePath;
    private readonly string _fullPath;
    private bool useMultiThreadImport = false;

    public ImportBillion()
    {
        _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
        _fullPath = string.Format("{0}{1}", _filePath, "importExcel.xls");
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
            lblError.Text = string.Empty;
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

    private void UpdateInsertProduct(Product product)
    {
        if (useMultiThreadImport)
        {
            var added = false;
            while (!added)
            {
                int workerThreads;
                int asyncIoThreads;
                ThreadPool.GetAvailableThreads(out workerThreads, out asyncIoThreads);
                if (workerThreads != 0)
                {
                    ThreadPool.QueueUserWorkItem(UpdateInsertProductWorker, product);
                    added = true;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
        else
        {
            UpdateInsertProductWorker(product);
        }
    }

    private static void UpdateInsertProductWorker(object o)
    {
        AdvantShop.Localization.Culture.InitializeCulture();
        var product = (Product)o;
        bool addingNew;
        Product p = null;

        if (string.IsNullOrEmpty(product.ArtNo))
        {
            addingNew = true;
            // to do generate new ArtNO
            product.ArtNo = null;
        }
        else
        {
            p = ProductService.GetProduct(product.ArtNo);
            addingNew = p.ID == 0;
        }
        if (!addingNew)
        {
            product.ProductId = p.ProductId;
            product.UrlPath = p.UrlPath;
            //product.MetaId = p.MetaId;
            product.Recomended = p.Recomended;
            product.OnSale = p.OnSale;
            product.BestSeller = p.BestSeller;
            product.New = p.New;
            ProductService.UpdateProduct(product, false); // UpdateProductByArtNo(product);
            Log(string.Format(Resource.Admin_Import1C_Updated, p.Name, p.ArtNo), ExcelLog.ImportLogMessageType.ProductUpdated);
            ImportStatistic.TotalUpdateRow++;
        }
        else
        {
            ProductService.AddProduct(product, false);
            //RouteService.InsertParam(product.ArtNo, "ProductID", product.Id, "Details.aspx");
            Log(string.Format(Resource.Admin_Import1C_Added, product.Name, product.ArtNo), ExcelLog.ImportLogMessageType.ProductAdded);
            ImportStatistic.TotalAddRow++;
        }
        ImportStatistic.RowPosition++;

        if (ImportStatistic.TotalRow == ImportStatistic.RowPosition)
        {
            ImportStatistic.IsRun = false;
            LuceneSearch.CreateAllIndexInBackground();
        }
    }

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (ImportStatistic.ThreadImport.IsAlive)
        {
            ImportStatistic.ThreadImport.Abort();
            ImportStatistic.IsRun = false;
            ImportStatistic.Init();
            hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
        }
    }

    private static string GetSizeForBdFormat(string str)
    {
        if (string.IsNullOrEmpty(str)) return "0|0|0";

        var listSymb = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.' };
        string res = string.Empty;
        var list = new List<string>();
        foreach (char t in str)
        {
            if (listSymb.Contains(t))
            {
                res += t;
            }
            else
            {
                if (!string.IsNullOrEmpty(res))
                {
                    list.Add(res.Trim());
                    res = string.Empty;
                }
            }
        }
        if (!string.IsNullOrEmpty(res))
            list.Add(res.Trim());

        res = list.Aggregate(string.Empty, (current, item) => current + (item + "|"));
        res = res.Remove(res.Length - 1, 1);
        return res;
    }

    private static void LogInvalidData(string value, int column, int row)
    {
        ExcelLog.LogInvalidData(value, column, row);
        ImportStatistic.TotalErrorRow++;
        ImportStatistic.RowPosition++;
    }

    private static void Log(string message, ExcelLog.ImportLogMessageType type)
    {
        ExcelLog.Log(message, type);
    }

    #endregion

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {

            if (!ImportStatistic.IsRun)
            {
                ImportStatistic.Init();
                ImportStatistic.IsRun = true;
                linkCancel.Visible = true;
                MsgErr(true);
                lblRes.Text = string.Empty;

                bool boolSuccess = true;

                // Files

                try
                {
                    FileHelpers.CreateDirectory(_filePath);
                    FileHelpers.DeleteFile(_fullPath);
                }
                catch (Exception ex)
                {
                    MsgErr(ex.Message + " at Files");
                    boolSuccess = false;
                }

                if (!boolSuccess)
                {
                    return;
                }
                pUploadExcel.Visible = false;
                // Save file

                //FileUpload1.SaveAs(_fullPath);
                var tr = new Thread(ProcessExcel);
                ImportStatistic.ThreadImport = tr;
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

    private void ProcessExcel()
    {

        try
        {

            // How much ? ------------------

            int intRootCategories = 7;
            int intSubCategoriesLevel1 = 7;
            int intSubCategoriesLevel2 = 7;

            int intProductCountForCategory = 150;

            // Internal Variables ------------------

            int intCurrentProductIndex = SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("Select MAX(ProductId) + 1 FROM Catalog.Product", CommandType.Text, null));
            // 50000

            int intProductAltId1 = 0;
            int intProductAltId2 = 0;
            int intProductAltId3 = 0;

            int intProductAltId4 = 0;
            int intProductAltId5 = 0;
            int intProductAltId6 = 0;

            string strCategoryName1 = "";
            string strCategoryName2 = "";
            string strCategoryName3 = "";

            string strPropName1 = "";
            string strPropName2 = "";
            string strPropName3 = "";
            string strPropName4 = "";
            string strPropName5 = "";
            string strPropName6 = "";
            string strPropName7 = "";

            int intTempProductId = 0;

            //int i, j, k;

            var rnd = new Random();

            // ---------------

            ImportStatistic.TotalRow = (intRootCategories * intSubCategoriesLevel1 * intSubCategoriesLevel2 * intProductCountForCategory);

            // Step by rows

            for (int i = 0; i <= intRootCategories - 1; i++) // 15
            {
                strCategoryName1 = Strings.GetRandomString(rnd, 25, 6);

                if (!chbSimpleMode.Checked) // Simple Mode
                {
                    strPropName1 = Strings.GetRandomString(rnd, 4);
                    strPropName2 = Strings.GetRandomString(rnd, 7);
                    strPropName3 = Strings.GetRandomString(rnd, 7);
                    strPropName4 = Strings.GetRandomString(rnd, 8);
                    strPropName5 = Strings.GetRandomString(rnd, 8);
                    strPropName6 = Strings.GetRandomString(rnd, 9);
                    strPropName7 = Strings.GetRandomString(rnd, 9);
                }

                for (int j = 0; j <= intSubCategoriesLevel1 - 1; j++) // 15 x 10
                {

                    strCategoryName2 = Strings.GetRandomString(rnd, 25, 5);

                    for (int k = 0; k <= intSubCategoriesLevel2 - 1; k++) // 15 x 10 x 10
                    {

                        strCategoryName3 = Strings.GetRandomString(rnd, 25, 4);

                        for (int p = 0; p <= intProductCountForCategory - 1; p++) // 15 x 10 x 10 x 10
                        {

                            intCurrentProductIndex++;

                            intTempProductId = ProcessProduct(rnd, intCurrentProductIndex);

                            // Category ----------------------------------------------------------------------------------------------

                            string parentCategory = string.Format("[{0} >> {1} >> {2}]", strCategoryName1.Trim(), strCategoryName2.Trim(), strCategoryName3.Trim());
                            CategoryService.SubParseAndCreateCategory(parentCategory, intTempProductId);

                            if (!chbSimpleMode.Checked) // Simple Mode
                            {

                                // Property -----------------------------------------------------------------------------------------------

                                BuildProperties(rnd, intTempProductId, strPropName1, strPropName2, strPropName3, strPropName4, strPropName5, strPropName6, strPropName7);

                                // Images -------------------------------------------------------------------------------------------------

                                var tempId = PhotoService.AddPhoto(new Photo(0, intTempProductId, PhotoType.Product) { Description = Strings.GetRandomString(rnd, 25, 4) });
                                if (!string.IsNullOrWhiteSpace(tempId))
                                {
                                    CopyProductPic(rnd, tempId);
                                }

                                tempId = PhotoService.AddPhoto(new Photo(0, intTempProductId, PhotoType.Product) { Description = Strings.GetRandomString(rnd, 25, 4) });
                                if (!string.IsNullOrWhiteSpace(tempId))
                                {
                                    CopyProductPic(rnd, tempId);
                                }

                                tempId = PhotoService.AddPhoto(new Photo(0, intTempProductId, PhotoType.Product) { Description = Strings.GetRandomString(rnd, 25, 4) });
                                if (!string.IsNullOrWhiteSpace(tempId))
                                {
                                    CopyProductPic(rnd, tempId);
                                }


                                // Alt Products --------------------------------------------------------------------------------------------

                                if (intProductAltId1 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId1, RelatedType.Alternative);
                                }

                                if (intProductAltId2 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId2, RelatedType.Alternative);
                                }

                                if (intProductAltId3 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId3, RelatedType.Alternative);
                                }

                                if (intProductAltId4 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId4, RelatedType.Alternative);
                                }

                                if (intProductAltId5 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId5, RelatedType.Alternative);
                                }

                                if (intProductAltId6 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId6, RelatedType.Alternative);
                                }

                                // And RelatedProduct ---------------------------------------------

                                if (intProductAltId1 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId1, RelatedType.Related);
                                }

                                if (intProductAltId2 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId2, RelatedType.Related);
                                }

                                if (intProductAltId3 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId3, RelatedType.Related);
                                }

                                // And RelatedProduct

                                if (intProductAltId4 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId4, RelatedType.Related);
                                }

                                if (intProductAltId5 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId5, RelatedType.Related);
                                }

                                if (intProductAltId6 != 0)
                                {
                                    ProductService.AddRelatedProduct(intTempProductId, intProductAltId6, RelatedType.Related);
                                }

                                //intProductAltId1 = intProductAltId2;
                                //intProductAltId2 = intProductAltId3;
                                //intProductAltId3 = intTempProductId;

                                intProductAltId1 = intProductAltId2;
                                intProductAltId2 = intProductAltId3;
                                intProductAltId3 = intProductAltId4;

                                intProductAltId4 = intProductAltId5;
                                intProductAltId5 = intProductAltId6;
                                intProductAltId6 = intTempProductId;

                                // Custom Options --------------------------------------------------------------------------------------------

                                var copt = new CustomOption(true) { CustomOptionsId = -1, ProductId = intTempProductId };

                                // copt.InputType = CustomOptionInputType.RadioButton;
                                copt.Title = Demo.GetRandomCity();

                                var opt = new OptionItem { OptionId = -1, PriceBc = 0, SortOrder = 10 };
                                opt.Title = Demo.GetRandomName();

                                var opt2 = new OptionItem { OptionId = -1, PriceBc = DevTool.GetRandomInt(rnd, 0, 100), SortOrder = 20 };
                                opt2.Title = Demo.GetRandomLastName();

                                var opt3 = new OptionItem { OptionId = -1, PriceBc = DevTool.GetRandomInt(rnd, 0, 10), SortOrder = 30 };
                                opt3.Title = Demo.GetRandomName();

                                copt.Options = new List<OptionItem> { opt, opt2, opt3 };

                                CustomOptionsService.AddCustomOption(copt);

                            }

                        }

                    }

                }

            }

            CategoryService.RecalculateProductsCountManual();

            //TODO find where is this function!
            //ProductService.SumImportLog(Resource.Admin_ImportXLS_UpdoadingSuccessfullyCompleted,Resource.Admin_ImportXLS_UpdoadingCompletedWithErrors);


        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + " at xls");
            Debug.LogError(ex);
        }

        ImportStatistic.IsRun = false;
        ImportStatistic.ThreadImport.Abort();

    }


    private Int32 ProcessProduct(Random rnd, Int32 intProductIndex)
    {

        if (!ImportStatistic.IsRun) return 0;

        var product = new Product();

        // Simple prop

        product.ArtNo = intProductIndex.ToString();

        product.Name = Strings.GetRandomString(rnd, 35, 5);

        product.Enabled = true;

        product.Discount = DevTool.GetRandomInt(rnd, 0, 100);

        product.Weight = DevTool.GetRandomInt(rnd, 0, 1000);

        product.Size = GetSizeForBdFormat(string.Format("{0} x {1} x {2}",
                                            DevTool.GetRandomInt(rnd, 0, 100),
                                            DevTool.GetRandomInt(rnd, 0, 100),
                                            DevTool.GetRandomInt(rnd, 0, 100)));

        product.BriefDescription = Strings.GetRandomString(rnd, 250, 7);

        product.Description = Strings.GetRandomString(rnd, 2500, 7);

        // Offer

        var offr = new Offer
                       {
                           OfferListId = CatalogService.DefaultOfferListId,
                           Price = DevTool.GetRandomInt(rnd, 1000, 10000),
                           SupplyPrice = DevTool.GetRandomInt(rnd, 0, 3000),
                           ShippingPrice = DevTool.GetRandomInt(rnd, 0, 2000),
                           Amount = DevTool.GetRandomInt(rnd, 0, 500),
                           Unit = Strings.GetRandomString(rnd, 4)
                       };


        product.Offers = new List<Offer> { offr };

        // URL

        product.UrlPath = product.ArtNo;

        // Meta

        var metaResul = new AdvantShop.SEO.MetaInfo
        {
            Title = string.Format("#STORE_NAME# - {0} {1} ({2})",
                Strings.GetRandomString(rnd, 4),
                Strings.GetRandomString(rnd, 4),
                Strings.GetRandomString(rnd, 4)),
            MetaKeywords = string.Format("{0}, {1}, {2}, {3}, {4}",
                Strings.GetRandomString(rnd, 4),
                Strings.GetRandomString(rnd, 4),
                Strings.GetRandomString(rnd, 4),
                Strings.GetRandomString(rnd, 4),
                Strings.GetRandomString(rnd, 4)),
            MetaDescription = Strings.GetRandomString(rnd, 255, 7)
        };

        metaResul.ObjId = product.ProductId; // New Adv3.0 Fix

        product.Meta = metaResul;

        // Update / Insert Product

        UpdateInsertProduct(product);

        return product.ProductId;

    }

    private void BuildProperties(Random rnd, Int32 intProductID, string strPropName1, string strPropName2,
        string strPropName3, string strPropName4, string strPropName5, string strPropName6, string strPropName7)
    {

        // Prepare

        Int32 pID = intProductID;

        string prop0 = Demo.GetRandomCity();
        string prop1 = Demo.GetRandomEmail();
        string prop2 = Demo.GetRandomName();
        string prop3 = Demo.GetRandomPhone();
        string prop4 = DevTool.GetRandomInt(rnd, 0, 3).ToString();

        string prop5 = string.Format("{0}*{1}", DevTool.GetRandomInt(rnd, 0, 100).ToString(),
                                                DevTool.GetRandomInt(rnd, 0, 100).ToString());

        string prop6 = AdvantShop.DevTool.GetRandomInt(rnd, 0, 100).ToString();

        // Add all properties

        PropertyService.AddAndUpdateProductProperty(pID, strPropName1, prop0, 0, 0, true);
        PropertyService.AddAndUpdateProductProperty(pID, strPropName2, prop1, 10, 10, false);
        PropertyService.AddAndUpdateProductProperty(pID, strPropName3, prop2, 20, 20, false);
        PropertyService.AddAndUpdateProductProperty(pID, strPropName4, prop3, 30, 30, false);
        PropertyService.AddAndUpdateProductProperty(pID, strPropName5, prop4, 40, 40, false);
        PropertyService.AddAndUpdateProductProperty(pID, strPropName6, prop5, 50, 50, false);
        PropertyService.AddAndUpdateProductProperty(pID, strPropName7, prop6, 60, 60, false);

    }

    private void CopyProductPic(Random rnd, string fileID)
    {

        Int32 intPriductPicIndex = DevTool.GetRandomInt(rnd, 1, 6);

        //  string str = string.Format(MapPath(@"~\DevDemo\pic\") + "{0}_big.jpg", intPriductPicIndex);

        string strTargetFilaName = "";
        string strSourceFileName = "";

        // _big ----------------------------

        strTargetFilaName = string.Format(Server.MapPath(@"~\pictures\product\big\") + "{0}_big.jpg", fileID);
        strSourceFileName = string.Format(Server.MapPath(@"~\DevDemo\pic\") + "{0}_big.jpg", intPriductPicIndex);

        if (!System.IO.File.Exists(strTargetFilaName))
            System.IO.File.Copy(strSourceFileName, strTargetFilaName);

        // _middle ----------------------------

        strTargetFilaName = string.Format(Server.MapPath(@"~\pictures\product\middle\") + "{0}_middle.jpg", fileID);
        strSourceFileName = string.Format(Server.MapPath(@"~\DevDemo\pic\") + "{0}_middle.jpg", intPriductPicIndex);

        if (!System.IO.File.Exists(strTargetFilaName))
            System.IO.File.Copy(strSourceFileName, strTargetFilaName);

        // _small ----------------------------

        strTargetFilaName = string.Format(Server.MapPath(@"~\pictures\product\small\") + "{0}_small.jpg", fileID);
        strSourceFileName = string.Format(Server.MapPath(@"~\DevDemo\pic\") + "{0}_small.jpg", intPriductPicIndex);

        if (!System.IO.File.Exists(strTargetFilaName))
            System.IO.File.Copy(strSourceFileName, strTargetFilaName);

        // _xsmall ----------------------------

        strTargetFilaName = string.Format(Server.MapPath(@"~\pictures\product\xsmall\") + "{0}_xsmall.jpg", fileID);
        strSourceFileName = string.Format(Server.MapPath(@"~\DevDemo\pic\") + "{0}_xsmall.jpg", intPriductPicIndex);

        if (!System.IO.File.Exists(strTargetFilaName))
            System.IO.File.Copy(strSourceFileName, strTargetFilaName);


        // old implementation -----------------

        //System.IO.File.Copy(string.Format(Server.MapPath(@"~\DevDemo\pic\") + "{0}_big.jpg", intPriductPicIndex),
        //    string.Format(Server.MapPath(@"~\pictures\product\big\") + "{0}_big.jpg", fileID));

        //System.IO.File.Copy(string.Format(Server.MapPath(@"~\DevDemo\pic\") + "{0}_middle.jpg", intPriductPicIndex),
        //    string.Format(Server.MapPath(@"~\pictures\product\middle\") + "{0}_middle.jpg", fileID));

        //System.IO.File.Copy(string.Format(Server.MapPath(@"~\DevDemo\pic\") + "{0}_small.jpg", intPriductPicIndex),
        //    string.Format(Server.MapPath(@"~\pictures\product\small\") + "{0}_small.jpg", fileID));

        //System.IO.File.Copy(string.Format(Server.MapPath(@"~\DevDemo\pic\") + "{0}_xsmall.jpg", intPriductPicIndex),
        //    string.Format(Server.MapPath(@"~\pictures\product\xsmall\") + "{0}_xsmall.jpg", fileID));


    }

}