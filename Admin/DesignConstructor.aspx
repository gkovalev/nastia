<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" ValidateRequest="False"
    AutoEventWireup="true" CodeFile="DesignConstructor.aspx.cs" Inherits="DesignConstructor" Title="" %>
<%@ Register Src="~/Admin/UserControls/ThemesSettings.ascx" TagName="ThemesSettings" TagPrefix="adv" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <style>
        #tabs-contents td { vertical-align: middle; }
    </style>
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px;">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/settings_ico.gif" alt="" />
                    </td>
                    <td class="style1">
                        <asp:Label ID="lbl" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Header %>"></asp:Label><br />
                        <asp:Label ID="lblName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_DesignSettings %>"></asp:Label>
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
                        <li id="themes">
                            <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Themes %>" />
                        </li>
                        <li id="colors">
                            <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Colors  %>" />
                        </li>
                        <li id="backgrounds">
                            <asp:Literal runat="server" Text="<%$ Resources:Resource, Admin_DesignSettings_Backgrounds  %>" />
                        </li>                                
                    </ul>
                    <input type="hidden" runat="server" id="tabid" class="tabid"/>
                </td>
                <td id="tabs-contents">
                    <div class="tab-content">
                        <adv:ThemesSettings ID="ThemesSettings" runat="server" DesignType="Theme" />
                    </div>
                    <div class="tab-content">
                        <adv:ThemesSettings ID="ColorSettings" runat="server" DesignType="Color" />
                    </div>
                    <div class="tab-content">
                        <adv:ThemesSettings ID="BacgroundSettings" runat="server" DesignType="Background" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function setupTooltips() {
            $(".imgtooltip").tooltip({
                showURL: false,
                bodyHandler: function () {
                    var imagePath = $(this).attr("abbr");
                    if (imagePath.length == 0) {
                        return "<div><span><%= Resources.Resource.Admin_Catalog_NoMiniPicture %></span></div>";
                    }
                    else {
                        return $("<img/>").attr("src", imagePath);
                    }
                }
            });
            $(".showtooltip").tooltip({
                showURL: false
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });
    </script>
</asp:Content>