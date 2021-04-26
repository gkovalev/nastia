<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    CodeFile="err404.aspx.cs" Inherits="err404" EnableViewState="false" %>

<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="stroke">
        <div class="content-owner">
            <div class="err-code">
                <div class="err-request">
                </div>
                <div class="err-request-text">
                    <div class="message-title">
                        <% = Resources.Resource.err404_Title%></div>
                    <div class="message-text">
                        <% = Resources.Resource.err404_Message%></div>
                </div>
            </div>
            <div class="err-recommend">
                <% = Resources.Resource.err404_PossibleReasons%>:
                <ul>
                    <li>
                        <% = Resources.Resource.err404_PageWasDeleted %></li>
                    <li>
                        <% = Resources.Resource.err404_IncorrectLinkToThePage %></li>
                    <li>
                        <% = Resources.Resource.err404_IncorrectTypeOrAddress %></li>
                </ul>
                <div class="split-line">
                </div>
                <div class="text-last">
                    <a href="<%= UrlService.GetAbsoluteLink("/") %>">
                        <% = Resources.Resource.err404_ReturnMessage%></a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
