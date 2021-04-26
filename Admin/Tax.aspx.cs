using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Helpers;
using AdvantShop.Repository;
using AdvantShop.Taxes;

public partial class Admin_Tax : Page
{
    protected TaxElement CurrentTax = null;
    private SqlPaging _paging;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        var cats = new List<Category> { CategoryService.GetCategory(0) };
        Category[] rootCategories = cats.ToArray();
        CatalogDataTreeViewForTaxes.RootCategories = rootCategories;

        var taxIdStr = Request["taxid"];
        if (string.IsNullOrEmpty(taxIdStr))
        {
            Response.Redirect("Taxes.aspx", true);
            return;
        }
        int taxId;
        if (!Int32.TryParse(taxIdStr, out taxId))
        {
            Response.Redirect("Taxes.aspx", true);
            return;
        }

        CurrentTax = TaxServices.GetTax(taxId);

        Title = CurrentTax.Name;

        if (!IsPostBack)
        {
            _paging = new SqlPaging { TableName = "[Catalog].[TaxRegionRate] JOIN [Customers].[Region] ON [TaxRegionRate].[RegionID] = [Region].[RegionID]", ItemsPerPage = 10 };

            var f = new Field { Name = "TaxID" };
            var ff = new EqualFieldFilter { ParamName = "@TaxID", Value = CurrentTax.TaxId.ToString(CultureInfo.InvariantCulture) };
            f.Filter = ff;

            _paging.AddField(f);

            f = new Field { Name = "RegionName", Sorting = SortDirection.Ascending };
            _paging.AddField(f);
            grid.ChangeHeaderImageUrl("arrowName", f.Sorting == SortDirection.Ascending ? "images/arrowup.gif" : "images/arrowdown.gif");

            f = new Field { Name = "RegionRate" };
            _paging.AddField(f);

            f = new Field { Name = "[Catalog].[TaxRegionRate].RegionID as RegionID" };
            _paging.AddField(f);

            _paging.CurrentPageIndex = 1;

            ViewState["Paging"] = _paging;

            ddlDependsOnAddress.Items.Add(new ListItem(Resources.Resource.Admin_Taxes_DefaultAddress, ((int)TypeRateDepends.Default).ToString(CultureInfo.InvariantCulture)));
            ddlDependsOnAddress.Items.Add(new ListItem(Resources.Resource.Admin_Taxes_ShippingAddress, ((int)TypeRateDepends.ByShippingAddress).ToString(CultureInfo.InvariantCulture)));
            ddlDependsOnAddress.Items.Add(new ListItem(Resources.Resource.Admin_Taxes_BillingAddress, ((int)TypeRateDepends.ByBillingAddress).ToString(CultureInfo.InvariantCulture)));
            ddlDependsOnAddress.SelectedValue = ((int)CurrentTax.DependsOnAddress).ToString(CultureInfo.InvariantCulture);

            ddlRateType.Items.Add(new ListItem(Resources.Resource.Admin_Taxes_Fixed, ((int)RateType.LumpSum).ToString(CultureInfo.InvariantCulture)));
            ddlRateType.Items.Add(new ListItem(Resources.Resource.Admin_Taxes_Proportional, ((int)RateType.Proportional).ToString(CultureInfo.InvariantCulture)));
            ddlRateType.SelectedValue = ((int)CurrentTax.Type).ToString(CultureInfo.InvariantCulture);

            ddlCountry.DataSource = CountryService.GetAllCountries();
            DataBind();

            ddlCountry.SelectedValue = CurrentTax.CountryID.ToString(CultureInfo.InvariantCulture);
            ddlCountry_SelectedIndexChanged(ddlCountry, e);
            chkEnabled.Checked = CurrentTax.Enabled;
        }
        else
        {
            _paging = (SqlPaging)ViewState["Paging"];
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        grid.DataSource = _paging.PageItems;
        grid.DataBind();
    }

