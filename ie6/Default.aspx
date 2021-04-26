<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="ie6_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base href="<%= Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + (!Request.ApplicationPath.EndsWith("/") ? "/" : string.Empty) %>" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="ie6/css/styles.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <div class="header">
            <div class="qtabs">
            </div>
        </div>
        <div class="main" id="current-ex2">
            <div class="qtcontent">
                <div class="return">
                    <a href=".">
                        <%= Resources.Resource.IE6_BackToMainPage %></a>
                </div>
                <h1>
                    <%=Resources.Resource.IE6_Caution %></h1>
                <p>
                    <%=Resources.Resource.IE6_About %></p>
                <p>
                    <%=Resources.Resource.IE6_Recommended %></p>
                <br />
                <table class="brows">
                    <tr>
                        <td>
                            <a href="http://www.microsoft.com/windows/Internet-explorer/default.aspx" onclick="return !window.open(this.href)">
                                <img src="ie6/images/ie.jpg" alt="Internet Explorer" /></a>
                        </td>
                        <td>
                            <a href="http://www.opera.com/download/" onclick="return !window.open(this.href)">
                                <img src="ie6/images/op.jpg" alt="Opera Browser" /></a>
                        </td>
                        <td>
                            <a href="http://www.mozilla.com/firefox/" onclick="return !window.open(this.href)">
                                <img src="ie6/images/mf.jpg" alt="Mozilla Firefox" /></a>
                        </td>
                        <td>
                            <a href="http://www.google.com/chrome" onclick="return !window.open(this.href)">
                                <img src="ie6/images/gc.jpg" alt="Google Chrome" /></a>
                        </td>
                        <td>
                            <a href="http://www.apple.com/safari/download/" onclick="return !window.open(this.href)">
                                <img src="ie6/images/as.jpg" alt="Apple Safari" /></a>
                        </td>
                    </tr>
                    <tr class="brows_name">
                        <td>
                            <a href="http://www.microsoft.com/windows/Internet-explorer/default.aspx" onclick="return !window.open(this.href)">
                                Internet Explorer</a>
                        </td>
                        <td>
                            <a href="http://www.opera.com/download/" onclick="return !window.open(this.href)">Opera
                                Browser</a>
                        </td>
                        <td>
                            <a href="http://www.mozilla.com/firefox/" onclick="return !window.open(this.href)">Mozilla
                                Firefox</a>
                        </td>
                        <td>
                            <a href="http://www.google.com/chrome" onclick="return !window.open(this.href)">Google
                                Chrome</a>
                        </td>
                        <td>
                            <a href="http://www.apple.com/safari/download/" onclick="return !window.open(this.href)">
                                Apple Safari</a>
                        </td>
                    </tr>
                </table>
                <h2>
                    <%=Resources.Resource.IE6_Why %></h2>
                <p>
                    <%=Resources.Resource.IE6_OldBrowser %></p>
                <p>
                    <%=Resources.Resource.IE6_PortableVersion %></p>
                <table>
                    <tr>
                        <td class="td1">
                            <h3>
                                <%=Resources.Resource.IE6_Security %></h3>
                            <%=Resources.Resource.IE6_SecurityDescription %>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td2">
                            <h3>
                                <%=Resources.Resource.IE6_Fact %></h3>
                            <%=Resources.Resource.IE6_FactDescription %>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td3">
                            <h3>
                                <%=Resources.Resource.IE6_Develop %></h3>
                            <%=Resources.Resource.IE6_DevelopRecomended %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="footer">
            <!-- -->
        </div>
    </div>
    </form>
</body>
</html>
