<%@ WebHandler Language="C#" Class="ExportFeed_Data" %>

using System.Web;
using System.Web.SessionState;

public class ExportFeed_Data : IHttpHandler, IReadOnlySessionState
{
	public void ProcessRequest(HttpContext context)
	{
		context.Response.ContentType = "application/json";
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(AdvantShop.Statistic.ExportFeedStatistic.Data));
	}

	public bool IsReusable {
		get { return false; }
	}

}
