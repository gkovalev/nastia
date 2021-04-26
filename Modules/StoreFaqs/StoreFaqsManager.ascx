<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StoreFaqsManager.ascx.cs"
    Inherits="Advantshop.UserControls.Modules.StoreFaqs.Admin_StoreFaqsManager" %>
<style>
    .FaqsTable
    {
        width: 100%;
        border-collapse: collapse;
    }
    .FaqsTable td, .FaqsTable th
    {
        text-align: left;
        border-bottom: 1px solid #000000;
        height: 30px;
    }
</style>
<div>
    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr class="rowsPost">
            <td colspan="2" style="height: 34px;">
                <span class="spanSettCategory">
                    <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: StoreFaqs_ManagerHeader%>" /></span>
                <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                <hr color="#C2C2C4" size="1px" />
            </td>
        </tr>
    </table>
    <asp:ListView ID="lvFaqs" runat="server" ItemPlaceholderID="itemPlaceHolder" OnItemCommand="lvFaqsItemCommand">
        <LayoutTemplate>
            <table class="FaqsTable">
                <tr>
                    <th style="width: 200px;">
                        <asp:Label ID="lblDate" runat="server" Text='<%$ Resources: StoreFaqs_DateAdded%>'></asp:Label>
                    </th>
                    <th style="width: 250px;">
                        <asp:Label ID="lblEmail" runat="server" Text='<%$ Resources: StoreFaqs_FaqerEmail%>'></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lblFaq" runat="server" Text='<%$ Resources: StoreFaqs_Faq%>'></asp:Label>
                    </th>
                    <th style="width: 100px;">
                        <asp:Label ID="lblModerated" runat="server" Text='<%$ Resources: StoreFaqs_Moderated%>'></asp:Label>
                    </th>
                    <th style="width: 80px;">
                        <asp:Label ID="lblRate" runat="server" Text='<%$ Resources: StoreFaqs_Rate%>'></asp:Label>
                    </th>
                    <th style="width: 90px;">
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceHolder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%#Eval("DateAdded")%>
                </td>
                <td>
                    <%#Eval("FaqerEmail")%>
                </td>
                <td>
                    <%#Eval("Faq")%>
                </td>
                <td>
                    <asp:CheckBox ID="ckbModerated" runat="server" Checked='<%# Eval("Moderated")%>'
                        Enabled="False" />
                </td>
                <td>
                    <%#Eval("Rate")%>
                </td>
                <td>
                    <a href='<%# "javascript:open_window(\"../modules/StoreFaqs/editFaq.aspx?id=" + Eval("Id") +"\",700,600)"%>'>
                        <asp:Label runat="server" Text='<%$ Resources:StoreFaqs_Edit%>'></asp:Label></a>
                    <asp:LinkButton ID="btnDelete" runat="server" Text="<%$ Resources: StoreFaqs_Delete%>"
                        CommandName="deleteFaq" CommandArgument='<%#Eval("Id") %>' />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
