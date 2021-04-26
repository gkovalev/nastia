//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Repository;
using System.Web;
using AdvantShop.SEO;

public partial class Brands : AdvantShopPage
{
    const int BrandsPerPage = 8;

    List<Brand> dataSource = null;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
            SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Client_Brands_Header)), string.Empty);
        }
    }

    private void LoadData()
    {
        var brands = BrandService.GetBrands(false);
        var sb = new StringBuilder();
        var engLetters = BrandService.GetEngBrandChars();
        var rusLetters = BrandService.GetRusBrandChars();

        char selectedLetter = new char();

        ddlCountry.DataSource = CountryService.GetAllCountries().Where(c => brands.FindLast(b => b.CountryId == c.CountryID) != null);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem(Resources.Resource.Client_Brands_AllCoutries, "0"));

        if (Request["letter"] != null)
        {
            char.TryParse(Request["letter"].ToLower(), out selectedLetter);
            if (!engLetters.Contains(selectedLetter) && !rusLetters.Contains(selectedLetter) && selectedLetter != '0')
            {
                Error404();
            }
            else if (selectedLetter == '0')
            {
                dataSource = brands.Where(b => char.IsDigit(b.Name[0])).ToList();
            }
            else
            {
                dataSource = brands.Where(b => b.Name.ToLower().StartsWith(selectedLetter.ToString())).ToList();
            }

        }
        else if (Request["country"] != null)
        {
            ListItem item;
            string country = HttpUtility.HtmlDecode(Request["country"]);
            if ((item = ddlCountry.Items.FindByText(country)) == null)
            {
                Error404();
            }
            else
            {
                ddlCountry.SelectedValue = item.Value;
                dataSource = brands.Where(b => b.CountryId.ToString() == item.Value).ToList();
            }
        }
        else
        {
            dataSource = brands;
        }


        sb.AppendFormat("<a href=\"manufacturers\" class=\"all-letter{0}\">{1}</a>", Request["letter"] == null ? " simbol-selected" : string.Empty, Resources.Resource.Client_Brands_All);

        bool hasNumber = brands.Any(b => char.IsDigit(b.Name[0]));
        if (hasNumber)
            sb.AppendFormat("<a href='{0}' class='all-letter'>{1}</a> ", UrlService.GetAbsoluteLink("manufacturers?letter=" + "0"), "0-9");
        else
            sb.AppendFormat("<a class='all-letter disabled' href='javascript:void(0);'>{0}</a> ", "0-9");


        foreach (char ch in engLetters)
        {
            if (brands.Find(b => b.Name.ToLower().StartsWith(ch.ToString())) != null)
            {
                sb.AppendFormat("<a href='{0}'>{1}</a> ",
                    UrlService.GetAbsoluteLink("manufacturers?letter=" + ch), ch);
            }
            else
            {
                sb.AppendFormat("<a class='disabled' href='javascript:void(0);'>{0}</a> ", ch);
            }
        }
        lEngLetters.Text = sb.ToString();

        if (SettingsMain.Language == "ru-RU")
        {
            sb.Remove(0, sb.Length);
            foreach (char ch in rusLetters)
            {
                if (brands.Find(b => b.Name.ToLower().StartsWith(ch.ToString())) != null)
                {
                    sb.AppendFormat("<a href='{0}'>{1}</a> ",
                       UrlService.GetAbsoluteLink("manufacturers?letter=" + ch), ch);
                }
                else
                {
                    sb.AppendFormat("<a class='disabled' href='javascript:void(0);'>{0}</a> ", ch);
                }
            }
            lRusLetters.Text = sb.ToString();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (dataSource != null)
        {
            int page = paging.CurrentPage;
            paging.TotalPages = Convert.ToInt32(Math.Ceiling((double)dataSource.Count() / BrandsPerPage));
            if (paging.TotalPages < paging.CurrentPage || paging.CurrentPage <= 0)
            {
                Error404();
                return;
            }
            lvBrands.DataSource = dataSource.Skip((page - 1) * BrandsPerPage).Take(BrandsPerPage).ToList();
            lvBrands.DataBind();
        }
    }
}
