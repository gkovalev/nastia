<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GoogleCharts.ascx.cs"
    Inherits="Admin_UserControls_GoogleCharts" %>
<%@ Register Src="~/Admin/UserControls/Charts/OrdersAnnualChart.ascx" TagName="AnnualChart"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Charts/OrdersMonthCountChart.ascx" TagName="CountChart"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Charts/ShippingMethodChart.ascx" TagName="ShippingChart"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Charts/PaymentMethodChart.ascx" TagName="PaymentChart"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Charts/PlanProgressChart.ascx" TagName="ProgressChart"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Charts/YearProfitChart.ascx" TagName="ProfitChart"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Charts/DayProfitChart.ascx" TagName="DayProfitChart"
    TagPrefix="adv" %>
<div>
    <adv:progresschart id="progChart" runat="server" Height="100" Width="300" drawsales="true" />
    <br />
    <adv:profitchart id="ProfitChart1" runat="server" />
    <br />
    <adv:dayprofitchart id="ProfitChart2" runat="server" />
    <br />
    <adv:paymentchart id="pmntChart" runat="server" visible="false" />
    <adv:annualchart id="annChart" runat="server" visible="false" />
    <adv:countchart id="countChart" runat="server" visible="false" />
    <%--<adv:DWYChart ID="dwyChart" runat="server" Width="700" Height="400"/>--%>
    <adv:shippingchart id="shipChart" runat="server" visible="false" />
</div>
