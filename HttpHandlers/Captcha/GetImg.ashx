<%@ WebHandler Language="C#" Class="GetImg" %>

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Web;
using System.Drawing;
using AdvantShop.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

public class GetImg : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        //todo проверить логи на ошибку чтоб понять где делаеться  не правильная строка (ADV-1200)
        if (string.IsNullOrEmpty(context.Request["captchatext"])) return;
        context.Response.ContentType = "image/jpeg";
        byte[] tempString;
        try
        {
            tempString = Convert.FromBase64String(context.Request["captchatext"]);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
            return;
        }
        var captchaText = SecurityHelper.DecryptString(tempString);
        if (captchaText == null) return;

        var letter = new List<Letter>();
        int totalWidth = 0;
        int maxHeight = 0;
        foreach (char c in captchaText)
        {
            var ltr = new Letter(c);
            letter.Add(ltr);
            int space = (new Random()).Next(1, 5) + 1;
            ltr.Space = space;
            totalWidth += ltr.LetterSize.Width + space;
            if (maxHeight < ltr.LetterSize.Height)
            {
                maxHeight = ltr.LetterSize.Height;
            }
        }
        const int hMargin = 5;
        const int vMargin = 3;

        using (var bmp = new Bitmap(totalWidth + hMargin, maxHeight + vMargin))
        {
            using (var grph = Graphics.FromImage(bmp))
            {
                grph.FillRectangle(new SolidBrush(Color.Lavender), 0, 0, bmp.Width, bmp.Height);
                using (var grp = Graphics.FromImage(bmp))
                {
                    using (var background = Image.FromFile(HttpContext.Current.Server.MapPath("~/images/captcha/captcha1.png")))
                        grp.DrawImage(background, new Rectangle(0, 0, bmp.Width, bmp.Height));
                }
                grph.CompositingQuality = CompositingQuality.HighQuality;
                grph.SmoothingMode = SmoothingMode.HighQuality;
                grph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grph.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var xPos = hMargin;
                foreach (var ltr in letter)
                {
                    grph.DrawString(ltr.Symbol.ToString(), ltr.Font, new SolidBrush(Color.Navy), xPos, vMargin);
                    xPos += ltr.LetterSize.Width + ltr.Space;
                    ltr.Dispose();
                }
            }
            bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
