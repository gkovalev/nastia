<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ThaiRating.ascx.cs" Inherits="UserControls_ThaiRating" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Panel ID="pnlRatio" runat="server" Width="100%" Visible="True" CssClass="thaiRating">
    <div id="thaiRatingAuthBlock" runat="server" style="opacity: 0; position: absolute;
        padding: 10px; background: #ffffff; filter: alpha(opacity=0);" class="thaiRatingAuthBlock">
        <asp:Label runat="server" ID="lblAuthRating" Text="<%$ Resources:Resource, Client_ThaiRating %>"></asp:Label>
    </div>
    <b>
    <asp:Label ID="lblProductRatioHead" CssClass="SectionName" runat="server" Text="<%$ Resources:Resource, Client_Details_Rating %>" EnableViewState="false"></asp:Label></b>
    <table id="ratioTable1" style="padding-left: 20px; width: 135px;">
        <tr>
            <td style="vertical-align: middle;">
                <ajaxToolkit:Rating ID="ThaiRating" runat="server" CurrentRating="0" MaxRating="5"
                    AutoPostBack="true" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
                    FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" OnChanged="ThaiRating_Changed" />
                <asp:Label ID="lblSummRating" runat="server" CssClass="RatioText" EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRatingInfo" runat="server" Visible="False" Text="<%$ Resources:Resource, Client_Details_Voted %>" CssClass="ContentText" EnableViewState="false"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
