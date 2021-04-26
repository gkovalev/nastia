<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Title="Untitled Page" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/ReviewsBlock.ascx" TagName="ReviewsBlock" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Charts/GoogleCharts.ascx" TagName="GoogleCharts" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/CommonStatistics.ascx" TagName="CommonStatistics" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/FinanceStatistic.ascx" TagName="FinanceStatistics" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/Notepad.ascx" TagName="Notepad" TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/CurrentSaasData.ascx" TagName="CurrentSaasData" TagPrefix="adv" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder_Head">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript" src="js/googleCharts.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <table style="margin-left: 5px; width: 99%;" cellpadding="4" runat="server" id="tblDashBoard">
        <tr>
            <td valign="top">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" ImageUrl="~/Admin/images/dash.png" runat="server" />
                                    </td>
                                    <td style="width: 100%; height: 31px; font-size: 14px;">
                                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="<%$ Resources: Resource, Admin_Default_Dash %>"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ajaxToolkit:TabContainer ID="TabContainer" runat="server" CssClass="gray">
                                <ajaxToolkit:TabPanel ID="CommonTab" runat="server" HeaderText="<%$ Resources: Resource, Admin_Default_Common_Tab %>">
                                    <ContentTemplate>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <adv:CommonStatistics ID="CommonStatistics1" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%;">
                                                    <adv:ReviewsBlock ID="ReviewsBlock1" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                <ajaxToolkit:TabPanel ID="OrderTab" runat="server" HeaderText="<%$ Resources: Resource, Admin_Default_Order_Tab %>">
                                    <ContentTemplate>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <div style="width: 100%; height: 350; border: 1px #CACACA solid;">
                                                        <center>
                                                            <br />
                                                            <br />
                                                            <asp:Panel ID="pnProfitChart" cssClass="chartOrder" runat="server">
                                                            </asp:Panel>
                                                            <input type="hidden" id="hfChartOrder" value="{sales:'<%=Resources.Resource.Admin_Charts_Sales %>', profit:'<%= Resources.Resource.Admin_Charts_Profit %>', rows: [<%= RenderBigOrdersChartData() %>], title:'<%= Resources.Resource.Admin_Charts_BigChart %>'  }" />
                                                        </center>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%;">
                                                    <adv:FinanceStatistics ID="FinanceStatistics" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                <ajaxToolkit:TabPanel ID="NotepadTab" runat="server" HeaderText="<%$ Resources: Resource, Admin_Default_Notepad_Tab %>">
                                    <ContentTemplate>
                                        <adv:Notepad runat="server" />
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                <ajaxToolkit:TabPanel ID="SaasTab" runat="server" HeaderText="<%$ Resources: Resource, Admin_Default_Tarif_Tab %>">
                                    <ContentTemplate>
                                        <adv:CurrentSaasData ID="CurrentSaasData1" runat="server" />
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                            </ajaxToolkit:TabContainer>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" style="width: 300px;" align="center" rowspan="2">
                <adv:GoogleCharts ID="GoogleCharts2" ChartsWidth="300" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <center>
        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Blue" Visible="False"></asp:Label>
    </center>
</asp:Content>
