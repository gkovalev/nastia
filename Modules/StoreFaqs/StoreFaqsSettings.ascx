<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StoreFaqsSettings.ascx.cs"
    Inherits="Advantshop.UserControls.Modules.StoreFaqs.Admin_StoreFaqsSettings" %>
<div>
    <table border="0" cellpadding="2" cellspacing="0">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: StoreFaqs_Header%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right; margin-left: 10px;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 400px; text-align: left;">
                <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: StoreFaqs_Active%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="ckbEnableStoreFaqs" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 400px; text-align: left;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources: StoreFaqs_ShowRatio%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkShowRatio" Checked="True" />
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 400px; text-align: left;">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: StoreFaqs_PageSize%>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPageSize"></asp:TextBox>
            </td>
        </tr>
        <tr class="rowsPost">
            <td style="width: 400px; text-align: left;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: StoreFaqs_ActiveModerate%>"></asp:Localize>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="ckbActiveModerate" />
            </td>
        </tr>
        
        <tr class="rowsPost">
            <td style="width: 400px; text-align: left;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: StoreFaqs_PageTitle %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPageTitle"></asp:TextBox>
            </td>
        </tr>
        
        <tr class="rowsPost">
            <td style="width: 400px; text-align: left;">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: StoreFaqs_MetaKeyWords %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMetaKeyWords"></asp:TextBox>
            </td>
        </tr>
        
        <tr class="rowsPost">
            <td style="width: 400px; text-align: left;">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: StoreFaqs_MetaDescription %>"></asp:Localize>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtMetaDescription"></asp:TextBox>
            </td> 
        </tr>

        <tr class="rowsPost">
            <td style="width: 400px; text-align: left;">
                <asp:Localize ID="Localize4" runat="server" Text=""></asp:Localize>
            </td>
            <td>
                <asp:HyperLink href="../module/storeFaqs" runat="server" Text="<%$ Resources:StoreFaqs_URL%>" Target="_blank" />
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources:StoreFaqs_Save %>" />
            </td>
        </tr>
    </table>
</div>
