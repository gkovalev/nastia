//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.SaasData;
using AdvantShop.Statistic;
using NExcel;
using Resources;
using Yogesh.ExcelXml;

public partial class Admin_ImportXLS : Page
{
    private readonly string _filePath;
    private readonly string _fullPath;
    private bool useMultiThreadImport = false;

    public Admin_ImportXLS()
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

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            if (!FileUpload1.HasFile)
            {
                MsgErr(Resource.Admin_ImportXLS_ChooseFile);
                return;
            }

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

                FileUpload1.SaveAs(_fullPath);
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HaveExcel))
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        AdvantShop.Localization.Culture.InitializeCulture();
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ImportXLS_Title);

        if (!IsPostBack)
        {
            OutDiv.Visible = ImportStatistic.IsRun;
            linkCancel.Visible = ImportStatistic.IsRun;

            // Delete ImportLog data
            ExcelLog.DeleteLog();
        }
    }

    private void ProcessExcel()
    {
        var isNativeFormat = true;
        Workbook book = null;
        // Open native excel file format
        try
        {
            book = Workbook.getWorkbook(_fullPath);
        }
        catch (Exception)
        {
            isNativeFormat = false;
        }

        try
        {
            if (isNativeFormat)
            {
                var worksheet = book.getSheet(0);
                const int intRegHead = 1;

                if (worksheet.getColumn(1).Length > 0 && chboxDisableProducts.Checked)
                {
                    ProductService.DisableAllProducts();
                }
                ImportStatistic.TotalRow = worksheet.getColumn(1).Length - 1;
                // Step by rows
                for (int i = intRegHead; i <= worksheet.getColumn(1).Length - 1; i++)
                {
                    if (!ImportStatistic.IsRun) return;

                    var artNo = Convert.ToString(worksheet.getCell(0, i).Value);
                    var product = ProductService.GetProduct(artNo);

                    var name = Convert.ToString(worksheet.getCell(1, i).Value);
                    if (!string.IsNullOrEmpty(name))
                    {
                        product.Name = name;
                    }
                    else
                    {
                        LogInvalidData(name, 1, i);
                        continue;
                    }
                    string enabled = Convert.ToString(worksheet.getCell(4, i).Value);
                    product.Enabled = !string.IsNullOrEmpty(enabled) && enabled.Trim().Equals("+");

                    var discount = Convert.ToString(worksheet.getCell(9, i).Value);
                    try
                    {
                        product.Discount = (string.IsNullOrEmpty(discount)) ? 0 : (Convert.ToDecimal(discount));
                    }
                    catch (Exception)
                    {
                        LogInvalidData(discount, 9, i);
                        continue;
                    }

                    var weight = Convert.ToString(worksheet.getCell(11, i).Value);
                    try
                    {
                        product.Weight = (string.IsNullOrEmpty(weight)) ? 0 : (Convert.ToDecimal(weight));
                    }
                    catch (Exception)
                    {
                        LogInvalidData(weight, 11, i);
                        continue;
                    }

                    product.Size = GetSizeForBdFormat(Convert.ToString(worksheet.getCell(12, i).Value));

                    product.BriefDescription = Convert.ToString(worksheet.getCell(13, i).Value);
                    product.Description = Convert.ToString(worksheet.getCell(14, i).Value);

                    var offr = new Offer { OfferListId = CatalogService.DefaultOfferListId };

                    var price = Convert.ToString(worksheet.getCell(5, i).Value);
                    try
                    {
                        offr.Price = (string.IsNullOrEmpty(price)) ? 0 : Convert.ToDecimal(price);
                    }
                    catch (Exception)
                    {
                        LogInvalidData(price, 5, i);
                        continue;
                    }

                    var sypplyprice = Convert.ToString(worksheet.getCell(6, i).Value);
                    try
                    {
                        offr.SupplyPrice = (string.IsNullOrEmpty(sypplyprice)) ? 0 : Convert.ToDecimal(sypplyprice);
                    }
                    catch (Exception)
                    {
                        LogInvalidData(sypplyprice, 6, i);
                        continue;
                    }

                    var shippingPrice = Convert.ToString(worksheet.getCell(10, i).Value);
                    try
                    {
                        offr.ShippingPrice = (string.IsNullOrEmpty(shippingPrice)) ? 0 : (Convert.ToDecimal(shippingPrice));
                    }
                    catch (Exception)
                    {
                        LogInvalidData(shippingPrice, 10, i);
                        continue;
                    }

                    var amount = Convert.ToString(worksheet.getCell(7, i).Value);
                    try
                    {
                        offr.Amount = (string.IsNullOrEmpty(amount)) ? 0 : (Convert.ToInt32(amount));
                    }
                    catch (Exception)
                    {
                        LogInvalidData(amount, 7, i);
                        continue;
                    }


                    offr.Unit = Convert.ToString(worksheet.getCell(8, i).Value);

                    product.Offers = new List<Offer> { offr };

                    string rewurl = Convert.ToString(worksheet.getCell(2, i).Value);
                    //var synonym = (string.IsNullOrEmpty(rewurl))  ? product.ArtNo : rewurl; 
                    //product.Synonym = synonym;

                    // --- New Fix Code - XML
                    if (string.IsNullOrEmpty(rewurl)) // пустая строка
                    {
                        rewurl = product.ArtNo;
                    }
                    else
                    {
                        // Не пустая строка, Используется или нет этот синомим вообще ?
                        int productId = ProductService.GetProductId(product.ArtNo);
                        if (!UrlService.IsAvalibleUrl(productId, ParamType.Product, rewurl)) // RouteService.GetParamBySynonym(rewurl) != null
                        {
                            //TODO не компилится
                            // Ок, Используется, не у нашего ли товара? 
                            //UrlSynonym synonym = RouteService.GetUrlSynonymByParamValue(productId, ParamType.Product);
                            //if (synonym != null && synonym.Synonym != rewurl)
                            //{
                            //    // Нет не унашего. Значит он занят, чтобы небыло дубликата,
                            //    // ставим Артикул как синоним
                            //    rewurl = product.ArtNo;
                            //}
                        }
                    }
                    product.UrlPath = rewurl;


                    UpdateInsertProduct(product);


                    //if (string.IsNullOrEmpty(rewurl) || RouteService.GetParamBySynonym(rewurl) != null) rewurl = product.ArtNo;
                    //RouteService.UpdateParamSynonym(ParamType.Product, product.ProductId,
                    //RouteService.InsertUpdateParam(rewurl, ParamType.Product, product.ProductId.ToString());

                    var parentCategory = Convert.ToString(worksheet.getCell(3, i).Value);
                    if (!string.IsNullOrEmpty(parentCategory))
                    {
                        try
                        {
                            CategoryService.SubParseAndCreateCategory(parentCategory, product.ProductId);
                        }
                        catch (Exception)
                        {
                            LogInvalidData(parentCategory, 3, i);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                ExcelXmlWorkbook xmlbook = ExcelXmlWorkbook.Import(_fullPath);
                Worksheet xmlsheet = xmlbook[0];

                const int intRegHead = 1;

                if (xmlsheet.RowCount > 0 && chboxDisableProducts.Checked)
                {
                    ProductService.DisableAllProducts();
                }
                ImportStatistic.TotalRow = xmlsheet.RowCount - 1;
                // Step by rows

                for (int i = intRegHead; i <= xmlsheet.RowCount - 1; i++)
                {
                    if (!ImportStatistic.IsRun) return;

                    var artNo = Convert.ToString(xmlsheet[0, i].Value);
                    var product = ProductService.GetProduct(artNo);


                    var name = Convert.ToString(xmlsheet[1, i].Value);
                    if (!string.IsNullOrEmpty(name))
                    {
                        product.Name = name;
                    }
                    else
                    {
                        LogInvalidData(name, 1, i);
                        continue;
                    }

                    var enabled = Convert.ToString(xmlsheet[4, i].Value);
                    product.Enabled = !string.IsNullOrEmpty(enabled) && enabled.Trim().Equals("+");

                    var discount = Convert.ToString(xmlsheet[9, i].Value);
                    try
                    {
                        product.Discount = (string.IsNullOrEmpty(discount)) ? 0 : (Convert.ToDecimal(discount));
                    }
                    catch (Exception)
                    {
                        LogInvalidData(discount, 9, i);
                        continue;
                    }

                    var weight = Convert.ToString(xmlsheet[11, i].Value);
                    try
                    {
                        product.Weight = (string.IsNullOrEmpty(weight)) ? 0 : Convert.ToDecimal(weight);
                    }
                    catch (Exception)
                    {
                        LogInvalidData(weight, 11, i);
                        continue;
                    }

                    product.Size = GetSizeForBdFormat(Convert.ToString(xmlsheet[12, i].Value));
                    product.BriefDescription = Convert.ToString(xmlsheet[13, i].Value);
                    product.Description = Convert.ToString(xmlsheet[14, i].Value);

                    var offr = new Offer { OfferListId = CatalogService.DefaultOfferListId };

                    var price = Convert.ToString(xmlsheet[5, i].Value);
                    try
                    {
                        offr.Price = (string.IsNullOrEmpty(price)) ? 0 : Convert.ToDecimal(price);
                    }
                    catch (Exception)
                    {
                        LogInvalidData(price, 5, i);
                        continue;
                    }

                    var supplyPrice = Convert.ToString(xmlsheet[6, i].Value);
                    try
                    {
                        offr.SupplyPrice = (string.IsNullOrEmpty(supplyPrice)) ? 0 : Convert.ToDecimal(supplyPrice);
                    }
                    catch (Exception)
                    {
                        LogInvalidData(supplyPrice, 6, i);
                        continue;
                    }

                    var shippingPrice = Convert.ToString(xmlsheet[10, i].Value);
                    try
                    {
                        offr.ShippingPrice = (string.IsNullOrEmpty(shippingPrice)) ? 0 : (Convert.ToDecimal(shippingPrice));
                    }
                    catch (Exception)
                    {
                        LogInvalidData(shippingPrice, 10, i);
                        continue;
                    }

                    var amount = Convert.ToString(xmlsheet[7, i].Value);
                    try
                    {
                        offr.Amount = (string.IsNullOrEmpty(amount)) ? 0 : Convert.ToInt32(amount);
                    }
                    catch (Exception)
                    {
                        LogInvalidData(amount, 7, i);
                        continue;
                    }

                    offr.Unit = Convert.ToString(xmlsheet[8, i].Value);

                    product.Offers = new List<Offer> { offr };
                    string rewurl = Convert.ToString(xmlsheet[2, i].Value);
                    //string synonym = (string.IsNullOrEmpty(rewurl)) ? product.ArtNo : rewurl;
                    //product.Synonym = synonym;

                    // Todo
                    // --- New Fix Code - XML
                    if (string.IsNullOrEmpty(rewurl)) // пустая строка
                    {
                        rewurl = product.ArtNo;
                    }
                    else
                    {
                        // Не пустая строка, Используется или нет этот синомим вообще ?
                        int productId = ProductService.GetProductId(product.ArtNo);
                        if (!UrlService.IsAvalibleUrl(productId, ParamType.Product, rewurl)) // RouteService.GetParamBySynonym(rewurl) != null
                        {
                            //TODO не компилится
                            // Ок, Используется, не у нашего ли товара?                             
                            //UrlSynonym synonym = RouteService.GetUrlSynonymByParamValue(productId, ParamType.Product);
                            //if (synonym != null && synonym.Synonym != rewurl)
                            //{
                            //    // Нет не унашего. Значит он занят, чтобы небыло дубликата,
                            //    // ставим Артикул как синоним
                            //    rewurl = product.ArtNo;
                            //}
                        }
                    }
                    product.UrlPath = rewurl;

                    UpdateInsertProduct(product);

                    //RouteService.UpdateParamSynonym(ParamType.Product, product.ProductId, synonym);
                    //if (string.IsNullOrEmpty(rewurl) || RouteService.GetParamBySynonym(rewurl) != null) rewurl = product.ArtNo;

                    //RouteService.InsertUpdateParam(rewurl, ParamType.Product, product.ProductId.ToString());

                    var parentCategory = Convert.ToString(xmlsheet[3, i].Value);
                    if (string.IsNullOrEmpty(parentCategory))
                    {
                        continue;
                    }
                    try
                    {
                        CategoryService.SubParseAndCreateCategory(parentCategory, product.ProductId);
                    }
                    catch
                    {
                        LogInvalidData(parentCategory, 3, i);
                        //TODO: vk: тут надо бы вывести сообщение о том, что продукт уже есть в категории. или изменить try/catch блок на проверку существования продукта в категории
                        //наличие exception`ов здесь - нормально
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
        //ImportStatistic.ThreadImport.Abort();
    }

    //public static void SubParseAndCreateCategory(string strCategory, string artNo)
    //{
    //    //
    //    // strCategory "[Техника >> Игровые приставки >> PlayStation 3];[....]"
    //    //
    //    foreach (string strT in strCategory.Split(new[] { ';' }))
    //    {
    //        var st = strT;
    //        st = st.Replace("[", "");
    //        st = st.Replace("]", "");
    //        int parentId = 0;
    //        string[] temp = st.Split(new[] { ">>" }, StringSplitOptions.RemoveEmptyEntries);

    //        for (int i = 0; i <= temp.Length - 1; i++)
    //        {
    //            string str = temp[i].Trim();
    //            if (!string.IsNullOrEmpty(str))
    //            {
    //                var cat = CategoryService.GetChildCategoryIDByName(parentId, str);
    //                if (cat != 0)
    //                    parentId = cat;
    //                else
    //                {
    //                    parentId = CategoryService.AddCategory(new Category
    //                    {
    //                        Name = str,
    //                        ParentCategoryId = parentId,
    //                        Picture = string.Empty,
    //                        SortOrder = 0,
    //                        Enabled = true
    //                    }, false);
    //                }
    //            }
    //            if (i == temp.Length - 1)
    //            {
    //                var tempProductId = ProductService.GetProductIdByArtNo(artNo);
    //                if (tempProductId != -1)
    //                    ProductService.AddProductLink(tempProductId, parentId);
    //            }
    //        }
    //    }
    //}

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (ImportStatistic.ThreadImport.IsAlive)
        {
            //ImportStatistic.ThreadImport.Abort();
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
}