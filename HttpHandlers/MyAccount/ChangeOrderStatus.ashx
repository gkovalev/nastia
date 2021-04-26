<%@ WebHandler Language="C#" Class="ChangePaymentMethod_removed" %>

using System.Web;

public class ChangePaymentMethod_removed : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
