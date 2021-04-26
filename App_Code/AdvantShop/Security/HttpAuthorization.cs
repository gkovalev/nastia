//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;

namespace AdvantShop.Security
{
    /// <summary>
    /// Summary description for HttpAuthorization
    /// </summary>
    public class HttpAuthorization : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += context_AcquireRequestState;
        }

        private static readonly string[] AuthFileTypes = new[] { ".aspx", ".ashx", ".asmx" };

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;

            if (AdvantShop.Core.UrlRewriter.UrlService.CheckDebugAddress(app.Request.RawUrl.ToLower().Trim()))
            {
                return;
            }

            if (app.Request.FilePath.ToLower().EndsWith(AuthFileTypes) && app.Context.Session != null)
            {
                AuthorizeService.LoadUserCookies();
            }
        }

        public void Dispose()
        {

        }
    }
}