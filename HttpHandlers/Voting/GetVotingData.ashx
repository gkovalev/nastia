<%@ WebHandler Language="C#" Class="GetVotingData" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using AdvantShop.CMS;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;


public class GetVotingData : IHttpHandler
{
    private readonly string _cookieCollectionNameVoting = AdvantShop.Configuration.SettingsMain.SiteUrl + "_Voting";
    
    public void ProcessRequest(HttpContext context)
    {
        var theme = VoiceService.GetTopTheme();
        List<Answer> answers;
        if ((theme == null) || !(answers = theme.Answers).Any())
        {
            context.Response.Write(JsonConvert.Null);
            return;
        }

        int userVoteId = GetUserAnsverId(context.Request.Browser.Cookies, theme.VoiceThemeId);

        var vote = new { Question = theme.Name, Answers = GetAnswers(answers), Result = new { Rows = GetResultRows(answers, userVoteId), Count = theme.CountVoice }, isVoted = userVoteId != -1 };
       
        context.Response.ContentType = "application/json";
        context.Response.Write(JsonConvert.SerializeObject(vote));
    }

    private int GetUserAnsverId(bool haveCookies, int themeId)
    {
        int userAnsverId = -1;
        try
        {
            if (haveCookies)
            {
                var items = CommonHelper.GetCookieCollection(_cookieCollectionNameVoting);
                if (items != null && items[string.Format("ThemesID{0}", themeId)] != null)
                {
                    userAnsverId = Int32.Parse(items[string.Format("ThemesID{0}", themeId)]);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        return userAnsverId;
    }
    
    private object GetResultRows(List<Answer> answers, int userVoteId)
    {
        return
            (from item in answers
             select new {Text = item.Name, Value = item.Percent, Selected = item.AnswerId == userVoteId});
    }

    private object GetAnswers(List<Answer> answers)
    {
        return (from item in answers select new {Text = item.Name, item.AnswerId});
    }
    
    public bool IsReusable
    {
        get { return true;}
    }
}