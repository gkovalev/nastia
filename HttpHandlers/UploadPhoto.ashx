<%@ WebHandler Language="C#" Class="UploadPhoto" %>

using System;
using System.Web;
using AdvantShop.Catalog;
using System.Drawing;
using AdvantShop.Helpers;
using AdvantShop.SaasData;

public class UploadPhoto : IHttpHandler
{
    static void Msg(HttpContext context, string msg)
    {
        context.Response.Write("{error:'" + msg + "', msg:'error'}");
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";

        if (SaasDataService.IsSaasEnabled)
        {
            int maxPhotoCount = SaasDataService.CurrentSaasData.PhotosCount;
            if (PhotoService.GetCountPhotos(Convert.ToInt32(context.Request["productid"]), PhotoType.Product) >= maxPhotoCount)
            {
                Msg(context, "Достигнуто максимальное количество фотографий в товаре на вашем тарифном плане - " + maxPhotoCount);
                return;
            }
        }
        if (context.Request.Files.Count < 1)
        {
            context.Response.Write("{error:'no file', msg:'error'}");
            return;
        }

        HttpPostedFile pf = context.Request.Files[0];

        if (string.IsNullOrEmpty(pf.FileName))
        {
            context.Response.Write("{error:'no file', msg:'error'}");
            return;
        }

        if (!pf.FileName.Contains("."))
        {
            context.Response.Write("{error:'no file extension', msg:'error'}");
            return;
        }
        if (!FileHelpers.CheckImageExtension(pf.FileName))
        {
            context.Response.Write("{error:'wrong extension', msg:'error'}");
            return;
        }

        try
        {
            var productId = Convert.ToInt32(context.Request["productid"]);
            var tempName = PhotoService.AddPhoto(new Photo(0, productId, PhotoType.Product) { Description = context.Request["description"], OriginName = pf.FileName });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (Image image = Image.FromStream(pf.InputStream))
                {
                    FileHelpers.SaveProductImageUseCompress(tempName, image);
                }
            }
        }
        catch (Exception ex)
        {
            Msg(context, ex.Message + " at Uploadimage");
            AdvantShop.Diagnostics.Debug.LogError(ex, ex.Message + " at Uploadimage", false);
            return;
        }

        context.Response.Write("{error:'', msg:'success'}");
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}