    protected void grid_Command(object sender, GridViewCommandEventArgs e)
    {
        MsgErrRegions(true);
        switch (e.CommandName)
        {
            case "AddNewRegion":
                {
                    var ddlRegionNew = (DropDownList) grid.FooterRow.FindControl("ddlRegionNew");
                    if (ValidationHelper.IsValidPositiveIntNumber(ddlRegionNew.SelectedValue))
                    {
                        var rate = new RegionalRate
                                       {
                                           RegionId = Int32.Parse(ddlRegionNew.SelectedValue)
                                       };
                        if (!TaxServices.GetRegionalRatesForTax(CurrentTax.TaxId).Any(rr => rr.RegionId == rate.RegionId))
                        {
                            decimal t;
                            rate.Rate = Decimal.TryParse(
                                ((TextBox) grid.FooterRow.FindControl("txtRegionRateNew")).Text,
                                out t)
                                            ? t
                                            : 0;
                            if (!TaxServices.AddRegionRateToTax(CurrentTax.TaxId, rate))
                            {
                                MsgErrRegions(Resources.Resource.Admin_Tax_RegionRateError);
                            }
                        }
                    }
                    regionsPanel.Update();
                    //grid.ShowFooter = false;

                }
                break;

            case "CancelAdd":
                grid.ShowFooter = false;
                regionsPanel.Update();
                break;

            case "Delete":
                TaxServices.RemoveRegionalRateFromTax(CurrentTax.TaxId, Int32.Parse((string)e.CommandArgument));
                break;

            case "Update":
                {
                    var regionId = Int32.Parse(((HiddenField)grid.Rows[Int32.Parse(((string)e.CommandArgument))].FindControl("hdRegionId")).Value);
                    decimal rate = 0;
                    decimal t;
                    if (Decimal.TryParse(((TextBox)grid.Rows[Int32.Parse(((string)e.CommandArgument))].FindControl("txtRegionRate")).Text, out t))
                    {
                        rate = t;
                    }
                    else
                    {
                        rate = 0;
                    }
                    TaxServices.UpdateRegionalRate(CurrentTax.TaxId, regionId, rate);
                }
                break;

            case "Sort":
                {
                    _paging.Fields.Values.Where(x => x.Name != (string)e.CommandArgument).ToList().ForEach(x => x.Sorting = null);
                    switch ((string)e.CommandArgument)
                    {
                        case "RegionName":
                            {
                                _paging.Fields["RegionName"].Sorting = !_paging.Fields["RegionName"].Sorting.HasValue ?
                                        SortDirection.Ascending :
                                        (_paging.Fields["RegionName"].Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending);
                                grid.ChangeHeaderImageUrl("arrowName", _paging.Fields["RegionName"].Sorting == SortDirection.Ascending ? "images/arrowup.gif" : "images/arrowdown.gif");
                            }
                            break;
                        case "RegionRate":
                            {
                                _paging.Fields["RegionRate"].Sorting = !_paging.Fields["RegionRate"].Sorting.HasValue ?
                                        SortDirection.Ascending :
                                        (_paging.Fields["RegionRate"].Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending);
                                grid.ChangeHeaderImageUrl("arrowRate", _paging.Fields["RegionRate"].Sorting == SortDirection.Ascending ? "images/arrowup.gif" : "images/arrowdown.gif");
                            }
                            break;
                    }
                }
                break;
        }
    }
    
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        e.Cancel = true;
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RegionService.GetRegions(ddlCountry.SelectedValue).Count == 0)
        {
            CurrentTax.RegionalRates.Clear();
            grid.Visible = false;
            regionsPanel.Update();
            formPanel.Update();
        }
        else
        {
            grid.Visible = true;
            List<AjaxControlToolkit.CascadingDropDownNameValue> regions =
                RegionService.GetRegions(ddlCountry.SelectedValue);
            var fordelete = new List<RegionalRate>();
            foreach (var rate in
                CurrentTax.RegionalRates.Where(rate => regions.Where(reg => reg.value == rate.RegionId.ToString(CultureInfo.InvariantCulture)).Count() == 0))
            {
                fordelete.Add(rate);
            }
            foreach (var regionalRate in fordelete)
            {
                CurrentTax.RegionalRates.Remove(regionalRate);
            }
            TaxServices.UpdateTax(CurrentTax);
            regionsPanel.Update();
            formPanel.Update();
        }
    }

    private void MsgErr(string messageText)
    {
        Message.Visible = true;
        Message.ForeColor = Color.Red;
        Message.Text = "<br/>" + messageText;
    }

    private void MsgErrRegions(string messageText)
    {
        MessageRegions.Visible = true;
        MessageRegions.ForeColor = Color.Red;
        MessageRegions.Text = "<br/>" + messageText;
    }
    private void MsgErrRegions(bool clear)
    {
        MessageRegions.Text = string.Empty;
        MessageRegions.Visible = false;
    }
    private void MsgErr(bool clear)
    {
        Message.Text = string.Empty;
        Message.Visible = false;
    }

    private void Msg(string messageText)
    {
        Message.Visible = true;
        Message.ForeColor = Color.Blue;
        Message.Text = @"<br/>" + messageText;
    }

    protected void saveClick(object sender, EventArgs e)
    {
        MsgErr(false);
        CurrentTax.Name = txtName.Text;
        var cid = Int32.Parse(ddlCountry.SelectedValue);
        if (CurrentTax.CountryID != cid)
        {
            CurrentTax.RegionalRates.Clear();
        }
        CurrentTax.Enabled = chkEnabled.Checked;
        CurrentTax.CountryID = cid;
        int priority = 0;
        if (int.TryParse(txtPriority.Text, out priority))
        {
            CurrentTax.Priority = priority;
        }
        else
        {
            CurrentTax.Priority = 1;
            txtPriority.Text = "1";
        }
        decimal fedRate = 0;
        if (Decimal.TryParse(txtFederalRate.Text, out fedRate))
        {
            CurrentTax.FederalRate = fedRate;
        }
        else
        {
            CurrentTax.FederalRate = 0;
            txtFederalRate.Text = @"0.00";
        }

        CurrentTax.DependsOnAddress = ((TypeRateDepends)(Int32.Parse(ddlDependsOnAddress.SelectedValue)));
        CurrentTax.RegNumber = txtRegNumber.Text;
        CurrentTax.ShowInPrice = chkShowInPrice.Checked;
        CurrentTax.Type = (RateType)(Int32.Parse(ddlRateType.SelectedValue));
        try
        {
            TaxServices.UpdateTax(CurrentTax);
            Msg(Resources.Resource.Admin_Tax_Saved);
            formPanel.Update();
        }
        catch (Exception ex)
        {
            MsgErr(ex.Message);
            return;
        }

        // Mapping on product ----------------------------------------------------

        var sc = CatalogDataTreeViewForTaxes.SelectedCategories;

        if (sc.Count == 0)
        {
            TaxLasyLoad.SetCategorySelectionState("0", CurrentTax.TaxId, false, true);
        }
        foreach (var selectedCategory in sc)
        {
            int categoryID = selectedCategory.Category.CategoryId;
            var hasSelectedSubCats =
                sc.Where(cat => cat.Category.CategoryId != selectedCategory.Category.CategoryId).Any(c => c.Category.ParentCategoryId == categoryID);
            var hasSelectedProducts = false;
            foreach (Product product1 in
                CatalogDataTreeViewForTaxes.SelectedProducts.Where(product1 => CategoryService.GetProductsByCategoryId(categoryID).Where(p => p.ProductId == product1.ProductId).Count() > 0))
            {
                hasSelectedProducts = true;
            }
            if (selectedCategory.Full)
            {
                //sp.AddRange(selectedCategory.Category.Products);
                TaxLasyLoad.SetCategorySelectionState(selectedCategory.Category.CategoryId.ToString(CultureInfo.InvariantCulture), CurrentTax.TaxId, true, true);
            }
            else
            {
                if (hasSelectedSubCats)
                {
                    //Выполняется очистка выбранных категорий и продуктов для текущей категории
                    TaxLasyLoad.SetCategorySelectionState(selectedCategory.Category.CategoryId.ToString(CultureInfo.InvariantCulture), CurrentTax.TaxId, false, true);
                    //а затем сохранение состояния только для текущей категории
                    TaxLasyLoad.SetCategorySelectionState(selectedCategory.Category.CategoryId.ToString(CultureInfo.InvariantCulture), CurrentTax.TaxId, true, false);
                    //остальные выбранные категории будут сохранены на следующих итерация, а продукты в следующем цикле
                }
                else if (hasSelectedProducts)
                {
                    TaxLasyLoad.SetCategorySelectionState(selectedCategory.Category.CategoryId.ToString(CultureInfo.InvariantCulture), CurrentTax.TaxId, true, false);
                }

            }
        }
        
        foreach (Product selectedProduct in CatalogDataTreeViewForTaxes.SelectedProducts)
        {
            TaxLasyLoad.SetProductSelectionState(selectedProduct.ProductId, CurrentTax.TaxId);
        }
    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = AdvantShop.Connection.GetConnectionString();
    }
}