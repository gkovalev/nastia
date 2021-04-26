<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailChimpSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_MailChimpSettings" %>
<table class="tableMailChimp">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources: Resource, Admin_MailChimpSettings_CommonSettings %>"
                CssClass="spanSettCategory"></asp:Label>
            <hr style="color: #C2C2C4; height: 1px;" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Resource, Admin_MailChimpSettings_Id %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtMailChimpId" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: Resource, Admin_MailChimpSettings_Active %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:CheckBox ID="ckbActive" runat="server" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Label ID="lblMailChimpLeftHead" runat="server" Text="<%$ Resources: Resource, Admin_MailChimpSettings_ShopSubscribers %>"
                    Style="float: left;"></asp:Label>
                <asp:Label ID="lblMailChimpRightHead" runat="server" Text="<%$ Resources: Resource, Admin_MailChimpSettings_MailChimpSubscribers %>"
                    Style="float: right;"></asp:Label>
                <div class="clear">
                </div>
            </span>
            <hr style="color: #C2C2C4; height: 1px;" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Resource, Admin_MailChimpSettings_RegSubscribers %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:DropDownList ID="ddlMailChimpListsReg" runat="server" DataValueField="Id"
                DataTextField="Name" Width="250px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: Resource, Admin_MailChimpSettings_UnRegSubscribers %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:DropDownList ID="ddlMailChimpListsUnReg" runat="server" DataValueField="Id"
                DataTextField="Name" Width="250px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: Resource, Admin_MailChimpSettings_AnyQuestions %>"></asp:Localize>
            <hr style="color: #C2C2C4; height: 1px;" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <a href="http://kb.mailchimp.com/article/where-can-i-find-my-api-key" target="_blank">
                <%= Resources. Resource. Admin_MailChimpSettings_WhereApiKey %></a>
            <br />
        </td>
        <td>
        </td>
    </tr>
</table>
