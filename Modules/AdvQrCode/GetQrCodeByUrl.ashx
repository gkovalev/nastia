<%@ WebHandler Language="C#" Class="GetQrCodeByUrl" %>



using System;
using System.IO;
using System.Web;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Controls;

public class GetQrCodeByUrl : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "image/png";

        var url = HttpContext.Current.Request.Url.ToString();
        QrCode qrCode;
        (new QrEncoder(ErrorCorrectionLevel.H)).TryEncode(url.Split(new[] { "?" }, StringSplitOptions.None)[0], out qrCode);

        var renderer = new Renderer(2, System.Drawing.Brushes.Black, System.Drawing.Brushes.White);
        using (var ms = new MemoryStream())
        {
            renderer.WriteToStream(qrCode.Matrix, ms, System.Drawing.Imaging.ImageFormat.Png);

            context.Response.BinaryWrite(ms.ToArray());
            context.Response.Flush();
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