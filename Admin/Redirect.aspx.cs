//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.SEO;
using Resources;

public partial class Admin_about : Page
{
    private SqlPaging _Paging;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_MasterPageAdmin_SEO_redirects);

        if (!IsPostBack)
        {
            _Paging = new SqlPaging
                          {
                              TableName = "[Settings].[Redirect]",
                              ItemsPerPage = 1000
                          };

            _Paging.AddFieldsRange(
                new List<Field>
                    {
                        new Field {Name = "ID", IsDistinct = true},
                        new Field {Name = "RedirectFrom"},
                        new Field {Name = "RedirectTo"}
                    });

            _Paging.ItemsPerPage = 1000;

            _Paging.CurrentPageIndex = 1;
            ViewState["Paging"] = _Paging;
        }
        else
        {
            _Paging = (SqlPaging)(ViewState["Paging"]);
            _Paging.ItemsPerPage = 1000;

            if (_Paging == null)
            {
                throw (new Exception("Paging lost"));
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DataTable data = _Paging.PageItems;
        while (data.Rows.Count < 1 && _Paging.CurrentPageIndex > 1)
        {
            _Paging.CurrentPageIndex--;
            data = _Paging.PageItems;
        }

        grid.DataSource = data;
        grid.DataBind();
    }

    protected void btnDelete_Click(object sender, System.EventArgs e)
    {
        int id = Int32.Parse(hiddenID.Value);
        RedirectSeoService.DeleteRedirectSeo(id);

        hiddenID.Value = "";
    }

    protected void btnAddResirects_Click(object sender, EventArgs e)
    {
        string[] lines = redirects.Value.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            string[] words = line.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 2)
            {
                var redirect = new RedirectSeo
                {
                    RedirectFrom = (words[0][0] != '/') ? "/" + words[0] : words[0],
                    RedirectTo = (words[1][0] != '/') ? "/" + words[1] : words[1]
                };

                RedirectSeoService.AddRedirectSeo(redirect);
            }
        }
    }
}
