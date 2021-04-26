//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using AdvantShop.Core;
using AdvantShop.Catalog;
using AdvantShop.Taxes;

public partial class UserControls_CatalogDataTreeViewForTaxes : UserControl
{
    private readonly List<string> _selectedIds;

    private Category[] _rootCategories;

    private const int _selected = 0;
    private List<TaxCategorySelectionState> _selectedCategories;

    private List<Product> _selectedProducts;

    public UserControls_CatalogDataTreeViewForTaxes()
    {
        _selectedIds = new List<string>();
    }

    public Category[] RootCategories
    {
        get { return _rootCategories; }
        set { _rootCategories = value; }
    }

    public bool ClearSelections { get; set; }

    public List<Product> SelectedProducts
    {
        get
        {
            if (_selectedProducts == null)
            {
                _selectedProducts = new List<Product>();
                foreach (var key in Request.Form.AllKeys)
                {
                    var regexp = new Regex(".*_(.*)_([a-zA-Z0-9]*)_Product");
                    var match = regexp.Match(key);
                    if (!match.Success) continue;
                    var product = new Product { ProductId = Convert.ToInt32(match.Groups[2].Value) };
                    _selectedProducts.Add(product);
                }
            }

            return _selectedProducts;
        }
        set
        {
            _selectedProducts = new List<Product>();
            _selectedProducts.AddRange(value);
        }
    }

    public List<TaxCategorySelectionState> SelectedCategories
    {
        get
        {
            if (_selectedCategories == null)
            {
                _selectedCategories = new List<TaxCategorySelectionState>();
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key != null)
                    {
                        var regexp = new Regex("_*([a-zA-Z0-9]*)_([a-zA-Z0-9]+)_Category$");
                        Match match = regexp.Match(key);
                        if (match.Success)
                        {
                            var category = new Category
                                               {
                                                   CategoryId = Convert.ToInt32(match.Groups[2].Value),
                                                   ParentCategoryId = string.IsNullOrEmpty(match.Groups[1].Value) ? 0 : Convert.ToInt32(match.Groups[1].Value)
                                               };
                            string state = Request.Params[key + "_State"];
                            var ss = new TaxCategorySelectionState { Category = category, Full = state == "full" };
                            _selectedCategories.Add(ss);
                        }
                    }
                }
            }
            return _selectedCategories;
        }
        set
        {
            _selectedCategories = new List<TaxCategorySelectionState>();
            _selectedCategories.AddRange(value);
        }
    }

    private string[] SelectedIds
    {
        get
        {
            if (_selectedIds.Count == 0)
            {
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key != null)
                    {
                        var regexp = new Regex(".*_([a-zA-Z0-9]*)_(Product|Category)");
                        Match match = regexp.Match(key);
                        if (match.Success)
                        {
                            _selectedIds.Add(match.Groups[1].Value);
                        }
                    }
                }
            }

            return _selectedIds.ToArray();
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        TotalRecordsLiteral.InnerText = ProductService.GetProductsCount().ToString();
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ProcessData(_rootCategories);
    }

    protected override void Render(HtmlTextWriter writer)
    {
        TotalProductSelectedLiteral.InnerText = _selected.ToString();

        base.Render(writer);
    }

    private void ProcessData(IEnumerable<Category> rootCategories)
    {
        foreach (var rc in rootCategories)
        {
            TaxLasyLoad.ProcessCategoryRow(rc, 0, -1, DataTable, false, Convert.ToInt32(Request["taxid"]));
        }
    }
    public void ClearSelect()
    {
        _selectedIds.Clear();
    }
}