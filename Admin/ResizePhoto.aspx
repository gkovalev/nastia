<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ResizePhoto.aspx.cs" Inherits="Admin_ResizePhoto" %>

<%@ Register Src="~/Admin/UserControls/ResizePhoto/ResizeProductPhotos.ascx" TagName="ResizeProductPhotos"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/ResizePhoto/ResizeCategoryPhotos.ascx" TagName="ResizeCategoryPhotos"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/ResizePhoto/ResizeCarouselPhotos.ascx" TagName="ResizeCarouselPhotos"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/ResizePhoto/ResizeNewsPhotos.ascx" TagName="ResizeNewsPhotos"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/ResizePhoto/ResizeBrandPhotos.ascx" TagName="ResizeBrandPhotos"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/ResizePhoto/ResizePaymentShippingIcons.ascx"
    TagName="ResizePaymentShippingIcons" TagPrefix="adv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px;">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/settings_ico.gif" alt="" />
                    </td>
                    <td class="style1">
                        <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_ResizePhoto_Header %>"></asp:Label><br />
                        <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_ResizePhoto_SubHeader %>"></asp:Label>
                    </td>
                    <td>
                        <div style="float: right; margin-left: 7px; position: relative; width: 180px; text-align: left;">
                            <adv:OrangeRoundedButton Font-Bold="true" Width="170" ID="btnSave" runat="server"
                                Text="<%$ Resources:Resource, Admin_Update %>" OnClick="btnSave_Click" />
                        </div>
                    </td>
                    <td style='width: 10px'>
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"></asp:Label>
    </div>
    <div style="text-align: center;">
        <asp:Label ID="lblError" runat="server" ForeColor="Blue" Visible="False"></asp:Label>
        <br />
        <table cellpadding="0px" cellspacing="0px" style="width: 98%;" id="tabs">
            <tr>
                <td style="vertical-align: top; width: 225px;">
                    <ul id="tabs-headers">
                        <li id="product">
                            <%= Resources.Resource.Admin_ResizePhotos_Products %>
                        </li>
                        <li id="category">
                            <%= Resources.Resource.Admin_ResizePhotos_Categories %>
                        </li>
                        <li id="news">
                            <%= Resources.Resource.Admin_ResizePhotos_News %>
                        </li>
                        <li id="brand">
                            <%= Resources.Resource.Admin_ResizePhotos_Brands %>
                        </li>
                        <li id="carousel">
                            <%= Resources.Resource.Admin_ResizePhotos_Carousel %>
                        </li>
                        <li id="paymentshippng">
                            <%= Resources.Resource.Admin_ResizePhotos_PaymentShipping %>
                        </li>
                    </ul>
                    <input type="hidden" runat="server" id="tabid" class="tabid" />
                </td>
                <td id="tabs-contents">
                    <div class="tab-content">
                        <adv:ResizeProductPhotos ID="ResizeProductPhotos" runat="server" />
                    </div>
                    <div class="tab-content">
                        <adv:ResizeCategoryPhotos ID="ResizeCategoryPhotos" runat="server" />
                    </div>
                    <div class="tab-content">
                        <adv:ResizeNewsPhotos ID="ResizeNewsPhotos" runat="server" />
                    </div>
                    <div class="tab-content">
                        <adv:ResizeBrandPhotos ID="ResizeBrandPhotos" runat="server" />
                    </div>
                    <div class="tab-content">
                        <adv:ResizeCarouselPhotos ID="ResizeCarouselPhotos" runat="server" />
                    </div>
                    <div class="tab-content">
                        <adv:ResizePaymentShippingIcons ID="ResizePaymentShippingsIcons" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <br />
    </div>
</asp:Content>
