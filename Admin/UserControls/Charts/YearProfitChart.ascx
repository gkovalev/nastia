<%@ Control Language="C#" AutoEventWireup="true" CodeFile="YearProfitChart.ascx.cs"
    Inherits="Admin_UserControls_YearProfitChart" %>
<%@ Import Namespace="Resources" %>
<div class="chartCaption"><%= Resource.Admin_Charts_ProfitChart %></div>
<asp:Panel ID="pnYearChart" CssClass="chartYearChart" runat="server">
</asp:Panel>
<input type="hidden" id="hfChartProfit" value="{sales: '<%= Resources.Resource.Admin_Charts_Sales %>',profit: '<%= Resources.Resource.Admin_Charts_Profit %>', rows: [<%=RenderJGData() %>], width: <%=Width %>, height: <%=Height %> }"/>

