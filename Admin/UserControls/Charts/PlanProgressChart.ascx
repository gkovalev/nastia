<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanProgressChart.ascx.cs"
    Inherits="Admin_UserControls_PlanProgressChart" %>
<%@ Import Namespace="Resources" %>
<div class="chartCaption">
    <%= Resource.Admin_Charts_ProgressChart %></div>
    <asp:Panel ID='pnSales' CssClass="chartSales" runat="server" Font-Bold="true" HorizontalAlign="Center">
    </asp:Panel>
    <div style="text-align: center; font-weight: bold;">
        <span style="color: #4A83EE;">
            <%= AdvantShop.Catalog.CatalogService.GetStringPrice(prog.Key)%></span>/<span style="color: #FA9C00;"><%= AdvantShop.Catalog.CatalogService.GetStringPrice(plannedSales)%></span></div>
<input type="hidden" id="hfChartProgress" value="{done:'<%= Resource.Admin_Charts_ProgressDone %>', plan:'<%=Resource.Admin_Charts_ProgressPlan%>', rows: [<%= RenderJGSales()%>], width:<%=Width %>, height:<%=Height %> }" />
