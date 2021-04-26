using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.SEO;
using AdvantShop.Taxes;
using Resources;
using AdvantShop.Core;
using System.Data.SqlClient;
using System.Data;

public partial class Admin_Product : AdvantShopPage
{
    private bool _valid = true;
    private Product _product;
    private string _productPhoto;


    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }


    private int ProductId
    {
        get
        {
            int id;
            int.TryParse(Request["productid"], out id);
            return id;
        }
    }

    private int CategoryID
    {
        get
        {
            var id = CategoryService.DefaultNonCategoryId;
            if (!int.TryParse(Request["categoryid"], out id))
            {
                id = ProductService.GetFirstCategoryIdByProductId(ProductId);
            }
            return id;
        }
    }

    protected bool AddingNewProduct
    {
        //get { return (ProductId == 0); }
        get { return string.IsNullOrEmpty(Request["productid"]); }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        fckBriefDescription.Language = fckDescription.Language = CultureInfo.CurrentCulture.ToString();
        AdvantShop.Helpers.CommonHelper.DisableBrowserCache();
        if (AdvantShop.Localization.Culture.Language == AdvantShop.Localization.Culture.ListLanguage.Russian)
        {
            slimboxStyle.Attributes["href"] = UrlService.GetAdminAbsoluteLink("/css/slimbox2_ru.css");
        }
        if (string.IsNullOrEmpty(Request["categoryid"]) && string.IsNullOrEmpty(Request["productid"]))
        {
            Page.Response.Redirect("Catalog.aspx");
        }

        Page.Title = string.Format("{0}", SettingsMain.ShopName);
        relatedProducts.ProductID = alternativeProducts.ProductID = productPhotos.ProductID = productVideos.ProductID
            = productCustomOption.ProductId = productProperties.ProductId = rightNavigation.ProductID = ProductId;
        rightNavigation.CategoryID = CategoryID;

        lRelatedProduct.Text = SettingsCatalog.RelatedProductName;
        lAlternativeProduct.Text = SettingsCatalog.AlternativeProductName;

        if (!IsPostBack)
        {
            if (!AddingNewProduct)
            {
                _product = ProductService.GetProduct(ProductId);
                if (_product == null)
                {
                    Response.Redirect("Catalog.aspx");
                    return;
                }
                LoadProduct(_product);
            }
            else
            {
                _product = new Product
                {
                    Name = Resource.Admin_Product_AddNewProduct,
                    Offers = new List<Offer> { new Offer() }
                };
                txtTitle.Text = string.Empty;
                txtMetaKeywords.Text = string.Empty;
                txtMetaDescription.Text = string.Empty;

                LoadProduct(_product);
            }

            UpdateMainPhoto();
            LoadSiteNavigation();

            txtName.Focus();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        var brand = popUpBrand.SelectBrandId != 0 ? BrandService.GetBrandById(popUpBrand.SelectBrandId) : null;
        lBrand.Text = brand != null ? brand.Name : Resource.Admin_Product_NotSelected;
        ibRemoveBrand.Visible = popUpBrand.SelectBrandId != 0;

        if (!AddingNewProduct)
        {
            aToClient.HRef = "../" + UrlService.GetLinkDB(ParamType.Product, ProductId);
            aToClient.Visible = true;
        }
        else
        {
            aToClient.Visible = false;
        }
    }

    private void LoadSiteNavigation()
    {
        if (CategoryID == CategoryService.DefaultNonCategoryId)
        {
            sn.Visible = false;
            Localize_Admin_Catalog_CategoryLocation.Visible = false;
        }
        else
        {
            sn.Visible = true;
            Localize_Admin_Catalog_CategoryLocation.Visible = true;
            sn.BuildNavigationAdmin(CategoryID);
        }
    }

    private void LoadProduct(Product product)
    {
        lProductName.Text = product.Name;
        lbIsProductActive.ForeColor = _product.Enabled
                                          ? System.Drawing.Color.FromArgb(2, 125, 194)
                                          : System.Drawing.Color.Red;
        lbIsProductActive.Visible = !AddingNewProduct;
        lbIsProductActive.Text = _product.Enabled
                                     ? Resource.Admin_m_Product_Active
                                     : Resource.Admin_Catalog_ProductDisabled;
        btnSave.Text = AddingNewProduct
                           ? Resource.Admin_Product_AddProduct
                           : Resource.Admin_Product_Save;
        btnAdd.Visible = !AddingNewProduct;

        if (product.ProductId != 0)
        {
            btnAdd.OnClientClick = string.Format("window.location=\'Product.aspx?CategoryID={0}\';return false;", CategoryID > 0 ? CategoryID : 0);
            lblProductId.Text = product.ProductId.ToString();
            txtStockNumber.Text = product.ArtNo;
            txtName.Text = product.Name;
            txtSynonym.Text = product.UrlPath;
            chkEnabled.Checked = product.Enabled;
            chkOrderByRequest.Checked = product.OrderByRequest;
            txtWeight.Text = product.Weight.ToString();

            var temp = product.Size.Split('|');
            if (temp.Length == 3)
            {
                txtSizeLength.Text = temp[0];
                txtSizeWidth.Text = temp[1];
                txtSizeHeight.Text = temp[2];
            }
            else txtSizeLength.Text = string.IsNullOrEmpty(product.Size) ? "0" : product.Size;

            popUpBrand.SelectBrandId = product.BrandId;
            lBrand.Text = product.BrandId == 0 ? Resource.Admin_Product_NotSelected : product.Brand.Name;

            chkBestseller.Checked = product.BestSeller;
            chkRecommended.Checked = product.Recomended;
            chkNew.Checked = product.New;
            chkOnSale.Checked = product.OnSale;
            var flagEnabled = ProductService.GetCountOfCategoriesByProductId(product.ProductId) > 0;
            chkBestseller.Enabled = flagEnabled;
            chkRecommended.Enabled = flagEnabled;
            chkNew.Enabled = flagEnabled;
            chkOnSale.Enabled = flagEnabled;
            lblMarkersDisabled.Visible = !flagEnabled;

            txtSupplyPrice.Text = product.Offers.First().SupplyPrice.ToString("#0.00") ?? "0";
            txtPrice.Text = product.Offers.First().Price.ToString("#0.00") ?? "0";
            txtShippingPrice.Text = product.Offers.First().ShippingPrice.ToString("#0.00") ?? "0";
            txtAmount.Text = product.Offers.First().Amount.ToString();
            txtUnit.Text = product.Offers.First().Unit;

            txtMaxAmount.Text = product.Offers.First().MaxAmount.ToString();
            txtMinAmount.Text = product.Offers.First().MinAmount.ToString();
            txtMultiplicity.Text = product.Offers.First().Multiplicity.ToString();
            
            txtDiscount.Text = product.Discount.ToString();
            fckDescription.Text = product.Description;
            fckBriefDescription.Text = product.BriefDescription;

            var meta = MetaInfoService.GetMetaInfo(product.ProductId, MetaType.Product);
            if (meta == null)
            {
                _product.Meta = new MetaInfo(0, 0, MetaType.Product, string.Empty, string.Empty, string.Empty);
                chbDefaultMeta.Checked = true;
            }
            else
            {
                chbDefaultMeta.Checked = false;
                _product.Meta = meta;
                txtTitle.Text = _product.Meta.Title;
                txtMetaKeywords.Text = _product.Meta.MetaKeywords;
                txtMetaDescription.Text = _product.Meta.MetaDescription;
            }

            LoadCategoryTree();
        }
    }

    protected string GetPageTitle()
    {
        return string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Product_SubHeader);
    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
    }

    private void Msg(string messageText)
    {
        lMessage.Visible = true;
        lMessage.Text = messageText;
    }

    #region Photos
    protected string HtmlProductImage()
    {
        return string.IsNullOrEmpty(_productPhoto)
                   ? "<img style=\'margin-right: 30px; border: solid 1px gray;\' src=\'images/nophoto.gif\' />"
                   : (_productPhoto.Contains("://")
                          ? "<a rel=\"lightbox\" href=\"" + _productPhoto +
                            "\"><img style=\'margin-right: 30px; border: solid 1px gray;\' width=\'120\' src=\'" +
                            _productPhoto + "\' /></a>"
                          : "<a rel=\"lightbox\" href=\"" +
                            FoldersHelper.GetImageProductPath(ProductImageType.Big, _productPhoto, true) +
                            "\"><img style=\'margin-right: 30px; border: solid 1px gray;\' src=\'" +
                            FoldersHelper.GetImageProductPath(ProductImageType.Small, _productPhoto, true) + "\' /></a>");
    }
    protected void productPhotos_OnPhotoMessage(object sender, Admin_UserControls_ProductPhotos.PhotoMessageEventArgs e)
    {
        Msg(e.Message);
    }

    protected void productPhotos_OnMainPhotoUpdate(object sender, EventArgs e)
    {
        UpdateMainPhoto();
    }

    protected void UpdateMainPhoto()
    {
        var product = ProductService.GetProduct(ProductId);
        _productPhoto = product == null ? null : product.Photo;
        ltPhoto.Text = HtmlProductImage();
        upPhoto.Update();
    }
    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {
        _valid = ValidateInput();

        string redir = null;
        txtPrice.Text = txtPrice.Text.Replace(" ", string.Empty);
        txtSupplyPrice.Text = txtSupplyPrice.Text.Replace(" ", string.Empty);
        txtShippingPrice.Text = txtShippingPrice.Text.Replace(" ", string.Empty);

        if (AddingNewProduct)
        {
            var id = CreateProduct();
            var catId = 0;
            if (id != 0 && int.TryParse(Request["categoryid"], out catId) && catId > 0)
            {
                if (CategoryService.IsExistCategory(catId))
                {
                    ProductService.EnableDynamicProductLinkRecalc();
                    ProductService.AddProductLink(id, catId);
                    ProductService.DisableDynamicProductLinkRecalc();
                    ProductService.SetProductHierarchicallyEnabled(id);
                }
            }

            redir = id == 0 ? null : string.Format("Product.aspx?ProductID={0}{1}", id, catId == 0 ? "" : "&CategoryID=" + Request["categoryid"]);
        }
        else
        {
            if (string.IsNullOrEmpty(txtStockNumber.Text.Trim()))
            {
                lStockNumberError.Text = Resource.Admin_Product_ArtNoEmpty;
                return;
            }
            UpdateProduct();
        }

        if (!string.IsNullOrEmpty(redir))
        {
            Response.Redirect(redir);
        }
    }

    private int CreateProduct()
    {
        int id = 0;
        var artNo = txtStockNumber.Text;
        bool validArtNo = true;
        try
        {
            // проверяем свободен ли артикул
            if (ProductService.GetProductId(artNo) != 0)
            {
                validArtNo = false;
                Msg(Resource.Admin_Product_Duplicate);
            }

            Validate();
            //проверяем свободен ли урл
            if (!UrlService.IsAvalibleUrl(ParamType.Product, txtSynonym.Text))
            {
                validArtNo = false;
                Msg(Resource.Admin_SynonymExist);
            }

            imgExcl1.Visible = !IsValidTab(1) || !validArtNo;
            imgExcl2.Visible = !IsValidTab(2);
            if (IsValid && validArtNo)
            {
                ProductService.EnableDynamicProductLinkRecalc();
                var prod = GetProductFromForm();
                id = ProductService.AddProduct(prod, true);
                ProductService.DisableDynamicProductLinkRecalc();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            Msg("Erorr at create product");
            return 0;
        }
        return id;
    }

    private void UpdateProduct()
    {
        bool validArtNo = true;
        var artNo = txtStockNumber.Text;
        try
        {
            //проверяем свободен ли артикул
            var tempId = ProductService.GetProductId(artNo);
            if (tempId != 0 && tempId != ProductId)
            {
                validArtNo = false;
                Msg(Resource.Admin_Product_Duplicate);
            }

            Validate();
            if (IsValidTab(1))
            {
                var synonym = txtSynonym.Text;
                if (!string.IsNullOrEmpty(synonym))
                {
                    if (!UrlService.IsAvalibleUrl(ProductId, ParamType.Product, synonym))
                    {
                        Msg(Resource.Admin_SynonymExist);
                        return;
                    }
                }
            }
            imgExcl1.Visible = !IsValidTab(1) || !validArtNo || !IsValidTab(3);
            imgExcl2.Visible = !IsValidTab(2);

            imgExcl6.Visible = !_valid;

            if (IsValid && _valid && validArtNo)
            {
                ProductService.EnableDynamicProductLinkRecalc();
                ProductService.UpdateProduct(GetProductFromForm(), true);
                SaveTaxesByProductFromDatalist();
                ProductService.DisableDynamicProductLinkRecalc();
                productCustomOption.SaveCustomOption();
                _product = ProductService.GetProduct(ProductId);
                LoadProduct(_product);
                UpdateMainPhoto();
            }

            LoadSiteNavigation();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private Product GetProductFromForm()
    {
        _product = AddingNewProduct
            ? new Product { Meta = new MetaInfo(), Offers = new List<Offer>() }
            : ProductService.GetProduct(ProductId);
        _product.ArtNo = txtStockNumber.Text;
        _product.Name = txtName.Text;
        _product.UrlPath = txtSynonym.Text;
        _product.BriefDescription = fckBriefDescription.Text == "<br />" || fckBriefDescription.Text == "&nbsp;" || fckBriefDescription.Text == "\r\n"
                                        ? string.Empty
                                        : fckBriefDescription.Text;

        _product.Description = fckDescription.Text == "<br />" || fckDescription.Text == "&nbsp;" || fckDescription.Text == "\r\n"
                                   ? string.Empty
                                   : fckDescription.Text;

        _product.Weight = Convert.ToDecimal(txtWeight.Text);
        _product.Size = txtSizeLength.Text + "|" + txtSizeWidth.Text + "|" + txtSizeHeight.Text;
        _product.Discount = Convert.ToDecimal(txtDiscount.Text);
        _product.Enabled = chkEnabled.Checked;
        _product.OrderByRequest = chkOrderByRequest.Checked;

        _product.BestSeller = chkBestseller.Checked;
        _product.Recomended = chkRecommended.Checked;
        _product.New = chkNew.Checked;
        _product.OnSale = chkOnSale.Checked;
        _product.BrandId = popUpBrand.SelectBrandId;
        if (_product.Offers.Count == 0)
        {
            var off = new Offer
                          {
                              ProductId = _product.ProductId,
                              OfferListId = 6,
                              Amount = Convert.ToInt32(txtAmount.Text),
                              Price = Convert.ToDecimal(txtPrice.Text),
                              ShippingPrice = Convert.ToDecimal(txtShippingPrice.Text),
                              Unit = txtUnit.Text,
                              SupplyPrice = Convert.ToDecimal(txtSupplyPrice.Text),
                              Multiplicity = Convert.ToInt32(txtMultiplicity.Text),
                              MaxAmount = txtMaxAmount.Text.IsNotEmpty() ? Convert.ToInt32(txtMaxAmount.Text) : (int?)null,
                              MinAmount = txtMinAmount.Text.IsNotEmpty() ? Convert.ToInt32(txtMinAmount.Text) : (int?)null
                          };
            _product.Offers.Add(off);
        }
        else
        {
            _product.Offers.First().Amount = Convert.ToInt32(txtAmount.Text);
            _product.Offers.First().Price = Convert.ToDecimal(txtPrice.Text);
            _product.Offers.First().ShippingPrice = Convert.ToDecimal(txtShippingPrice.Text);
            _product.Offers.First().Unit = txtUnit.Text;
            _product.Offers.First().SupplyPrice = Convert.ToDecimal(txtSupplyPrice.Text);
           
            _product.Offers.First().Multiplicity = Convert.ToInt32(txtMultiplicity.Text);
            _product.Offers.First().MaxAmount = txtMaxAmount.Text.IsNotEmpty()
                                                    ? Convert.ToInt32(txtMaxAmount.Text)
                                                    : (int?) null;
            _product.Offers.First().MinAmount = txtMinAmount.Text.IsNotEmpty()
                                                    ? Convert.ToInt32(txtMinAmount.Text)
                                                    : (int?) null;
        }

        _product.Meta.Title = txtTitle.Text;
        _product.Meta.MetaDescription = txtMetaDescription.Text;
        _product.Meta.MetaKeywords = txtMetaKeywords.Text;
        _product.Meta.Type = MetaType.Product;
        _product.Meta.ObjId = _product.ProductId;
        return _product;
    }

    void SaveTaxesByProductFromDatalist()
    {
        for (int i = 0; i < datalistTaxes.Items.Count; i++)
        {
            int taxId = 0;
            Int32.TryParse(((HiddenField)(datalistTaxes.Items[i].FindControl("hfTaxId"))).Value, out taxId);
            if (taxId != 0)
                TaxServices.SwitchProductTax(ProductId, taxId, ((CheckBox)(datalistTaxes.Items[i].FindControl("chbTax"))).Checked, true);
        }
    }

    protected bool ValidateInput()
    {
        return _valid;
    }

    protected bool IsValidTab(int tab)
    {
        return
            (from BaseValidator v in Validators where v.ValidationGroup.Equals(tab.ToString()) && !v.IsValid select v).
                ToArray().Length == 0;
    }

    protected void IsValidTabb(int tab)
    {
        lSize.Visible = (from BaseValidator v in Validators where v.ValidationGroup.Equals(tab.ToString()) && !v.IsValid select v).ToArray().Length == 0;
    }

    #region CategoryTree

    protected void LoadCategoryTree()
    {
        if (!IsPostBack)
        {
            var node = new TreeNode { Text = Resource.Admin_m_Category_Root, Value = @"0", Selected = true, SelectAction = TreeNodeSelectAction.None };
            LinksProductTree.Nodes.Add(node);
            LoadChildCategories(LinksProductTree.Nodes[0]);
            FillListBox();
        }
    }

    protected void btnDelLink_Click(object sender, EventArgs e)
    {
        int categoryId;
        if (!string.IsNullOrEmpty(ListlinkBox.SelectedValue) && int.TryParse(ListlinkBox.SelectedValue, out categoryId))
        {
            ProductService.DeleteProductLink(ProductId, categoryId);
            CategoryService.RecalculateProductsCountManual();
            FillListBox();
        }
    }
    protected void lnAddLink_Click(object sender, EventArgs e)
    {
        if ((LinksProductTree.SelectedValue != null) && LinksProductTree.SelectedValue != "0" && LinksProductTree.SelectedValue.IsInt())
        {
            int temp;
            Int32.TryParse(LinksProductTree.SelectedValue, out temp);
            if (temp != 0)
            {
                ProductService.EnableDynamicProductLinkRecalc();
                ProductService.AddProductLink(ProductId, temp);
                ProductService.DisableDynamicProductLinkRecalc();
                ProductService.SetProductHierarchicallyEnabled(ProductId);
            }
            FillListBox();
        }
    }
    public void PopulateNode(object sender, TreeNodeEventArgs e)
    {
        LoadChildCategories(e.Node);
    }

    private void LoadChildCategories(TreeNode node)
    {
        foreach (Category c in CategoryService.GetChildCategoriesByCategoryId(Convert.ToInt32(node.Value), false))
        {
            if (c.CategoryId != Convert.ToInt32(Request["id"]))
            {
                var newNode = new TreeNode { Text = string.Format("{0}", c.Name), Value = c.CategoryId.ToString() };
                if (c.HasChild)
                {
                    newNode.Expanded = false;
                    newNode.PopulateOnDemand = true;
                    //newNode.ShowCheckBox = true;
                    //newNode.NavigateUrl = "javascript:void(0)";
                }
                else
                {
                    newNode.Expanded = true;
                    newNode.PopulateOnDemand = false;

                    //newNode.ShowCheckBox = true;
                    //newNode.NavigateUrl = "javascript:void(0)";
                }
                node.ChildNodes.Add(newNode);
            }
        }
    }
    public void FillListBox()
    {
        ListlinkBox.Items.Clear();
        try
        {
            foreach (var catId in ProductService.GetCategoriesIDsByProductId(ProductId))
            {
                var item = new ListItem();

                IList<Category> parentCategories = CategoryService.GetParentCategories(catId);

                var way = new StringBuilder();
                for (int i = parentCategories.Count - 1; i >= 0; i--)
                {
                    if (way.Length == 0)
                    {
                        way.Append(parentCategories[i].Name);
                    }
                    else
                    {
                        way.Append(" > " + parentCategories[i].Name);
                    }
                }
                if (ProductService.IsMainLink(ProductId, catId))
                    way.AppendMany(" (", Resource.Admin_Product_MainCategory, ")");
                item.Text = way.ToString();
                item.Value = catId.ToString();
                ListlinkBox.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            //Debug.LogError(ex, ProductId);
            Debug.LogError(ex);
        }
    }
    #endregion

    protected void btnMainLink_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ListlinkBox.SelectedValue)) return;
        ProductService.SetMainLink(ProductId, Convert.ToInt32(ListlinkBox.SelectedValue));
        //RouteService.DeleteFromCache(ProductId, ParamType.Product);
        FillListBox();
    }

    protected void ibRemoveBrand_Click(object sender, EventArgs e)
    {
        popUpBrand.SelectBrandId = 0;
        ibRemoveBrand.Visible = false;

        if (Request["ProductID"].TryParseInt() != 0)
        {
            ProductService.DeleteBrand(Request["ProductID"].TryParseInt());
        }

    }
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        string redir = null;
        
    }
    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        //Added by Evgeni
        string redir = null;

        var artNo = txtStockNumber.Text;
        var artDestNo = artNo + "_NEW";

        var ProductId = 0;
        int.TryParse(Request["ProductId"], out ProductId);
        //ExecSP
       ProductId =   (int) SQLDataAccess.ExecuteScalar("[Catalog].[sp_Evgeni_CopyProduct]", CommandType.StoredProcedure,
                                            new SqlParameter("@sourceArt", artNo),
                                            new SqlParameter("@destArt", artDestNo));
        //
        redir = string.Format("Product.aspx?ProductID={0}", ProductId);


        if (!string.IsNullOrEmpty(redir))
        {
            Response.Redirect(redir);
        }
    }
}