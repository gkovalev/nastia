<%@ WebHandler Language="C#" Class="StoreFaqsAdd" %>

using System;
using System.Web;
using AdvantShop.Modules;

public class StoreFaqsAdd : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        int parentId;
        bool resultParseParentId = int.TryParse(context.Request["parentId"], out parentId);
        
        if (!resultParseParentId)
        {
            context.Response.Write("false");
        }

        int scope;
        bool resultParseScope = int.TryParse(context.Request["scope"], out scope);

        if (!resultParseScope)
        {
            context.Response.Write("false");
        }   
        
        if (string.IsNullOrEmpty(context.Request["name"]))
        {
            context.Response.Write("false");
        }
        
        if (string.IsNullOrEmpty(context.Request["email"]))
        {
            context.Response.Write("false");
        }
        
        if (string.IsNullOrEmpty(context.Request["Faq"]))
        {
            context.Response.Write("false");
        }

        StoreFaqRepository.AddStoreFaq(new StoreFaq
        {
            Moderated = false,
            Rate = scope,
            ParentId = parentId,
            FaqerEmail = HttpUtility.HtmlEncode(context.Request["email"]),
            FaqerName = HttpUtility.HtmlEncode(context.Request["name"]),
            Faq = HttpUtility.HtmlEncode(context.Request["Faq"])
        });

        context.Response.Write("success");   
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}