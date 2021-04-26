//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Helpers.CsvHelper;
using AdvantShop.Statistic;
using Resources;
using AdvantShop.SaasData;

public partial class Admin_ExportCsv : Page
{
    private readonly string _strFilePath;
    private readonly string _strFullPath;
    public string NotDoPost = string.Empty;
    private Separators.SeparatorsEnum _separator;
    private Encodings.EncodingsEnum _encoding;
    protected List<ProductFields.Fields> FieldMapping = new List<ProductFields.Fields>();
    private const string StrFileName = "products.csv";

    public Admin_ExportCsv()
    {
        _strFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
        _strFullPath = string.Format("{0}{1}", _strFilePath, StrFileName);
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        CommonHelper.DisableBrowserCache();
        choseDiv.Visible = !ExportStatistic.IsRun;
        divAction.Visible = !ExportStatistic.IsRun;
        divbtnAction.Visible = !ExportStatistic.IsRun;

        LoadFirstProduct();
        var tbl = new Table() { ID = "tblValues" };
        var ddlRow = new TableRow { ID = "ddlRow" };
        var lblRow = new TableRow { ID = "lblRow", BackColor = ColorTranslator.FromHtml("#EFF0F2") };
        var cellM = new TableCell { ID = "cellM" };
        cellM.Attributes.Add("style", "vertical-align:top; width:150px");
        cellM.Controls.Add(new Label { Text = Resources.Resource.Admin_ExportCsv_Column });
        ddlRow.Cells.Add(cellM);

        var cellL = new TableCell { ID = "cellL" };
        cellL.Attributes.Add("style", "vertical-align:top; width:150px");
        cellL.Controls.Add(new Label { Text = Resources.Resource.Admin_ExportCsv_SampleOfData });
        var div4 = new Panel { Width = 110 };
        cellL.Controls.Add(div4);
        lblRow.Cells.Add(cellL);

        foreach (var item in Enum.GetValues(typeof(ProductFields.Fields)))
        {
            // none and photo in export by default no need
            if ((ProductFields.Fields)item == ProductFields.Fields.None) continue;
            var cell = new TableCell { ID = "cell" + ((int)item).ToString() };
            cell.Attributes.Add("style", "vertical-align:top");
            var ddl = new DropDownList { ID = "ddlType" + ((int)item).ToString(), Width = 150 };
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.None), ((int)ProductFields.Fields.None).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Sku), ((int)ProductFields.Fields.Sku).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Name), ((int)ProductFields.Fields.Name).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.ParamSynonym), ((int)ProductFields.Fields.ParamSynonym).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Category), ((int)ProductFields.Fields.Category).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Enabled), ((int)ProductFields.Fields.Enabled).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Price), ((int)ProductFields.Fields.Price).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.PurchasePrice), ((int)ProductFields.Fields.PurchasePrice).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Amount), ((int)ProductFields.Fields.Amount).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Unit), ((int)ProductFields.Fields.Unit).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Discount), ((int)ProductFields.Fields.Discount).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.ShippingPrice), ((int)ProductFields.Fields.ShippingPrice).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Weight), ((int)ProductFields.Fields.Weight).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Size), ((int)ProductFields.Fields.Size).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.BriefDescription), ((int)ProductFields.Fields.BriefDescription).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Description), ((int)ProductFields.Fields.Description).ToString()));

            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Title), ((int)ProductFields.Fields.Title).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.MetaKeywords), ((int)ProductFields.Fields.MetaKeywords).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.MetaDescription), ((int)ProductFields.Fields.MetaDescription).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Photos), ((int)ProductFields.Fields.Photos).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Markers), ((int)ProductFields.Fields.Markers).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Properties), ((int)ProductFields.Fields.Properties).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Producer), ((int)ProductFields.Fields.Producer).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.OrderByRequest), ((int)ProductFields.Fields.OrderByRequest).ToString()));

            //Added By Evgeni
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.EAN), ((int)ProductFields.Fields.EAN).ToString()));
            ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.SubBrandId), ((int)ProductFields.Fields.SubBrandId).ToString()));
            //

            if (!string.IsNullOrEmpty(Request["state"]) && Request["state"].ToLower() == "select")
                ddl.SelectedValue = ((int)item).ToString();
            var lb = new Label { ID = "lbProduct" + ((int)item).ToString(), Text = ddlProduct.Items.Count > 0 && Request["state"] == "select" ? ddlProduct.Items[(int)item].Text : string.Empty };
            lb.Attributes.Add("style", "display:block");
            ddl.Attributes.Add("onchange", string.Format("Change(this)"));
            cell.Controls.Add(ddl);
            //cell.Controls.Add(lb);
            ddlRow.Cells.Add(cell);
            var cellLbl = new TableCell { ID = "cellLbl" + ((int)item).ToString() };
            cellLbl.Controls.Add(lb);
            lblRow.Cells.Add(cellLbl);
        }

        tbl.Rows.Add(ddlRow);
        tbl.Rows.Add(lblRow);
        choseDiv.Controls.Add(tbl);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExcel)
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        hrefAgaint.Visible = false;
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportExcel_Title);
        //ddlSelectFields.SelectedValue = Request["state"];
        if (!IsPostBack)
        {

            ddlEncoding.Items.Clear();
            ddlEncoding.Items.Add(new ListItem
                                      {
                                          Text = Encodings.EncodingsEnum.Windows1251.ToString(),
                                          Value = ((int)Encodings.EncodingsEnum.Windows1251).ToString(),
                                          Selected = true
                                      });
            ddlEncoding.Items.Add(new ListItem
                                      {
                                          Text = Encodings.EncodingsEnum.Utf8.ToString(),
                                          Value = ((int)Encodings.EncodingsEnum.Utf8).ToString()
                                      });
            ddlEncoding.Items.Add(new ListItem
                                      {
                                          Text = Encodings.EncodingsEnum.Utf16.ToString(),
                                          Value = ((int)Encodings.EncodingsEnum.Utf16).ToString()
                                      });
            ddlEncoding.Items.Add(new ListItem
                                      {
                                          Text = Encodings.EncodingsEnum.Koi8R.ToString(),
                                          Value = ((int)Encodings.EncodingsEnum.Koi8R).ToString()
                                      });

            ddlSeparetors.Items.Clear();
            ddlSeparetors.Items.Add(new ListItem
                                        {
                                            Text = Resource.Admin_ImportCsv_Semicolon,
                                            Value = ((int)Separators.SeparatorsEnum.SemicolonSeparated).ToString(),
                                            Selected = true
                                        });
            ddlSeparetors.Items.Add(new ListItem
                                        {
                                            Text = Resource.Admin_ImportCsv_Comma,
                                            Value = ((int)Separators.SeparatorsEnum.CommaSeparated).ToString()
                                        });
            ddlSeparetors.Items.Add(new ListItem
                                        {
                                            Text = Resource.Admin_ImportCsv_Tab,
                                            Value = ((int)Separators.SeparatorsEnum.TabSeparated).ToString()
                                        });
        }
        //LoadFirstProduct();
        if (choseDiv.FindControl("tblValues") != null && IsPostBack)
        {
            var cells = ((TableRow)choseDiv.FindControl("ddlRow")).Cells;
            foreach (TableCell item in cells)
            {
                var element = item.Controls.OfType<DropDownList>().FirstOrDefault();
                if (element == null) continue;

                if (item.Controls.OfType<DropDownList>().First().SelectedValue != ((int)ProductFields.Fields.None).ToString())
                {
                    if (!FieldMapping.Contains((ProductFields.Fields)Convert.ToInt32(item.Controls.OfType<DropDownList>().First().SelectedValue)))
                        FieldMapping.Add((ProductFields.Fields)Convert.ToInt32((item.Controls.OfType<DropDownList>().First().SelectedValue)));//, cells.GetCellIndex(item));
                    else
                    {
                        MsgErr(string.Format(Resource.Admin_ImportCsv_DuplicateMessage, item.Controls.OfType<DropDownList>().First().SelectedItem.Text));
                        return;
                    }
                }
            }
        }
        if (FieldMapping.Count == 0 && IsPostBack)
        {
            MsgErr(Resource.Admin_ExportCsv_ListEmpty);
            return;
        }
        MsgErr(true);
        OutDiv.Visible = ExportStatistic.IsRun;
        linkCancel.Visible = ExportStatistic.IsRun;
    }

    private void LoadFirstProduct()
    {
        int productId = SQLDataAccess.ExecuteScalar<int>("Select TOP(1) productId from Catalog.ProductCategories", CommandType.Text);
        var product = ProductService.GetProduct(productId);
        if (product == null) return;
        foreach (var item in Enum.GetValues(typeof(ProductFields.Fields)))
        {
            if ((ProductFields.Fields)item == ProductFields.Fields.None)
                ddlProduct.Items.Add(new ListItem { Text = @"-", Value = ((int)ProductFields.Fields.None).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Sku)
                ddlProduct.Items.Add(new ListItem { Text = product.ArtNo, Value = ((int)ProductFields.Fields.Sku).ToString() });
            //Added By Evgeni
            if ((ProductFields.Fields)item == ProductFields.Fields.EAN)
                ddlProduct.Items.Add(new ListItem { Text = product.EAN, Value = ((int)ProductFields.Fields.EAN).ToString() });
            if ((ProductFields.Fields)item == ProductFields.Fields.SubBrandId)
                ddlProduct.Items.Add(new ListItem { Text = product.SubBrandId.ToString(), Value = ((int)ProductFields.Fields.SubBrandId).ToString() });
//

            if ((ProductFields.Fields)item == ProductFields.Fields.Name)
                ddlProduct.Items.Add(new ListItem { Text = product.Name.HtmlEncode(), Value = ((int)ProductFields.Fields.Name).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.ParamSynonym)
                ddlProduct.Items.Add(new ListItem { Text = product.UrlPath, Value = ((int)ProductFields.Fields.ParamSynonym).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Category)
                ddlProduct.Items.Add(new ListItem { Text = CategoryService.GetCategory(product.CategoryID).Name, Value = ((int)ProductFields.Fields.Category).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Enabled)
                ddlProduct.Items.Add(new ListItem { Text = product.Enabled ? "+" : "-", Value = ((int)ProductFields.Fields.Enabled).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Price)
                ddlProduct.Items.Add(new ListItem { Text = product.Offers[0].Price.ToString("F2"), Value = ((int)ProductFields.Fields.Price).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.PurchasePrice)
                ddlProduct.Items.Add(new ListItem { Text = product.Offers[0].SupplyPrice.ToString("F2"), Value = ((int)ProductFields.Fields.PurchasePrice).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Amount)
                ddlProduct.Items.Add(new ListItem { Text = product.Offers[0].Amount.ToString(), Value = ((int)ProductFields.Fields.Amount).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Unit)
                ddlProduct.Items.Add(new ListItem { Text = product.Offers[0].Unit, Value = ((int)ProductFields.Fields.Unit).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Discount)
                ddlProduct.Items.Add(new ListItem { Text = product.Discount.ToString("F2"), Value = ((int)ProductFields.Fields.Discount).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.ShippingPrice)
                ddlProduct.Items.Add(new ListItem { Text = product.Offers[0].ShippingPrice.ToString("F2"), Value = ((int)ProductFields.Fields.ShippingPrice).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Weight)
                ddlProduct.Items.Add(new ListItem { Text = product.Weight.ToString("F2"), Value = ((int)ProductFields.Fields.Weight).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Size)
                ddlProduct.Items.Add(new ListItem { Text = product.Size.Replace("|", " x "), Value = ((int)ProductFields.Fields.Size).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.BriefDescription)
                ddlProduct.Items.Add(new ListItem { Text = product.BriefDescription.Reduce(20).HtmlEncode(), Value = ((int)ProductFields.Fields.BriefDescription).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Description)
                ddlProduct.Items.Add(new ListItem { Text = product.Description.Reduce(20).HtmlEncode(), Value = ((int)ProductFields.Fields.Description).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Title)
                ddlProduct.Items.Add(new ListItem { Text = product.Meta.Title.Reduce(20), Value = ((int)ProductFields.Fields.Title).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.MetaKeywords)
                ddlProduct.Items.Add(new ListItem { Text = product.Meta.MetaKeywords.Reduce(20), Value = ((int)ProductFields.Fields.MetaKeywords).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.MetaDescription)
                ddlProduct.Items.Add(new ListItem { Text = product.Meta.MetaDescription.Reduce(20), Value = ((int)ProductFields.Fields.MetaDescription).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Photos)
                ddlProduct.Items.Add(new ListItem { Text = CsvExport.PhotoToString(product.ProductPhotos), Value = ((int)ProductFields.Fields.Photos).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Markers)
                ddlProduct.Items.Add(new ListItem { Text = CsvExport.MarkersToString(product), Value = ((int)ProductFields.Fields.Markers).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Properties)
                ddlProduct.Items.Add(new ListItem { Text = CsvExport.PropertiesToString(product.ProductPropertyValues).HtmlEncode(), Value = ((int)ProductFields.Fields.Properties).ToString() });

            if ((ProductFields.Fields)item == ProductFields.Fields.Producer)
            {
                var brand = BrandService.GetBrandById(product.ProductId);
                ddlProduct.Items.Add(new ListItem
                                         {
                                             Text = brand != null ? BrandService.GetBrandById(product.ProductId).Name.HtmlEncode() : string.Empty,
                                             Value = ((int)ProductFields.Fields.Producer).ToString()
                                         });
            }
            if ((ProductFields.Fields)item == ProductFields.Fields.OrderByRequest)
                ddlProduct.Items.Add(new ListItem { Text = product.OrderByRequest ? "+" : "-", Value = ((int)ProductFields.Fields.OrderByRequest).ToString() });
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        if (lError.Visible) return;

        if (ExportStatistic.IsRun == false)
        {
            divAction.Visible = false;
            divbtnAction.Visible = false;
            choseDiv.Visible = false;
            _separator = (Separators.SeparatorsEnum)Convert.ToInt32(ddlSeparetors.SelectedValue);
            _encoding = (Encodings.EncodingsEnum)Convert.ToInt32(ddlEncoding.SelectedValue);

            ExportStatistic.Init();
            ExportStatistic.IsRun = true;
            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnDownload.Visible = false;
            try
            {
                // Directory
                FileHelpers.CreateDirectory(_strFilePath);
                ExportStatistic.TotalRow = ProductService.GetProductsCount();
                var tr = new Thread(Save);
                ExportStatistic.ThreadImport = tr;
                tr.IsBackground = true;
                tr.Start();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                MsgErr(ex.Message);
            }
        }
    }

    protected void Save()
    {
        CsvExport.SaveProductsToCsv(_strFullPath, _encoding, _separator, FieldMapping);
        ExportStatistic.IsRun = false;
    }

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (!ExportStatistic.ThreadImport.IsAlive) return;
        ExportStatistic.IsRun = false;
        ExportStatistic.Init();
        hrefAgaint.Visible = true;
        linkCancel.Visible = false;
    }

    protected void btnAsyncLoad_Click(object sender, EventArgs e)
    {
        linkCancel.Visible = false;
        NotDoPost = "true";
        try
        {
            using (FileStream f = File.Open(_strFullPath, FileMode.Open))
            {
                f.Close();
            }
        }
        catch (Exception ex)
        {
            //Debug.LogError(ex, sender, e);
            Debug.LogError(ex);
            MsgErr(ex.Message);
        }
        hrefAgaint.Visible = true;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        divSomeMessage.Visible = !hrefAgaint.Visible;

        if (ExportStatistic.IsRun)
        {
            ltLink.Text = string.Empty;
            return;
        }
        if (File.Exists(_strFullPath))
        {
            var f = new FileInfo(_strFullPath);
            const double size = 0;
            double sizeM = (double)f.Length / 1048576; //1024 * 1024

            string sizeMesage;
            if ((int)sizeM > 0)
            {
                sizeMesage = ((int)sizeM) + " MB";
            }
            else
            {
                double sizeK = (double)f.Length / 1024;
                if ((int)sizeK > 0)
                {
                    sizeMesage = ((int)sizeK) + " KB";
                }
                else
                {
                    sizeMesage = ((int)size) + " B";
                }
            }

            var temp = @"<a href='" + UrlService.GetAbsoluteLink("price_temp/" + StrFileName) + @"' {0}>" +
                          Resource.Admin_ExportExcel_DownloadFile + @"</a>";
            hrefLink.Text = string.Format(temp, "style='color: white; text-decoration: none;'");

            spanMessage.Text = @"<span> " + Resource.Admin_ExportExcel_FileSize + @": " + sizeMesage + @"</span>" + @"<span>, " +
                            AdvantShop.Localization.Culture.ConvertDate(File.GetLastWriteTime(_strFullPath)) + @"</span>";
            ltLink.Text = string.Format(temp, "") + spanMessage.Text;
        }
        else
        {
            hrefLink.Text = "#";
            spanMessage.Text = @"<span>" + Resource.Admin_ExportExcel_NotExistDownloadFile + @"</span>";
            ltLink.Text = @"<span>" + Resource.Admin_ExportExcel_NotExistDownloadFile + @"</span>";
        }
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lError.Visible = false;
            lError.Text = string.Empty;
        }
        else
        {
            lError.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        lError.Visible = true;
        lError.Text = @"<br/>" + messageText;
    }

    protected void ddlSelectFields_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}