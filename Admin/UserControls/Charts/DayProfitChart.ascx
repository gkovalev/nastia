<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DayProfitChart.ascx.cs"
    Inherits="Admin_UserControls_DayProfitChart" %>
<%@ Import Namespace="Resources" %>
<div class="chartCaption"><%= Resource.Admin_Charts_DayProfitChart %></div>
<asp:Panel ID="pnProfitChart" CssClass="chartProfitDay" runat="server">
</asp:Panel>
<input type="hidden" id="hfChartProfitDay" value="{sales: '<%= Resource.Admin_Charts_Sales %>',profit: '<%= Resource.Admin_Charts_Profit %>', rows: [<%=RenderJGData() %>], width: <%=Width %>, height: <%=Height %> }" />
