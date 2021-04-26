//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using AdvantShop.FilePath;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.Helpers.CsvHelper;
using AdvantShop.SaasData;
using AdvantShop.Statistic;
using Resources;

public partial class Admin_ImportCSV : Page
{
    private readonly string _filePath;
    private readonly string _fullPath;
    private bool _hasHeadrs;
    private Dictionary<string, int> FieldMapping = new Dictionary<string, int>();
    private readonly List<ProductFields.Fields> _mustRequiredFfield;

    protected Admin_ImportCSV()
    {
        _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
        _fullPath = string.Format("{0}{1}", _filePath, "importCSV.csv");
        _mustRequiredFfield = new List<ProductFields.Fields>();
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

    protected void btnAction_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(lblError.Text))
            return;

        divAction.Visible = false;
        choseDiv.Visible = false;
        if (File.Exists(_fullPath))
        {
            try
            {
                if (!ImportStatistic.IsRun)
                {
                    _hasHeadrs = Request["hasheadrs"] == "true";
                    ImportStatistic.Init();
                    ImportStatistic.IsRun = true;
                    linkCancel.Visible = true;
                    MsgErr(true);
                    lblRes.Text = string.Empty;
                    var tr = new Thread(ProcessCsv);
                    ImportStatistic.ThreadImport = tr;
                    tr.Start();
                    OutDiv.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }

    private static void LogInvalidData(string message)
    {
        ImportStatistic.WriteLog(message);
        ImportStatistic.TotalErrorRow++;
        ImportStatistic.RowPosition++;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        MsgErr(true);

        divStart.Visible = !ImportStatistic.IsRun && (string.IsNullOrEmpty(Request["action"]));
        divAction.Visible = !ImportStatistic.IsRun && (Request["action"] == "start");

        choseDiv.Visible = !ImportStatistic.IsRun;

        OutDiv.Visible = ImportStatistic.IsRun;
        linkCancel.Visible = ImportStatistic.IsRun;

        if (ImportStatistic.IsRun || (Request["action"] != "start")) return;
        if (!File.Exists(_fullPath)) return;

        var tbl = new Table() { ID = "tblValues" };
        var namesRow = new TableRow { ID = "namesRow", BackColor = System.Drawing.ColorTranslator.FromHtml("#0D76B8") };
        var firstValRow = new TableRow { ID = "firstValsRow" };
        var ddlRow = new TableRow { ID = "ddlRow" };

        var firstCell = new TableCell { Width = 200, BackColor = System.Drawing.Color.White, };
        firstCell.Controls.Add(new Label { Text = Resources.Resource.Admin_ImportCsv_Column, CssClass = "firstColumn" });
        var div1 = new Panel { CssClass = "arrow_left_bg" };
        div1.Controls.Add(new Panel { CssClass = "arrow_right_bg" });
        firstCell.Controls.Add(div1);


        var secondCell = new TableCell { Width = 200 };
        secondCell.Controls.Add(new Label { Text = Resources.Resource.Admin_ImportCsv_FistLineInTheFile, CssClass = "firstColumn" });
        var div2 = new Panel { CssClass = "arrow_left_bg_two" };
        div2.Controls.Add(new Panel { CssClass = "arrow_right_bg" });
        secondCell.Controls.Add(div2);

        var firdCell = new TableCell { Width = 200 };
        firdCell.Controls.Add(new Label { Text = Resources.Resource.Admin_ImportCsv_DataType, CssClass = "firstColumn" });
        var div3 = new Panel { CssClass = "arrow_left_bg_free" };
        div3.Controls.Add(new Panel { CssClass = "arrow_right_bg" });
        firdCell.Controls.Add(div3);
        var div4 = new Panel { Width = 200 };
        firdCell.Controls.Add(div4);

        namesRow.Cells.Add(firstCell);
        firstValRow.Cells.Add(secondCell);
        ddlRow.Cells.Add(firdCell);

        _hasHeadrs = chbHasHeadrs.Checked;
        using (var csv = new CsvHelper.CsvReader(new StreamReader(_fullPath, Encodings.GetEncoding())))
        {
            csv.Configuration.Delimiter = Separators.GetCharSeparator();
            csv.Configuration.BufferSize = 0x1000;
            csv.Configuration.HasHeaderRecord = false;

            csv.Read();

            if (_hasHeadrs && csv.CurrentRecord.HasDuplicates())
            {
                var strFileds = string.Empty;
                foreach (var item in csv.CurrentRecord.Duplicates())
                {
                    strFileds += "\"" + item + "\",";
                }
                MsgErr(Resource.Admin_ImportCsv_DuplicateHeader + strFileds.Trim(','));
                btnAction.Visible = false;
            }

            for (int i = 0; i < csv.CurrentRecord.Length; i++)
            {
                //Added by Evgeni to Import csv with dotes
               // csv.CurrentRecord[0] = csv.CurrentRecord[0].Replace(".", "").Replace(" ", "");

                var cell = new TableCell { ID = "cell" + i.ToString() };
                var lb = new Label();
                bool flagMustReqField = false;
                if (Request["hasheadrs"].ToLower() == "true")
                {
                    var tempCsv = (csv[i].Length > 50 ? csv[i].Substring(0, 49) : csv[i]).Replace("*", "");
                    if (_mustRequiredFfield.Any(item => ProductFields.GetStringNameByEnum(item).Replace("*", "") == tempCsv.ToLower()))
                    {
                        flagMustReqField = true;
                    }
                    lb.Text = tempCsv;
                }
                else
                {
                    lb.Text = Resource.Admin_ImportCsv_Empty;
                }
                lb.ForeColor = System.Drawing.Color.White;
                cell.Controls.Add(lb);

                if (flagMustReqField)
                {
                    var lbl = new Label
                    {
                        Text = @"*",
                        ForeColor = System.Drawing.Color.Red
                    };
                    cell.Controls.Add(lbl);
                }

                namesRow.Cells.Add(cell);

                cell = new TableCell() { Width = 150 };
                var ddl = new DropDownList { ID = "ddlType" + i.ToString(), Width = 150 };
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.None), ProductFields.GetStringNameByEnum(ProductFields.Fields.None)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Sku), ProductFields.GetStringNameByEnum(ProductFields.Fields.Sku)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Name), ProductFields.GetStringNameByEnum(ProductFields.Fields.Name).Trim('*')));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.ParamSynonym), ProductFields.GetStringNameByEnum(ProductFields.Fields.ParamSynonym)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Category), ProductFields.GetStringNameByEnum(ProductFields.Fields.Category).Trim('*')));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Enabled), ProductFields.GetStringNameByEnum(ProductFields.Fields.Enabled).Trim('*')));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Price), ProductFields.GetStringNameByEnum(ProductFields.Fields.Price).Trim('*')));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.PurchasePrice), ProductFields.GetStringNameByEnum(ProductFields.Fields.PurchasePrice)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Amount), ProductFields.GetStringNameByEnum(ProductFields.Fields.Amount)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Unit), ProductFields.GetStringNameByEnum(ProductFields.Fields.Unit)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Discount), ProductFields.GetStringNameByEnum(ProductFields.Fields.Discount)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.ShippingPrice), ProductFields.GetStringNameByEnum(ProductFields.Fields.ShippingPrice)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Weight), ProductFields.GetStringNameByEnum(ProductFields.Fields.Weight)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Size), ProductFields.GetStringNameByEnum(ProductFields.Fields.Size)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.BriefDescription), ProductFields.GetStringNameByEnum(ProductFields.Fields.BriefDescription)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Description), ProductFields.GetStringNameByEnum(ProductFields.Fields.Description)));

                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Title), ProductFields.GetStringNameByEnum(ProductFields.Fields.Title)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.MetaKeywords), ProductFields.GetStringNameByEnum(ProductFields.Fields.MetaKeywords)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.MetaDescription), ProductFields.GetStringNameByEnum(ProductFields.Fields.MetaDescription)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Photos), ProductFields.GetStringNameByEnum(ProductFields.Fields.Photos)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Markers), ProductFields.GetStringNameByEnum(ProductFields.Fields.Markers)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Properties), ProductFields.GetStringNameByEnum(ProductFields.Fields.Properties)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Producer), ProductFields.GetStringNameByEnum(ProductFields.Fields.Producer)));
                ddl.Items.Add(new ListItem(ProductFields.GetDisplayNameByEnum(ProductFields.Fields.OrderByRequest), ProductFields.GetStringNameByEnum(ProductFields.Fields.OrderByRequest)));

                ddl.SelectedValue = lb.Text.Replace("*", "").Trim().ToLower();
                cell.Controls.Add(ddl);
                ddlRow.Cells.Add(cell);
            }

            csv.Read();
            if (csv.CurrentRecord != null)
                for (int i = 0; i < csv.CurrentRecord.Length; i++)
                {
                    //Added by Evgeni to remove dots
                    //csv.CurrentRecord[0] = csv.CurrentRecord[0].Replace(".", "").Replace(" ", "");

                    var cell = new TableCell();
                    if (csv[i] == null)
                        cell.Controls.Add(new Label { Text = string.Empty });
                    else
                        cell.Controls.Add(new Label { Text = csv[i].Length > 50 ? csv[i].Substring(0, 49).HtmlEncode() : csv[i].HtmlEncode() });
                    firstValRow.Cells.Add(cell);
                }
        }
        tbl.Rows.Add(namesRow);
        tbl.Rows.Add(firstValRow);
        tbl.Rows.Add(ddlRow);
        choseDiv.Controls.Add(tbl);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HaveExcel))
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ImportXLS_Title);
        if (!IsPostBack)
        {
            if (divStart.Visible)
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
                ddlEncoding.SelectedValue = ((int)Encodings.CsvEnconing).ToString();

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
                ddlSeparetors.SelectedValue = ((int)Separators.CsvSeparator).ToString();
            }
        }

        if (choseDiv.FindControl("tblValues") != null && IsPostBack)
        {
            short index = 0;
            var cells = ((TableRow)choseDiv.FindControl("ddlRow")).Cells;
            foreach (TableCell item in cells)
            {
                var element = item.Controls.OfType<DropDownList>().FirstOrDefault();
                if (element == null) continue;

                if (element.SelectedValue != ProductFields.GetStringNameByEnum(ProductFields.Fields.None))
                {
                    if (!FieldMapping.ContainsKey(element.SelectedValue))
                        FieldMapping.Add(element.SelectedValue, index);
                    else
                    {
                        MsgErr(string.Format(Resource.Admin_ImportCsv_DuplicateMessage, element.SelectedItem.Text));//  "Duplicate field");
                        return;
                    }
                }
                index++;
            }
        }

        if (btnAction.Visible && IsPostBack)
        {
            foreach (var item in _mustRequiredFfield)
                if (!FieldMapping.ContainsKey(ProductFields.GetStringNameByEnum(item).Trim('*')))
                {
                    MsgErr(string.Format(Resource.Admin_ImportCsv_NotChoice, ProductFields.GetDisplayNameByEnum(item)));
                    return;
                }
        }
        //MsgErr(true);
    }

    private void ProcessCsv()
    {
        if (chboxDisableProducts.Checked)
        {
            ProductService.DisableAllProducts();
        }

        long count = 0;
        using (var csv = new CsvHelper.CsvReader(new StreamReader(_fullPath, Encodings.GetEncoding())))
        {
            csv.Configuration.Delimiter = Separators.GetCharSeparator();
            csv.Configuration.HasHeaderRecord = _hasHeadrs;
            while (csv.Read())
                count++;
        }

        ImportStatistic.TotalRow = count;

        using (var csv = new CsvHelper.CsvReader(new StreamReader(_fullPath, Encodings.GetEncoding())))
        {
            csv.Configuration.Delimiter = Separators.GetCharSeparator();
            csv.Configuration.HasHeaderRecord = _hasHeadrs;

            while (csv.Read())
            {
                if (!ImportStatistic.IsRun)
                {
                    csv.Dispose();
                    FileHelpers.DeleteFile(_fullPath);
                    return;
                }
                try
                {
                    //Added by Evgeni to Remove dots
                 //   csv.CurrentRecord[0] = csv.CurrentRecord[0].Replace(".", "").Replace(" ", "");
                    // Step by rows
                    var productInStrings = new Dictionary<ProductFields.Fields, string>();

                    string nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Sku);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Sku, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Name).Trim('*');
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        var name = Convert.ToString(csv[FieldMapping[nameField]]);
                        if (!string.IsNullOrEmpty(name))
                        {
                            productInStrings.Add(ProductFields.Fields.Name, name);
                        }
                        else
                        {
                            LogInvalidData(string.Format(Resource.Admin_ImportCsv_CanNotEmpty, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Name), ImportStatistic.RowPosition + 2));
                            continue;
                        }
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Enabled).Trim('*');
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        string enabled = Convert.ToString(csv[FieldMapping[nameField]]);
                        productInStrings.Add(ProductFields.Fields.Enabled, enabled);
                        //product.Enabled = !string.IsNullOrEmpty(enabled) && enabled.Trim().Equals("+");
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Discount);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        var discount = Convert.ToString(csv[FieldMapping[nameField]]);
                        if (string.IsNullOrEmpty(discount))
                            discount = "0";
                        decimal tmp;
                        if (decimal.TryParse(discount, out  tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.Discount, tmp.ToString());
                        }
                        else if (decimal.TryParse(discount, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.Discount, tmp.ToString());
                        }
                        else
                        {
                            LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Discount), ImportStatistic.RowPosition + 2));
                            continue;
                        }
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Weight);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        var weight = Convert.ToString(csv[FieldMapping[nameField]]);
                        if (string.IsNullOrEmpty(weight))
                            weight = "0";
                        decimal tmp;
                        if (decimal.TryParse(weight, out  tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.Weight, tmp.ToString());
                        }
                        else if (decimal.TryParse(weight, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.Weight, tmp.ToString());
                        }
                        else
                        {
                            LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Weight), ImportStatistic.RowPosition + 2));
                            continue;
                        }
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Size);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Size, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.BriefDescription);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.BriefDescription, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Description);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Description, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Price).Trim('*');
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        var price = Convert.ToString(csv[FieldMapping[nameField]]);
                        if (string.IsNullOrEmpty(price))
                            price = "0";
                        decimal tmp;
                        if (decimal.TryParse(price, out  tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.Price, tmp.ToString());
                        }
                        else if (decimal.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.Price, tmp.ToString());
                        }
                        else
                        {
                            LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Price), ImportStatistic.RowPosition + 2));
                            continue;
                        }
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.PurchasePrice);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        var sypplyprice = Convert.ToString(csv[FieldMapping[nameField]]);
                        if (string.IsNullOrEmpty(sypplyprice))
                            sypplyprice = "0";
                        decimal tmp;
                        if (decimal.TryParse(sypplyprice, out  tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.PurchasePrice, tmp.ToString());
                        }
                        else if (decimal.TryParse(sypplyprice, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.PurchasePrice, tmp.ToString());
                        }
                        else
                        {
                            LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.PurchasePrice), ImportStatistic.RowPosition + 2));
                            continue;
                        }
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.ShippingPrice);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        var shippingPrice = Convert.ToString(csv[FieldMapping[nameField]]);
                        if (string.IsNullOrEmpty(shippingPrice))
                            shippingPrice = "0";
                        decimal tmp;
                        if (decimal.TryParse(shippingPrice, out  tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.ShippingPrice, tmp.ToString());
                        }
                        else if (decimal.TryParse(shippingPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.ShippingPrice, tmp.ToString());
                        }
                        else
                        {
                            LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.ShippingPrice), ImportStatistic.RowPosition + 2));
                            continue;
                        }
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Amount);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        var amount = Convert.ToString(csv[FieldMapping[nameField]]);
                        if (string.IsNullOrEmpty(amount))
                            amount = "0";
                        int tmp;
                        if (int.TryParse(amount, out  tmp))
                        {
                            productInStrings.Add(ProductFields.Fields.Amount, amount);
                        }
                        else
                        {
                            LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Amount), ImportStatistic.RowPosition + 2));
                            continue;
                        }
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Unit);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Unit, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.ParamSynonym);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        string rewurl = Convert.ToString(csv[FieldMapping[nameField]]);
                        productInStrings.Add(ProductFields.Fields.ParamSynonym, rewurl);
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Title);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Title, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.MetaKeywords);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.MetaKeywords, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.MetaDescription);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.MetaDescription, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Photos);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Photos, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Markers);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Markers, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Properties);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Properties, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Producer);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        productInStrings.Add(ProductFields.Fields.Producer, Convert.ToString(csv[FieldMapping[nameField]]));
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.OrderByRequest);
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        string orderbyrequest = Convert.ToString(csv[FieldMapping[nameField]]);
                        productInStrings.Add(ProductFields.Fields.OrderByRequest, orderbyrequest);
                    }

                    nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Category).Trim('*');
                    if (FieldMapping.ContainsKey(nameField))
                    {
                        var parentCategory = Convert.ToString(csv[FieldMapping[nameField]]);
                        if (!string.IsNullOrEmpty(parentCategory))
                        {
                            productInStrings.Add(ProductFields.Fields.Category, parentCategory);
                        }
                    }

                    ImportProduct.UpdateInsertProduct(productInStrings);

                }
                catch (Exception ex)
                {
                    MsgErr(ex.Message + " at csv");
                    Debug.LogError(ex);
                }
            }
            CategoryService.RecalculateProductsCountManual();
        }
        ImportStatistic.IsRun = false;
        LuceneSearch.CreateAllIndexInBackground();
        CacheManager.Clean();
        FileHelpers.DeleteFilesFromImageTempInBackground();
        FileHelpers.DeleteFile(_fullPath);
    }

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (ImportStatistic.ThreadImport.IsAlive)
        {
            ImportStatistic.IsRun = false;
            hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
        }
    }

    protected void btnSaveSettings_Click(object sender, EventArgs e)
    {
        if (FileUpload.HasFile)
            FileUpload.SaveAs(_fullPath);
        else return;
        if (!File.Exists(_fullPath)) return;

        Encodings.CsvEnconing = (Encodings.EncodingsEnum)Convert.ToInt32(ddlEncoding.SelectedValue);
        Separators.CsvSeparator = (Separators.SeparatorsEnum)Convert.ToInt32(ddlSeparetors.SelectedValue);
        Response.Redirect("ImportCSV.aspx?action=start&hasheadrs=" + chbHasHeadrs.Checked.ToString().ToLower());
    }
}