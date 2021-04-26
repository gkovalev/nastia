//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using Resources;

public partial class PrintCertificate : Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    //protected string RenderPrice(decimal price, int qty, string ccode, decimal cvalue)
    //{
    //    return CatalogService.GetStringPrice(price * qty, cvalue, ccode);
    //}

    protected string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist)
    {
        if (evlist == null || evlist.Count == 0)
            return "&nbsp;";

        var html = new StringBuilder();
        html.Append("<ul>");

        foreach (EvaluatedCustomOptions ev in evlist)
        {
            html.Append(string.Format("<li>{0}: {1}</li>", ev.CustomOptionTitle, ev.OptionTitle));
        }

        html.Append("</ul>");
        return html.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["certificatecode"]))
        {
            Response.Redirect("default.aspx");
        }

        try
        {
            if (!IsPostBack)
            {
                var certificate = GiftCertificateService.GetCertificateByCode(Request["certificatecode"]);

                if (certificate != null)
                {
                    lblCertificateID.Text = certificate.CertificateId.ToString();
                    lblCertificateCode.Text = certificate.CertificateCode;
//                   lblCertificateStatus.Text = certificate.Status.ToString();
                    lblOrderNumber.Text = certificate.OrderNumber;
                    lblSum.Text = certificate.Sum.ToString();
                    lblFrom.Text = certificate.FromName;
                    lblTo.Text = certificate.ToName;
                    lblUserMessage.Text = certificate.CertificateMessage;

                    if (certificate.Type == CertificatePostType.Email)
                    {
                        pnlMail.Visible = false;
                        pnlEmail.Visible = true;
                        lblEmail.Text = certificate.Email;
                    }
                    else
                    {
                        pnlMail.Visible = true;
                        pnlEmail.Visible = false;
                        lblAddress.Text = string.Format("{0}, {1}, {2}, {3}, {4}", certificate.Country, certificate.Zone,
                                                        certificate.City, certificate.Address, certificate.Zip);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}