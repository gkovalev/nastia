<%@ WebHandler Language="C#" Class="DeleteCertificate" %>

using System.Web;
using System.Web.SessionState;
using AdvantShop.Catalog;

public class DeleteCertificate : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        var cer = GiftCertificateService.GetCustomerCertificate();
        if (cer != null)
        {
            GiftCertificateService.DeleteCustomerCertificate(cer.CertificateId);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
