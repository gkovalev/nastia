<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OnLineUsers.ascx.cs" Inherits="Admin_UserControls_OnLineUsers" %>
<div>
<a href="UsersOnline.aspx" class="Link" >
<%= Resources.Resource.Admin_UsersOnline%>:<% = AdvantShop.Statistic.ClientInfoService.GetCountInfo().ToString()%>
</a>
</div>
