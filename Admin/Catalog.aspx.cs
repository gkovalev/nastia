using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.SaasData;
using AjaxControlToolkit;
using Resources;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class Admin_Catalog : Page
{
    #region EShowMethod enum

    public enum EShowMethod
    {
        Normal,
        AllProducts,
        OnlyInCategories,
        OnlyWithoutCategories
    }

    #endregion

    private readonly IList<string> _selectedCategories;
    protected int CategoryId = -1;
    private bool _inverseSelection;
    private bool _needReloadTree;
    private SqlPaging _paging;
    private InSetFieldFilter _selectionFilter;
    private InSetFieldFilter _selectionFilterCategories;
    protected EShowMethod ShowMethod = EShowMethod.Normal;

    public Admin_Catalog()
    {
        _selectedCategories = new List<string>();
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    //protected override void OnLoad(EventArgs e)
    protected void Page_Load(object sender, EventArgs e)
    {
        //base.OnLoad(e);
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_MasterPageAdminCatalog_Catalog);

        Category cat = null;
        MainPageProductNew.LoadData();
        MainPageProductOnSale.LoadData();
        MainPageProductBestseller.LoadData();
        if (!string.IsNullOrEmpty(Request["categoryid"]))
        {
            if (Request["categoryid"].ToLower().Equals("WithoutCategory".ToLower()))
            {
                ShowMethod = EShowMethod.OnlyWithoutCategories;
            }
            else if (Request["categoryid"].ToLower().Equals("InCategories".ToLower()))
            {
                ShowMethod = EShowMethod.OnlyInCategories;
            }
            else if (Request["categoryid"].ToLower().Equals("AllProducts".ToLower()))
            {
                ShowMethod = EShowMethod.AllProducts;
            }
            else
            {
                ShowMethod = EShowMethod.Normal;
                int.TryParse(Request["categoryid"], out CategoryId);
                cat = CategoryService.GetCategory(CategoryId);
            }
        }
        else
        {
            CategoryId = 0;
            ShowMethod = EShowMethod.Normal;
        }

        if (cat == null)
        {
            CategoryId = 0;
            if (ShowMethod == EShowMethod.Normal)
            {
                ShowMethod = EShowMethod.AllProducts;
                ShowMethod = EShowMethod.Normal;
            }
        }
        else
        {
            CategoryId = cat.CategoryId;
            lblCategoryName.Text = cat.Name;
            ConfirmButtonExtenderCategory.ConfirmText =
                string.Format(Resource.Admin_MasterPageAdminCatalog_Confirmation, cat.Name);
        }

        hlEditCategory.NavigateUrl = "javascript:open_window(\'m_Category.aspx?CategoryID=" + CategoryId + "&mode=edit\', 750, 640)";

        if (!IsPostBack)
        {
            var node2 = new TreeNode { Text = Resource.Admin_m_Category_Root, Value = "0", Selected = true };
            tree2.Nodes.Add(node2);

            LoadChildCategories2(tree2.Nodes[0]);

            _paging = new SqlPaging();

            switch (ShowMethod)
            {
                case EShowMethod.AllProducts:
                    lblCategoryName.Text = Resource.Admin_Catalog_AllProducts;
                    Localize_Admin_Catalog_CategoryLocation.Visible = false;
                    _paging.TableName =

                        "[Catalog].[Product] left JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId]  and Type='Product' AND [Main] = 1 LEFT JOIN [Catalog].[ProductCategories] ON [Catalog].[ProductCategories].[ProductID] = [Product].[ProductID]";

                    break;
                case EShowMethod.OnlyInCategories:
                    lblCategoryName.Text = Resource.Admin_Catalog_AllProductsInCategories;
                    Localize_Admin_Catalog_CategoryLocation.Visible = false;
                    _paging.TableName =

                        "[Catalog].[Product] left JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type='Product' AND [Main] = 1 inner JOIN [Catalog].[ProductCategories] ON [Catalog].[ProductCategories].[ProductID] = [Product].[ProductID]";

                    break;
                case EShowMethod.OnlyWithoutCategories:
                    lblCategoryName.Text = Resource.Admin_Catalog_AllProductsWithoutCategories;
                    Localize_Admin_Catalog_CategoryLocation.Visible = false;
                    _paging.TableName =

                        "[Catalog].[Product] inner join (select ProductId from Catalog.Product where ProductId not in(Select ProductId from Catalog.ProductCategories)) as tmp on tmp.ProductId=[Product].[ProductID] Left JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and [OfferListID] = 6 LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId]  and Type='Product' AND [Main] = 1";

                    break;
                case EShowMethod.Normal:
                    Localize_Admin_Catalog_CategoryLocation.Visible = true;
                    _paging.TableName =
                        "[Catalog].[Product] left JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId]  and Type='Product' AND [Main] = 1 INNER JOIN Catalog.ProductCategories on ProductCategories.ProductId = [Product].[ProductID]";

                    _paging.TableNameForUnion = "[Catalog].[Category] left join Catalog.Photo on Photo.ObjId = Category.CategoryId and Type='CategorySmall' ";


                    var ff = new Field { Name = "[CategoryID] as ID", IsDistinct = true };
                    _paging.AddFieldUnionTable(ff);

                    var rootFilter = new NotEqualFieldFilter { Value = CategoryId.ToString(), ParamName = "@root" };
                    ff = new Field { Name = "[CategoryID] as CurrentCategory", Filter = rootFilter, NotInQuery = true };
                    _paging.AddFieldUnionTable(ff);

                    ff = new Field { Name = "PhotoName" };
                    _paging.AddFieldUnionTable(ff);

                    var pf = new EqualFieldFilter { Value = CategoryId.ToString(), ParamName = "@Parent" }; // InChildCategoriesFieldFilter()

                    ff = new Field { Name = "ParentCategory", NotInQuery = true, Filter = pf };
                    _paging.AddFieldUnionTable(ff);

                    ff = new Field { Name = "Category.[Description] as BriefDescription" };
                    _paging.AddFieldUnionTable(ff);



                    ff = new Field { Name = "Name", Sorting = SortDirection.Ascending };
                    _paging.AddFieldUnionTable(ff);

                    ff = new Field { Name = "0 as Price" };
                    _paging.AddFieldUnionTable(ff);

                    ff = new Field { Name = "0 as Amount" };
                    _paging.AddFieldUnionTable(ff);

                    ff = new Field { Name = "Enabled" };
                    _paging.AddFieldUnionTable(ff);

                    ff = new Field { Name = "0 as typeItem" };
                    _paging.AddFieldUnionTable(ff);

                    ff = new Field { Name = "SortOrder" };
                    _paging.AddFieldUnionTable(ff);

                    ff = new Field { Name = "convert(nvarchar(50), CategoryID) as ArtNo" };
                    _paging.AddFieldUnionTable(ff);
                    break;
            }

            _paging.ItemsPerPage = 10;

            _paging.AddField(new Field { Name = "[Product].[ProductID] as ID", IsDistinct = true });

            _paging.AddField(new Field { Name = "PhotoName" });

            var pfilter = new EqualFieldFilter { Value = CategoryId.ToString(), ParamName = "@CategoryID" }; // InChildCategoriesFieldFilter()

            if ((ShowMethod != EShowMethod.AllProducts) && (ShowMethod != EShowMethod.OnlyWithoutCategories) && (ShowMethod != EShowMethod.OnlyInCategories))
            {
                _paging.AddField(new Field { Name = "[ProductCategories].[CategoryID]", NotInQuery = true, Filter = pfilter });
            }

            _paging.AddField(new Field { Name = "[OfferListID]", NotInQuery = true, Filter = ShowMethod == EShowMethod.OnlyWithoutCategories ? null : new EqualFieldFilter { ParamName = "@OfferListID", Value = CatalogService.DefaultOfferListId.ToString(CultureInfo.InvariantCulture) } });

            var f = new Field { Name = "BriefDescription" };
            _paging.AddField(f);



            f = new Field { Name = "Name" };

            _paging.AddField(f);

            f = new Field { Name = "Price" };
            _paging.AddField(f);

            f = new Field { Name = "Amount" };
            _paging.AddField(f);

            f = new Field { Name = "Enabled" };
            _paging.AddField(f);

            f = new Field { Name = "1 as typeItem", Sorting = SortDirection.Ascending };
            _paging.AddField(f);

            f = new Field();
            switch (ShowMethod)
            {
                case EShowMethod.Normal:
                    f.Name = "[ProductCategories].[SortOrder]";
                    f.Sorting = SortDirection.Ascending;
                    break;
                default:
                    f.Name = "\'-1\' as SortOrder"; // do not consider sorting in categories
                    grid.Columns[9].Visible = false; // 10 is index of Column "SortOrder"
                    break;
            }

            _paging.AddField(f);

            f = new Field { Name = "ArtNo", Sorting = SortDirection.Ascending };
            _paging.AddField(f);

            //grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");

            _paging.ItemsPerPage = 10;

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            ViewState["Paging"] = _paging;

        }
        else
        {
            _paging = (SqlPaging)(ViewState["Paging"]);
            _paging.ItemsPerPage = Convert.ToInt32(ddRowsPerPage.SelectedValue);

            if (_paging == null)
            {
                throw (new Exception("Paging lost"));
            }

            string strIds = Request.Form["SelectedIds"];

            if (!string.IsNullOrEmpty(strIds))
            {
                strIds = strIds.Trim();
                var arrids = strIds.Split(' ');

                var ids = new string[arrids.Length];
                _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                _selectionFilterCategories = new InSetFieldFilter { IncludeValues = true };

                for (int idx = 0; idx <= ids.Length - 1; idx++)
                {
                    string t = arrids[idx];
                    var idParts = t.Split('_');
                    switch (idParts[0])
                    {
                        case "Product":
                            if (idParts[1] != "-1")
                            {
                                ids[idx] = idParts[1];
                            }
                            else
                            {
                                _selectionFilter.IncludeValues = false;
                                _inverseSelection = true;
                            }
                            break;
                        case "Category":
                            if (idParts[1] != "-1")
                            {
                                _selectedCategories.Add(idParts[1]);
                            }
                            else
                            {
                                _selectionFilterCategories.IncludeValues = false;
                                _inverseSelection = true;
                            }
                            break;
                        default:
                            _inverseSelection = true;
                            break;
                    }
                }
                _selectionFilter.Values = ids.Distinct().Where(item => item != null).ToArray();
                _selectionFilterCategories.Values = _selectedCategories.ToArray();
            }
        }
    }

    protected void ibRecalculate_Click(object sender, ImageClickEventArgs e)
    {
        CategoryService.RecalculateProductsCountManual();
        tree.Nodes.Clear();
        _needReloadTree = true;
    }

    protected void hlDeleteCategory_Click(object sender, EventArgs e)
    {
        var needRedirect = false;
        var cat = CategoryService.GetCategory(CategoryId);
        try
        {

            if (CategoryId == -1)
            {
                return;
            }
            CategoryService.DeleteCategoryAndPhotos(CategoryId);
            CategoryService.DeleteCategoryLink(CategoryId);
            CategoryService.RecalculateProductsCountManual();
            needRedirect = true;

        }
        catch (Exception ex)
        {
            lMessage.Text = ex.Message;
            lMessage.Visible = true;
            Debug.LogError(ex);
        }

        if (needRedirect)
        {
            Response.Redirect("Catalog.aspx?CategoryID=" + cat.ParentCategoryId);
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        //-----Selection filter
        if (ddSelect.SelectedIndex != 0)
        {
            if (ddSelect.SelectedIndex == 2)
            {
                if (_selectionFilter != null)
                {
                    _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                }
                else
                {
                    _selectionFilter = null;
                }
            }
            if (_selectionFilter != null)
                _paging.Fields["ID"].Filter = _selectionFilter;
            if (_selectionFilterCategories != null)
                _paging.FieldsUnionTable["ID"].Filter = _selectionFilterCategories;
        }
        else
        {
            _paging.Fields["ID"].Filter = null;
            if (_paging.FieldsUnionTable != null)
            {
                _paging.FieldsUnionTable["ID"].Filter = null;
            }
        }

        //----Enabled filter
        if (ddlEnabled.SelectedIndex != 0)
        {
            var efilter = new EqualFieldFilter { ParamName = "@enabled" };
            if (ddlEnabled.SelectedIndex == 1)
            {
                efilter.Value = "1";
            }
            if (ddlEnabled.SelectedIndex == 2)
            {
                efilter.Value = "0";
            }
            _paging.Fields["Enabled"].Filter = efilter;
            if (_paging.FieldsUnionTable != null)
            {
                _paging.FieldsUnionTable["Enabled"].Filter = new EqualFieldFilter() { ParamName = "@cEnabled", Value = efilter.Value };
            }
        }
        else
        {
            _paging.Fields["Enabled"].Filter = null;
            if (_paging.FieldsUnionTable != null)
            {
                _paging.FieldsUnionTable["Enabled"].Filter = null;
            }
        }

        //----Price filter

        var priceFilter = new RangeFieldFilter { ParamName = "@price" };

        int priceFrom;
        priceFilter.From = int.TryParse(txtPriceFrom.Text, out priceFrom) ? priceFrom : 0;

        int priceTo;
        priceFilter.To = int.TryParse(txtPriceTo.Text, out priceTo) ? priceTo : int.MaxValue;

        if (!string.IsNullOrEmpty(txtPriceFrom.Text) || !string.IsNullOrEmpty(txtPriceTo.Text))
        {
            _paging.Fields["Price"].Filter = priceFilter;
        }
        else
        {
            _paging.Fields["Price"].Filter = null;
        }

        //----Qty filter
        var qtyFilter = new RangeFieldFilter { ParamName = "@Amount" };
        int from;
        qtyFilter.From = int.TryParse(txtQtyFrom.Text, out from) ? from : int.MinValue;

        int to;
        qtyFilter.To = int.TryParse(txtQtyTo.Text, out to) ? to : int.MaxValue;


        if (!string.IsNullOrEmpty(txtQtyFrom.Text) || !string.IsNullOrEmpty(txtQtyTo.Text))
        {
            _paging.Fields["Amount"].Filter = qtyFilter;
        }
        else
        {
            _paging.Fields["Amount"].Filter = null;
        }

        //----SortOrder filter
        var soFilter = new RangeFieldFilter { ParamName = "@SortOrder" };

        try
        {
            soFilter.From = int.Parse(txtSortOrderFrom.Text);
        }
        catch (Exception)
        {
            soFilter.From = int.MinValue;
        }

        try
        {
            soFilter.To = int.Parse(txtSortOrderTo.Text);
        }
        catch (Exception)
        {
            soFilter.To = int.MaxValue;
        }

        if (ShowMethod == EShowMethod.Normal)
        {
            if (!string.IsNullOrEmpty(txtSortOrderFrom.Text) || !string.IsNullOrEmpty(txtSortOrderTo.Text))
            {
                _paging.Fields["[ProductCategories].[SortOrder]"].Filter = soFilter;
                if (_paging.FieldsUnionTable != null)
                {
                    _paging.FieldsUnionTable["SortOrder"].Filter = new RangeFieldFilter() { ParamName = "@cSortOrder", From = soFilter.From, To = soFilter.To };
                }
            }
            else
            {
                _paging.Fields["[ProductCategories].[SortOrder]"].Filter = null;
                if (_paging.FieldsUnionTable != null)
                {
                    _paging.FieldsUnionTable["SortOrder"].Filter = null;
                }
            }
        }

        //----Id filter
        if (!string.IsNullOrEmpty(txtArtNo.Text))
        {
            var sfilter = new CompareFieldFilter { Expression = txtArtNo.Text, ParamName = "@artno" };
            _paging.Fields["ArtNo"].Filter = sfilter;
            if (_paging.FieldsUnionTable != null)
            {
                _paging.FieldsUnionTable["ArtNo"].Filter = new CompareFieldFilter { Expression = sfilter.Expression, ParamName = "@cArtNo" };
            }
        }
        else
        {
            _paging.Fields["ArtNo"].Filter = null;
            if (_paging.FieldsUnionTable != null)
            {
                _paging.FieldsUnionTable["ArtNo"].Filter = null;
            }
        }

        //----Name filter
        if (!string.IsNullOrEmpty(txtName.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtName.Text, ParamName = "@name" };
            _paging.Fields["Name"].Filter = nfilter;
            if (_paging.FieldsUnionTable != null)
            {
                _paging.FieldsUnionTable["Name"].Filter = new CompareFieldFilter() { ParamName = "@cName", Expression = nfilter.Expression };
            }
        }
        else
        {
            _paging.Fields["Name"].Filter = null;

            if (_paging.FieldsUnionTable != null)
            {
                _paging.FieldsUnionTable["Name"].Filter = null;
            }
        }

        //---Photo filter
        if (ddPhoto.SelectedIndex != 0)
        {
            var phfilter = new NullFieldFilter();
            if (ddPhoto.SelectedIndex == 1)
            {
                phfilter.Null = false;
            }
            if (ddPhoto.SelectedIndex == 2)
            {
                phfilter.Null = true;
            }
            phfilter.ParamName = "@PhotoName";
            _paging.Fields["PhotoName"].Filter = phfilter;
        }
        else
        {
            _paging.Fields["PhotoName"].Filter = null;
        }

        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;
        lblFound.Text = _paging.TotalRowsCount.ToString();
        pnlFilterCount.Visible = true;
        pnlNormalCount.Visible = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        btnFilter_Click(sender, e);
        grid.ChangeHeaderImageUrl(null, null);
    }

    protected void pn_SelectedPageChanged(object sender, EventArgs e)
    {
        _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
    }

    protected void linkGO_Click(object sender, EventArgs e)
    {
        int pagen;
        try
        {
            pagen = int.Parse(txtPageNum.Text);
        }
        catch (Exception)
        {
            pagen = -1;
        }
        if (pagen < 1 || pagen > _paging.PageCount) return;
        pageNumberer.CurrentPageIndex = pagen;
        _paging.CurrentPageIndex = pagen;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (grid.UpdatedRow != null)
        {
            //еслли это категория то обновляем как категорию
            if (grid.UpdatedRow["typeItem"] == "0")
            {
                var cat = CategoryService.GetCategory(Convert.ToInt32(grid.UpdatedRow["Id"].Replace("Category_", "")));
                cat.Name = grid.UpdatedRow["Name"];
                cat.Enabled = bool.Parse(grid.UpdatedRow["Enabled"]);
                cat.SortOrder = int.Parse(grid.UpdatedRow["SortOrder"]);
                CategoryService.UpdateCategory(cat, true);
            }
            //если это продукт то обновляетм продукт (код продукта 1)
            if (grid.UpdatedRow["typeItem"] == "1")
            {
                var cproduct = ProductService.GetProduct(Convert.ToInt32(grid.UpdatedRow["Id"].Replace("Product_", "")));
                cproduct.Name = grid.UpdatedRow["Name"];
                cproduct.ArtNo = grid.UpdatedRow["ArtNo"];

                decimal price = -1;
                try
                {
                    price = decimal.Parse(grid.UpdatedRow["Price"]);
                }
                catch (Exception)
                {
                }
                if (price != -1)
                {
                    cproduct.Offers[0].Price = price;
                }

                var amount = -2;
                try
                {
                    amount = int.Parse(grid.UpdatedRow["Qty"]);
                }
                catch (Exception)
                {
                }
                if (amount != -2)
                {
                    cproduct.Offers[0].Amount = amount;
                }

                bool enabledChanged = cproduct.Enabled != bool.Parse(grid.UpdatedRow["Enabled"]);
                cproduct.Enabled = bool.Parse(grid.UpdatedRow["Enabled"]);


                if (grid.UpdatedRow.ContainsKey("SortOrder"))
                {
                    int srtOrd;
                    int.TryParse(grid.UpdatedRow["SortOrder"], out srtOrd);
                    if (ShowMethod == EShowMethod.Normal && CategoryId > 0)
                    {
                        ProductService.UpdateProductLinkSort(cproduct.ProductId, srtOrd, Convert.ToInt32(Request["categoryid"]));
                    }
                }
                ProductService.UpdateProduct(cproduct, true);

                if (enabledChanged)
                {
                    CategoryService.RecalculateProductsCountManual();
                    tree.Nodes.Clear();
                    _needReloadTree = true;
                }
            }
        }

        var data = _paging.PageItems;
        while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
        {
            _paging.CurrentPageIndex--;
            data = _paging.PageItems;
        }

        var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
        data.Columns.Add(clmn);
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            for (var i = 0; i <= data.Rows.Count - 1; i++)
            {
                var intIndex = i;
                if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["ID"].ToString()))
                {
                    data.Rows[i]["IsSelected"] = !_inverseSelection;
                }

                if (_selectedCategories.Contains(data.Rows[intIndex]["ID"].ToString()))
                {
                    data.Rows[i]["IsSelected"] = !_inverseSelection;
                }
            }
        }

        if (data.Rows.Count < 1)
        {
            goToPage.Visible = false;
        }


        btnAddCategory.OnClientClick = "javascript:open_window(\'m_Category.aspx?CategoryID=" + CategoryId +
                                       "&mode=create\',750,640); return false;";

        if (!IsPostBack || _needReloadTree)
        {
            ibRecalculate.Attributes.Add("onmouseover", "this.src=\'images/broundarrow.gif\';");
            tree.Nodes.Clear();
            LoadRootCategories(tree.Nodes);

            var parentCategories = CategoryService.GetParentCategories(CategoryId);

            var nodes = tree.Nodes;

            if (CategoryId == 0 && ShowMethod == EShowMethod.Normal)
            {
                var node = (from TreeNode n in nodes select n).First();
                node.Select();
            }

            for (var i = parentCategories.Count - 1; i >= 0; i--)
            {
                var ii = i;
                var tn = (from TreeNode n in nodes where n.Value == parentCategories[ii].CategoryId.ToString() select n).SingleOrDefault();
                if (i == 0)
                {
                    tn.Select();
                    tn.Expand();
                }
                else
                {
                    tn.Expand();
                }

                nodes = tn.ChildNodes;
            }
        }

        grid.DataSource = data;
        grid.DataBind();

        sn.BuildNavigationAdmin(CategoryId);
        pageNumberer.PageCount = _paging.PageCount;

        switch (ShowMethod)
        {
            case EShowMethod.AllProducts:
                lblProducts.Text = CategoryService.GetTolatCounTofProducts().ToString();
                btnAddCategory.Visible = false;
                lblsubCats.Text = @"0";
                hlEditCategory.Visible = false;
                hlDeleteCategory.Visible = false;
                lblSeparator.Visible = false;
                sn.Visible = false;
                break;
            case EShowMethod.OnlyWithoutCategories:
                lblProducts.Text = CategoryService.GetTolatCounTofProductsWithoutCategories().ToString();
                lblsubCats.Text = @"0";
                btnAddCategory.Visible = false;
                hlEditCategory.Visible = false;
                hlDeleteCategory.Visible = false;
                lblSeparator.Visible = false;
                sn.Visible = false;
                break;
            case EShowMethod.OnlyInCategories:
                lblProducts.Text = CategoryService.GetTolatCounTofProductsInCategories().ToString();
                lblsubCats.Text = @"0";
                btnAddCategory.Visible = false;
                hlEditCategory.Visible = false;
                hlDeleteCategory.Visible = false;
                lblSeparator.Visible = false;
                sn.Visible = false;
                break;

            case EShowMethod.Normal:
                if (CategoryId == 0)
                {
                    btnAddProduct.Visible = false;
                    lblSeparator.Visible = false;
                    hlDeleteCategory.Visible = false;
                }
                else
                {
                    btnAddProduct.Visible = true;
                }
                lblProducts.Text = CategoryService.GetProductsCountInCategory(CategoryId).ToString();
                lblsubCats.Text = CategoryService.GetSubcategoriesCount(CategoryId).ToString();
                categoryCountBlock.Visible = true;
                break;
        }
    }

    protected void grid_DataBinding(object sender, EventArgs e)
    {
        var category = CategoryService.GetCategory(CategoryId);

        if ((category == null) || CategoryId == 0) return;
        var data = (DataTable)grid.DataSource;
        var row = data.NewRow();
        row["Name"] = string.Empty;
        row["typeItem"] = "-1";
        row["ID"] = category.ParentCategoryId;
        row["PhotoName"] = "";
        row["BriefDescription"] = string.Empty;
        row["ArtNo"] = 0;
        row["Price"] = 0;
        row["Amount"] = 0;
        row["Enabled"] = false;
        row["SortOrder"] = 0;

        data.Rows.InsertAt(row, 0);
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                         {
                             {"ArtNo", "arrowArtNo"},
                             {"[Product].[ProductID]", "arrowArtNo"},
                             {"Name", "arrowName"},
                             {"Price", "arrowPrice"},
                             {"Amount", "arrowQty"},
                             {"Enabled", "arrowEnabled"},
                             {"[ProductCategories].[SortOrder]", "arrowSortOrder"},
                             {"typeItem", "arrowtypeItem"}
                         };

        const string urlArrowUp = "images/arrowup.gif";
        const string urlArrowDown = "images/arrowdown.gif";
        const string urlArrowGray = "images/arrowdownh.gif";

        var csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
        var nsf = new Field();
        if (e.SortExpression.Equals("SortOrder"))
        {
            switch (ShowMethod)
            {
                case EShowMethod.Normal:
                    nsf = _paging.Fields["[ProductCategories].[SortOrder]"];
                    break;
            }
        }
        else
        {
            nsf = _paging.Fields[e.SortExpression];
        }

        if (nsf.Name.Equals(csf.Name))
        {
            csf.Sorting = csf.Sorting == SortDirection.Ascending
                              ? SortDirection.Descending
                              : SortDirection.Ascending;
            grid.ChangeHeaderImageUrl(arrows[csf.Name],
                                      (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
        }
        else
        {
            csf.Sorting = null;
            grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

            nsf.Sorting = SortDirection.Ascending;
            grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
        }

        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;
    }

    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        if (SaasDataService.IsSaasEnabled)
        {
            var maxProductCount = SaasDataService.CurrentSaasData.ProductsCount;
            var productsCount = ProductService.GetProductCountByOffer(6);

            if (productsCount >= maxProductCount)
            {
                lMessage.Text = Resource.Admin_Product_MaximumProducts + " - " + maxProductCount;
                lMessage.Visible = true;
                return;
            }
        }

        Response.Redirect("Product.aspx?categoryid=" + CategoryId);
    }

    protected void lbDeleteSelected1_Click(object sender, EventArgs e)
    {
        if (!_inverseSelection)
        {
            if (_selectionFilter != null)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    ProductService.DeleteProduct(Convert.ToInt32(id), true);
                }

                foreach (string catId in _selectedCategories)
                {
                    CategoryService.DeleteCategoryAndPhotos(Convert.ToInt32(catId));
                }
            }
        }
        else
        {
            List<int> itemsIds;
            List<int> itemsUnionIds;
            //удаляем товары без категорий
            switch (Request["categoryid"])
            {
                case "WithoutCategory":
                    itemsIds = _paging.ItemsIds<int>("[Product].[ProductID] as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        ProductService.DeleteProduct(id, true);
                    }
                    break;
                case "InCategories":
                    itemsIds = _paging.ItemsIds<int>("[Product].[ProductID] as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        ProductService.DeleteProduct(id, true);
                    }
                    break;
                case "AllProducts":
                    itemsIds = _paging.ItemsIds<int>("[Product].[ProductID] as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        ProductService.DeleteProduct(id, true);
                    }
                    break;
                case "0":
                    itemsIds = _paging.ItemsIds<int>("[Product].[ProductID] as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        ProductService.DeleteProduct(id, true);
                    }
                    itemsUnionIds = _paging.ItemsUnionIds<int>("[CategoryID] as ID");
                    foreach (int id in itemsUnionIds.Where(id => !_selectedCategories.Contains(id.ToString())))
                    {
                        CategoryService.DeleteCategoryAndPhotos(id);
                        CategoryService.DeleteCategoryLink(id);
                    }
                    break;
                default:
                    itemsIds = _paging.ItemsIds<int>("[Product].[ProductID] as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        ProductService.DeleteProduct(id, true);
                    }
                    itemsUnionIds = _paging.ItemsUnionIds<int>("[CategoryID] as ID");
                    foreach (int id in itemsUnionIds.Where(id => !_selectedCategories.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        CategoryService.DeleteCategoryAndPhotos(id);
                        CategoryService.DeleteCategoryLink(id);
                    }
                    break;
            }
        }
        CategoryService.RecalculateProductsCountManual();
        _needReloadTree = true;
        //Response.Redirect("Catalog.aspx" + Request.Url.Query);
    }

    protected void lbDeleteSelectedFromCategory_Click(object sender, EventArgs e)
    {
        switch (ShowMethod)
        {
            default:
                if (!_inverseSelection)
                {
                    if (_selectionFilter != null)
                    {
                        foreach (var id in _selectionFilter.Values)
                        {
                            CategoryService.DeleteCategoryAndLink(Convert.ToInt32(id), Convert.ToInt32(Request["categoryid"]));
                        }
                    }

                    if (_selectedCategories != null)
                    {
                        foreach (var id in _selectedCategories)
                        {
                            CategoryService.DeleteCategoryAndPhotos(Convert.ToInt32(id));
                            CategoryService.DeleteCategoryLink(Convert.ToInt32(id));
                        }
                    }
                }
                else
                {
                    foreach (var p in CategoryService.GetProductsByCategoryId(CategoryId))
                    {
                        if (!_selectionFilter.Values.Contains(p.ProductId.ToString()))
                        {
                            CategoryService.DeleteCategoryAndLink(p.ProductId, Convert.ToInt32(Request["categoryid"]));
                        }
                    }

                    foreach (var p in CategoryService.GetChildCategoriesByCategoryId(CategoryId, false))
                    {
                        if (_selectedCategories.Contains(p.CategoryId.ToString())) continue;

                        CategoryService.DeleteCategoryAndPhotos(p.CategoryId);
                        CategoryService.DeleteCategoryLink(p.CategoryId);
                    }
                }
                break;
        }
        CategoryService.RecalculateProductsCountManual();
        Response.Redirect("Catalog.aspx" + Request.Url.Query);
    }

    protected void tree_TreeNodeCommand(object sender, CommandEventArgs e)
    {
        var needRedirect = false;
        try
        {
            if (e.CommandName.StartsWith("DeleteCategory"))
            {
                int catId = CategoryId;
                if (e.CommandName.Contains("#"))
                {
                    catId = Convert.ToInt32(e.CommandName.Substring(e.CommandName.IndexOf("#") + 1));
                }

                if (catId == -1)
                {
                    return;
                }
                if (catId != 0)
                {
                    CategoryService.DeleteCategoryAndPhotos(catId);
                    CategoryService.DeleteCategoryLink(catId);
                    CategoryService.RecalculateProductsCountManual();
                    needRedirect = true;
                }
                else
                {
                    lMessage.Text = Resource.Admin_Catalog_CantDellRoot;
                    lMessage.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lMessage.Text = ex.Message;
            lMessage.Visible = true;
            //Debug.LogError(ex, sender, e);
            Debug.LogError(ex);
        }

        if (needRedirect)
        {
            Response.Redirect("Catalog.aspx");
        }
    }

    private void LoadRootCategories(TreeNodeCollection treeNodeCollection)
    {
        var rootCategory = CategoryService.GetCategory(0);
        var newNode = new ButtonTreeNodeCatalog
                          {
                              Text = string.Format("{3}{0} ({1}/{2}){4}", rootCategory.Name, rootCategory.ProductsCount, rootCategory.TotalProductsCount,
                                    rootCategory.ProductsCount == 0 ? "<span class=\"lightlink\">" : string.Empty,
                                    rootCategory.ProductsCount == 0 ? "</span>" : string.Empty),
                              Value = rootCategory.CategoryId.ToString(),
                              NavigateUrl = "Catalog.aspx?CategoryID=" + rootCategory.CategoryId,
                              TreeView = tree,
                              Expanded = true,
                              PopulateOnDemand = false
                          };

        treeNodeCollection.Add(newNode);

        foreach (var c in CategoryService.GetChildCategoriesByCategoryId(0, false))
        {
            newNode = new ButtonTreeNodeCatalog
                          {
                              Text = string.Format("{3}{0} ({1}/{2}){4}", c.Name, c.ProductsCount, c.TotalProductsCount,
                                    c.ProductsCount == 0 ? "<span class=\"lightlink\">" : string.Empty,
                                    c.ProductsCount == 0 ? "</span>" : string.Empty),
                              MessageToDel = Server.HtmlEncode(string.Format(Resource.Admin_MasterPageAdminCatalog_Confirmation, c.Name.Replace("'", ""))),
                              Value = c.CategoryId.ToString(),
                              NavigateUrl = "Catalog.aspx?CategoryID=" + c.CategoryId,
                              TreeView = tree
                          };
            if (c.HasChild)
            {
                newNode.Expanded = false;
                newNode.PopulateOnDemand = true;
            }
            else
            {
                newNode.Expanded = true;
                newNode.PopulateOnDemand = false;
            }

            treeNodeCollection.Add(newNode);
        }
    }

    private void LoadChildCategories(TreeNode node)
    {
        foreach (var c in CategoryService.GetChildCategoriesByCategoryId(Convert.ToInt32(node.Value), false))
        {
            var newNode = new ButtonTreeNodeCatalog
                              {
                                  Text = string.Format("{3}{0} ({1}/{2}){4}", c.Name, c.ProductsCount, c.TotalProductsCount,
                                        c.ProductsCount == 0 ? "<span class=\"lightlink\">" : string.Empty,
                                        c.ProductsCount == 0 ? "</span>" : string.Empty),
                                  MessageToDel =
                                      Server.HtmlEncode(string.Format(
                                          Resource.Admin_MasterPageAdminCatalog_Confirmation, c.Name)),
                                  Value = c.CategoryId.ToString(),
                                  NavigateUrl = "Catalog.aspx?CategoryID=" + c.CategoryId,
                                  TreeView = tree
                              };
            if (c.HasChild)
            {
                newNode.Expanded = false;
                newNode.PopulateOnDemand = true;
            }
            else
            {
                newNode.Expanded = true;
                newNode.PopulateOnDemand = false;
            }
            node.ChildNodes.Add(newNode);
        }
    }

    protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        LoadChildCategories(e.Node);
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Deletelink":
                string cat = Request["categoryid"] ?? CategoryId.ToString();
                CategoryService.DeleteCategoryAndLink(Convert.ToInt32(e.CommandArgument), Convert.ToInt32(cat));
                _needReloadTree = true;
                break;
            case "DeleteAll":
                ProductService.DeleteProduct(Convert.ToInt32(e.CommandArgument), true);
                _needReloadTree = true;
                break;
            case "DeleteCategory":
                CategoryService.DeleteCategoryAndPhotos(Convert.ToInt32(e.CommandArgument));
                break;
        }
        CategoryService.RecalculateProductsCountManual();
    }

    protected string RenderSplitter()
    {
        var str = new StringBuilder();
        str.Append("<td class=\'splitter\'  onclick=\'togglePanel();return false;\' >");
        str.Append("<div class=\'leftPanelTop\'></div>");
        switch (Resource.Admin_Catalog_SplitterLang)
        {
            case "rus":
                str.Append("<div id=\'divHide\' class=\'left_hide_rus\'></div>");
                str.Append("<div id=\'divShow\' class=\'left_show_rus\'></div>");
                break;
            case "eng":
                str.Append("<div id=\'divHide\' class=\'left_hide_en\'></div>");
                str.Append("<div id=\'divShow\' class=\'left_show_en\'></div>");
                break;
        }
        str.Append("</td>");
        return str.ToString();
    }

    protected string RenderTotalProductLink()
    {
        var res = new StringBuilder();
        res.Append("<div");

        res.Append(ShowMethod == EShowMethod.AllProducts
                       ? " style=\'font-weight:bold; color:#027dc2; text-decoration:none;\'>"
                       : ">");

        res.Append(Resource.Admin_Catalog_AllProducts);
        res.Append(" (");
        res.Append(CategoryService.GetTolatCounTofProducts());
        res.Append(")");
        res.Append("</div>");

        return res.ToString();
    }

    protected string RenderTotalProductWithoutCategoriesLink()
    {
        var res = new StringBuilder();
        res.Append("<div");

        res.Append(ShowMethod == EShowMethod.OnlyWithoutCategories
                       ? " style=\'font-weight:bold; color:#027dc2; text-decoration:none;\'>"
                       : ">");

        res.Append(Resource.Admin_Catalog_AllProductsWithoutCategories);
        res.Append(" (");
        res.Append(CategoryService.GetTolatCounTofProductsWithoutCategories());
        res.Append(")");
        res.Append("</div>");

        return res.ToString();
    }

    protected string RenderTotalProductInCategoriesLink()
    {
        var res = new StringBuilder();
        res.Append("<div");

        res.Append(ShowMethod == EShowMethod.OnlyInCategories
                       ? " style=\'font-weight:bold; color:#027dc2; text-decoration:none;\'>"
                       : ">");

        res.Append(Resource.Admin_Catalog_AllProductsInCategories);
        res.Append(" (");
        res.Append(CategoryService.GetTolatCounTofProductsInCategories());
        res.Append(")");
        res.Append("</div>");

        return res.ToString();
    }

    protected int GetCountOfCategoriesByProductID(int productID)
    {
        return ProductService.GetCountOfCategoriesByProductId(productID);
    }

    protected string RenderDivHeader()
    {
        string divHeader;
        if (Request.Browser.Browser == "IE")
        {
            var c = new CultureInfo("en-us");
            divHeader = double.Parse(Request.Browser.Version, c.NumberFormat) < 7 ? "<div class=\'mtree_ie6\'>" : "<div class=\'mtree_ie\'>";
        }
        else
        {
            divHeader = "<div class=\'mtree\'>";
        }
        return divHeader;
    }

    protected string RenderDivBottom()
    {
        return "</div>";
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem == null) return;
        var t = (DataRowView)e.Row.DataItem;
        if ((int)t["typeItem"] == 1)
        {
            if (ShowMethod == EShowMethod.Normal)
            {
                ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).HRef =
                    "Product.aspx?ProductID=" + t["Id"] + "&categoryid=" + CategoryId;
            }
            else
            {
                ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).HRef =
                    "Product.aspx?ProductID=" + t["Id"];
            }
            ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).Title =
                Resource.Admin_MasterPageAdminCatalog_Edit;
            ((LinkButton)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).CommandName =
                "DeleteAll";
            ((ConfirmButtonExtender)
             (e.Row.Cells[e.Row.Cells.Count - 1].FindControl("ConfirmButtonExtenderbuttonDelete"))).ConfirmText
                = string.Format(Resource.Admin_Product_ConfirmDeletingProduct, t["Name"]);

            e.Row.Attributes["rowType"] = "product";
            e.Row.Attributes["element_id"] = t["Id"].ToString();
            e.Row.Attributes["categoryId"] = CategoryId.ToString();

            var tr = new AsyncPostBackTrigger
                         {
                             ControlID = ((e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).UniqueID,
                             EventName = "Click"
                         };

            UpdatePanel1.Triggers.Add(tr);
        }
        if ((int)t["typeItem"] == 0)
        {
            ((e.Row.Cells[6].FindControl("txtPrice"))).Visible = false;
            ((e.Row.Cells[7].FindControl("txtAmount"))).Visible = false;

            ((HtmlAnchor)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("cmdlink"))).Title = Resource.Admin_MasterPageAdminCatalog_Edit;
            ((e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDeleteLink"))).Visible = false;
            ((LinkButton)(e.Row.Cells[e.Row.Cells.Count - 1].FindControl("buttonDelete"))).CommandName = "DeleteCategory";
            ((ConfirmButtonExtender)
             (e.Row.Cells[e.Row.Cells.Count - 1].FindControl("ConfirmButtonExtenderbuttonDelete"))).ConfirmText
                = string.Format(Resource.Admin_MasterPageAdminCatalog_Confirmation, t["Name"]);

            e.Row.Attributes["rowType"] = "category";
            e.Row.Attributes["element_id"] = t["Id"].ToString();

            e.Row.Style["cursor"] = "pointer";
        }
        if ((int)t["typeItem"] != -1) return;
        e.Row.Cells.Clear();
        var tbc1 = new TableCell();
        tbc1.Attributes.Add("colspan", "1");


        e.Row.Cells.Add(tbc1);

        var tbcFiller = new TableCell();
        tbcFiller.Attributes.Add("colspan", "1");
        var tbc1Tt = new HtmlGenericControl("span");
        var img = new HtmlImage { Src = "images/file_pdir.gif" };
        tbc1Tt.Controls.Add(img);

        tbcFiller.Controls.Add(tbc1Tt);
        e.Row.Controls.Add(tbcFiller);

        e.Row.Controls.Add(new TableCell());


        var tbc2 = new TableCell();
        tbc2.Attributes.Add("colspan", "7");

        var tbc2Tt = new HtmlGenericControl("span");
        var link = new HtmlAnchor
                       {
                           InnerText = Resource.Admin_Catalog_GoToUpperLevel
                       };
        link.Attributes["class"] = "goToUpperLevel";
        link.HRef = "Catalog.aspx?CategoryID=" + ((DataRowView)e.Row.DataItem)["ID"];
        tbc2Tt.Controls.Add(link);
        tbc2.Controls.Add(tbc2Tt);

        e.Row.Cells.Add(tbc2);
        e.Row.Style["Cursor"] = "pointer";
        e.Row.Attributes.Add("rowType", "goToUpperLevel");
        e.Row.Attributes.Add("element_id", ((DataRowView)e.Row.DataItem)["ID"].ToString());
    }

    protected string GetImageItem(int type, int id)
    {
        var abbr = "";
        if (type == 0)
        {
            var categoryPic = CategoryService.GetCategory(id).MiniPicture;
            if (categoryPic != null && File.Exists(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, categoryPic.PhotoName)))
            {
                abbr = FoldersHelper.GetImageCategoryPath(CategoryImageType.Small, categoryPic.PhotoName, false);
            }
            return string.Format("<img abbr='{0}' class='imgtooltip' src='{1}'>", abbr, "images/file_dir.gif");
        }

        if (type == 1)
        {
            var productPic = ProductService.GetProduct(id).Photo;
            if (File.Exists(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, productPic)))
            {
                abbr = FoldersHelper.GetImageProductPath(ProductImageType.Small, productPic, false);
                return string.Format("<img abbr='{0}' class='imgtooltip' src='{1}'>", abbr, "images/adv_photo_ico.gif");
            }
        }

        return "";
    }

    #region Перенос товаров в другую категорию

    protected void btnChangeProductCategory_Click(object sender, EventArgs e)
    {
        // Перенос продуктов в другую категорию
        if (!string.IsNullOrEmpty(tree2.SelectedValue) && tree2.SelectedValue != "0")
        {
            ChangeProductsCategory();
        }
    }

    /// <summary>
    /// Перенос выбранных товаров в другую категорию
    /// </summary>
    private void ChangeProductsCategory()
    {
        int categoryId = Convert.ToInt32(tree2.SelectedValue);

        if (!_inverseSelection)
        {
            if (_selectionFilter != null)
                foreach (var id in _selectionFilter.Values)
                {
                    int productId = Convert.ToInt32(id);


                    foreach (int catId in ProductService.GetCategoriesIDsByProductId(productId))
                    {
                        ProductService.DeleteProductLink(productId, catId);
                    }
                    ProductService.AddProductLink(productId, categoryId);
                }
        }
        else
        {
            switch (Request["categoryid"])
            {
                case "WithoutCategory":
                    foreach (int id in
                        ProductService.GetProductIDsWithoutCategory().Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        ProductService.AddProductLink(id, categoryId);
                    }
                    break;
                case "InCategories":
                    foreach (int id in
                        ProductService.GetProductIDsInCategories().Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        foreach (int catId in ProductService.GetCategoriesIDsByProductId(id))
                            ProductService.DeleteProductLink(id, catId);

                        ProductService.AddProductLink(id, categoryId);
                    }
                    break;
                case "AllProducts":
                    foreach (int id in
                        ProductService.GetProductsIDs().Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        foreach (int catId in ProductService.GetCategoriesIDsByProductId(id))
                            ProductService.DeleteProductLink(id, catId);

                        ProductService.AddProductLink(id, categoryId);
                    }
                    break;
                case "0":
                    foreach (int id in
                        ProductService.GetProductsIDs().Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        foreach (int catId in ProductService.GetCategoriesIDsByProductId(id))
                            ProductService.DeleteProductLink(id, catId);

                        ProductService.AddProductLink(id, categoryId);
                    }
                    break;
                default:
                    foreach (int id in
                        CategoryService.GetProductIDs(CategoryId).Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        foreach (int catId in ProductService.GetCategoriesIDsByProductId(id))
                            ProductService.DeleteProductLink(id, catId);

                        ProductService.AddProductLink(id, categoryId);
                    }
                    break;
            }
        }
        CategoryService.RecalculateProductsCountManual();
        //Response.Redirect("Catalog.aspx" + Request.Url.Query);
    }

    public void PopulateNode2(object sender, TreeNodeEventArgs e)
    {
        LoadChildCategories2(e.Node);
    }

    public void OnSelectedNodeChanged2(object sender, EventArgs e)
    {
        hhl2.Text = tree2.SelectedNode.Value;
        mpeTree2.Show();
    }


    private void LoadChildCategories2(TreeNode node)
    {
        foreach (var c in CategoryService.GetChildCategoriesByCategoryId(Convert.ToInt32(node.Value), false))
        {
            var newNode = new TreeNode
                              {
                                  Text = string.Format("{0} - ({1})", c.Name, c.ProductsCount),
                                  Value = c.CategoryId.ToString()
                              };
            if (c.HasChild)
            {
                newNode.Expanded = false;
                newNode.PopulateOnDemand = true;
            }
            else
            {
                newNode.Expanded = true;
                newNode.PopulateOnDemand = false;
            }
            node.ChildNodes.Add(newNode);
        }
    }

    protected void lbChangeCategory_Click(object sender, EventArgs e)
    {
        mpeTree2.Show();
    }

    #endregion
}