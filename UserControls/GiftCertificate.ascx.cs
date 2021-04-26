using System;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.FilePath;

public partial class UserControls_GiftCertificate : System.Web.UI.UserControl
{

    public bool isModal { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        Logo.ImgSource = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false);

        if (Request["certificatecode"].IsNotEmpty())
        {
            GiftCertificate certificate = GiftCertificateService.GetCertificateByCode(Request["certificatecode"]);

            if (certificate==null)
            {
                return;
            }

            lblCertificateCode.Text = certificate.CertificateCode;
            lblToName.Text = certificate.ToName;
            lblFromName.Text = certificate.FromName;
            lblMessage.Text = certificate.CertificateMessage;
            lblSum.Text = CatalogService.GetStringPrice(certificate.Sum, true);
        }else
        {
            lblCertificateCode.Text = "0000-0000-0000";
        }
    }
}