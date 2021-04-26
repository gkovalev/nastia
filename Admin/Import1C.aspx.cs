//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.FilePath;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using Resources;
using AdvantShop.SaasData;
using AdvantShop.Core.UrlRewriter;

public partial class Admin_Import1C : Page
{
    protected enum ImportLogMessageType
    {
        ProductAdded,
        ProductUpdated,
        InvalidData,
        SuccessImport,
        ImportedWithErrors
    }

    public static float Summary;
    public static float Done;
    public static float Added;
    public static float Updated;
    public static float Errors;
    private readonly string _filePath;
    private readonly string _fullPath;
    private readonly string _xmlFile;
    private HttpContext _current;

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

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.Have1C))
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Import1C_Title);

        if (!IsPostBack)
        {
            sqlDataSourceLog.Delete();
        }
    }

    public Admin_Import1C()
    {
        _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
        _fullPath = string.Format("{0}{1}", _filePath, "upload.zip");
        _xmlFile = string.Format("{0}{1}", _filePath, "xml_export.xml");
    }

    public void UpdateInsertProduct(Product product, string categoryWay)
    {
        if (!string.IsNullOrEmpty(product.ArtNo))
        {
            var p = ProductService.GetProduct(product.ArtNo);
            if (p.ID == 0)
            {
                ProductService.AddProduct(product, false);

                //ProductService.AddProductLink(product.ProductId, categoryId);
                CategoryService.SubParseAndCreateCategory(categoryWay, product.ProductId);

                Log(string.Format(Resource.Admin_Import1C_Added, product.Name, product.ArtNo),
                    ImportLogMessageType.ProductAdded);
            }
            else
            {
                product.ProductId = p.ProductId;
                ProductService.UpdateProductByArtNo(product, false);
                //ProductService.AddProductLink(product.ProductId, categoryId);
                CategoryService.SubParseAndCreateCategory(categoryWay, product.ProductId);

                Log(string.Format(Resource.Admin_Import1C_Updated, product.Name, product.ArtNo),
                    ImportLogMessageType.ProductUpdated);
            }
        }
    }

    protected void Log(string message, ImportLogMessageType type)
    {
        sqlDataSourceLog.InsertParameters["mtype"].DefaultValue = type.ToString("d");
        sqlDataSourceLog.InsertParameters["message"].DefaultValue = message;
        sqlDataSourceLog.Insert();

        if (type == ImportLogMessageType.ProductAdded)
        {
            Done++;
            Added++;
        }
        if (type == ImportLogMessageType.ProductUpdated)
        {
            Updated++;
            Done++;
        }
        if (type == ImportLogMessageType.InvalidData)
        {
            Errors++;
            MsgErr(message);
        }
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {

        try
        {
            MsgErr(true);
            lblRes.Text = string.Empty;
            if (!FileUpload1.HasFile)
            {
                MsgErr(Resource.Admin_Import1C_ChooseFile);
                return;
            }
            try
            {
                FileHelpers.CreateDirectory(_filePath);
                //if (!Directory.Exists(_filePath))
                //{
                //    Directory.CreateDirectory(_filePath);
                //}
                FileHelpers.DeleteFile(_fullPath);
                //if (Directory.Exists(_fullPath))
                //{
                //    Directory.Delete(_fullPath);
                //}
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " at Files");
                return;
            }
            FileUpload1.SaveAs(_fullPath);
            FileHelpers.UnZipFile(_fullPath);

            Added = 0;
            Updated = 0;
            Errors = 0;
            _current = HttpContext.Current;
            var process = new Thread(ProcessXml);
            process.Start();
            //ProcessXml()
            pUploadXml.Visible = false;

            timer.Enabled = true;
            progressBar.Visible = true;

            lProgress.Visible = true;
            lDone.Visible = true;
            lSummary.Visible = true;
            pSummary.Visible = true;
        }
        catch (Exception ex)
        {
            AdvantShop.Diagnostics.Debug.LogError(ex);
        }
    }

    private void ProcessXml()
    {
        var doc = new XmlDocument();
        try
        {
            doc.Load(_xmlFile);
        }
        catch (XmlException e)
        {
            MsgErr(string.Format(Resource.Admin_Import1C_XmlError, e.LineNumber, e.LinePosition));
            return;
        }
        Summary = doc.GetElementsByTagName("Product").Count;
        lSummary.Text = Summary.ToString();
        UpdatePanel1.Update();
        HttpContext.Current = _current;
        try
        {
            Done = 0;
            var cats = new Dictionary<string, string>();
            //XmlNodeList categories = doc.GetElementsByTagName("Category");
            //if (categories.Count != 0)
            //{
            //    foreach (XmlNode categoryXml in categories)
            //    {
            //        Category category = CategoryService.GetCategoryFromDbByCategoryId(Convert.ToInt32(categoryXml.Attributes["ID"].InnerText));
            //        if (category == null)
            //        {
            //            try
            //            {
            //                category = new Category()
            //                               {
            //                                   Name = categoryXml.Attributes["Name"].InnerText,
            //                                   ParentCategoryId = Convert.ToInt32(categoryXml.Attributes["ParentCategory"].InnerText)
            //                               };
            //                int catID = CategoryService.AddCategory(category, true);
            //                cats.Add(categoryXml.Attributes["ID"].InnerText, catID.ToString());
            //            }
            //            catch (Exception)
            //            {
            //            }
            //        }
            //        else
            //        {
            //            category.Name = categoryXml.Attributes["Name"].InnerText;
            //            category.ParentCategoryId = Convert.ToInt32(categoryXml.Attributes["ParentCategory"].InnerText);
            //            CategoryService.UpdateCategory(category, false);
            //            cats.Add(category.CategoryId.ToString(), category.CategoryId.ToString());
            //        }
            //    }
            //}
            if (doc.GetElementsByTagName("Products").Count != 0)
            {
                var products = new Dictionary<string, Product>();
                var productCats = new Dictionary<string, string>();
                var productUnits = new Dictionary<string, string>();
                var productsXml = doc.GetElementsByTagName("Products")[0];
                if (productsXml == null) throw new NotImplementedException();
                if (productsXml.HasChildNodes && chboxDisableProducts.Checked)
                {
                    ProductService.DisableAllProducts();
                }
                foreach (XmlNode prodXml in productsXml.ChildNodes)
                {
                    var product = new Product
                                      {
                                          ArtNo = prodXml.Attributes["SKU"].InnerText,
                                          Name = prodXml.Attributes["Name"].InnerText
                                      };
                    productUnits.Add(product.ArtNo, prodXml.Attributes["Unit"].InnerText);
                    product.Description = prodXml.Attributes["Description"].InnerText;
                    //CategoryService.SubParseAndCreateCategory(prodXml.Attributes["Category"].InnerText, product.ProductId);
                    productCats.Add(product.ArtNo, prodXml.Attributes["Category"].InnerText);
                    product.Enabled = true;
                    try
                    {
                        products.Add(product.ArtNo, product);
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message, ImportLogMessageType.InvalidData);
                    }
                }
                //todo eVo write new logic for offers
                var offers = doc.GetElementsByTagName("Offer");
                if (offers.Count != 0)
                {
                    foreach (XmlNode offer in offers)
                    {
                        Product product = products[offer.Attributes["ProductSKU"].InnerText];
                        if (product == null)
                        {
                            break;
                        }
                        var pOffer = new Offer();
                        decimal price;
                        if (decimal.TryParse(offer.Attributes["Price"].Value.Replace('.', ','), out price))
                        {
                            pOffer.Price = price;
                        }
                        int amount;
                        if (int.TryParse(offer.Attributes["Amount"].InnerText, out amount))
                        {
                            pOffer.Amount = amount;
                        }
                        pOffer.Unit = productUnits[product.ArtNo];
                        pOffer.OfferListId = CatalogService.DefaultOfferListId;
                        if (product.Offers == null)
                        {
                            product.Offers = new List<Offer>();
                        }

                        product.Offers.Add(pOffer);
                    }
                }

                foreach (var product in products.Values)
                {
                    UpdateInsertProduct(product, productCats[product.ArtNo]);
                }

                LuceneSearch.CreateAllIndexInBackground();

                var photos = doc.GetElementsByTagName("Photo");
                if (photos.Count != 0)
                {
                    FileHelpers.UpdateDirectories();
                    var flagIsFrirst = true;
                    foreach (XmlNode photo in photos)
                    {
                        var fullname = _filePath + photo.Attributes["FileName"].Value;
                        if (File.Exists(fullname))
                        {
                            ProductService.AddProductPhotoByArtNo(photo.Attributes["ProductSKU"].Value, fullname, photo.Attributes["Description"].Value, flagIsFrirst);
                            flagIsFrirst = false;
                        }

                        File.Delete(_filePath + photo.Attributes["FileName"].Value);
                    }
                }
                XmlNodeList props = doc.GetElementsByTagName("Property");
                if (props.Count != 0)
                {
                    foreach (XmlNode prop in props)
                    {
                        var product = ProductService.GetProduct(prop.Attributes["ProductSKU"].InnerText);
                        if (product.ID == 0)
                        {
                            break;
                        }
                        PropertyService.AddProductProperty(product.ProductId, prop.Attributes["Name"].Value, prop.Attributes["Value"].Value, 0, 0, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log(ex.Message, ImportLogMessageType.InvalidData);
        }

        Log("Import successfull", ImportLogMessageType.SuccessImport);

        CategoryService.RecalculateProductsCountManual();
        HttpContext.Current = null;
        //ProductService.SumImportLog(Resource.Admin_Import1C_UpdoadingSuccessfullyCompleted, Resource.Admin_Import1C_UpdoadingCompletedWithErrors);
    }

    protected void timer_Tick(object sender, EventArgs e)
    {
        sqlDataSourceLog.DataBind();

        if (Summary == 0) return;

        lProgress.Text = (Done / Summary * 100) + "%";
        lSummary.Text = Summary.ToString();
        lDone.Text = Done.ToString();
        lAdded.Text = Added.ToString();
        lUpdated.Text = Updated.ToString();
        lErrors.Text = Errors.ToString();
    }

    protected void rLog_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (IsPostBack)
        {
            if (((ImportLogMessageType)((DataRowView)e.Item.DataItem)["mtype"]) == ImportLogMessageType.SuccessImport ||
                ((ImportLogMessageType)((DataRowView)e.Item.DataItem)["mtype"]) ==
                ImportLogMessageType.ImportedWithErrors)
            {
                timer.Enabled = false;
                hlDownloadImportLog.Visible = true;
                lblRes.Visible = true;
            }
        }
    }

    private void LogInvalidData(string value, int column, int row)
    {
        sqlDataSourceLog.InsertParameters["mtype"].DefaultValue = ImportLogMessageType.InvalidData.ToString("d");
        sqlDataSourceLog.InsertParameters["message"].DefaultValue = value == (null) ? string.Format("Value at cell [{0}; {1}] ([column, row]) cannot be empty", column + 1, row + 1) :
            string.Format("Invalid value ({0}) at cell [{1}; {2}] ([column, row])", value, column + 1, row + 1);
        sqlDataSourceLog.Insert();
    }

    //protected void AddPhoto_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        MsgErr(true);
    //        lblRes.Text = string.Empty;
    //        if (!FileUpload2.HasFile)
    //        {
    //            MsgErr(Resource.Admin_Import1C_ChooseFile);
    //            return;
    //        }
    //        try
    //        {
    //            if (!Directory.Exists(_filePath))
    //            {
    //                Directory.CreateDirectory(_filePath);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MsgErr(ex.Message + " at Files");
    //            return;
    //        }
    //        FileUpload2.SaveAs(_filePath + FileUpload2.FileName);
    //        sqlDataSourceLog.Delete();
    //        Log(Resource.Admin_Import1C_AddedPhoto + " " + FileUpload2.FileName, ImportLogMessageType.ProductAdded);
    //    }
    //    catch (Exception ex)
    //    {
    //        AdvantShop.Diagnostics.Debug.LogError(ex);
    //    }
    //}

    protected void sqlDataSourceLog_Init(object sender, EventArgs e)
    {
        sqlDataSourceLog.ConnectionString = AdvantShop.Connection.GetConnectionString();
    }
}