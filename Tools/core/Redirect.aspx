<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Redirect.aspx.cs" Inherits="Tools_core_redirect_seo"
    MasterPageFile="MasterPage.master" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools</title>
</asp:Content>
<asp:Content runat="server" ID="cntMain" ContentPlaceHolderID="main">
    <script type="text/javascript">
        function deleteRedirect(id) {
            $(".HiddenDeleteID").attr("value", id);
            $("#<%=btnDelete.ClientID%>").click();

            return false;
        }
    </script>
    <adv:AdvGridView ID="grid" AutoGenerateColumns="false" runat="server" CssClass="tableview"
        ReadOnlyGrid="true" CellPadding="2" CellSpacing="0" Width="98%" GridLines="None">
        <Columns>
            <asp:TemplateField HeaderText="ID" AccessibleHeaderText="ID" ItemStyle-Width="20px">
                <HeaderTemplate>
                    <span>ID</span>
                </HeaderTemplate>
                <ItemTemplate>
                    &nbsp;<%#Eval("ID")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="RedirectFrom" AccessibleHeaderText="RedirectFrom"
                ItemStyle-Width="350px">
                <HeaderTemplate>
                    <span>Redirect from</span>
                </HeaderTemplate>
                <ItemTemplate>
                    &nbsp;<%#Eval("RedirectFrom")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="RedirectTo" AccessibleHeaderText="RedirectTo" ItemStyle-Width="350px">
                <HeaderTemplate>
                    <span>Redirect to</span>
                </HeaderTemplate>
                <ItemTemplate>
                    &nbsp;<%#Eval("RedirectTo")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="DeleteItem" AccessibleHeaderText="DeleteItem" ItemStyle-Width="20px">
                <HeaderTemplate>
                    <span></span>
                </HeaderTemplate>
                <ItemTemplate>
                    <img src="../../Admin/images/deletebtn.png" style="border: none;" onclick="deleteRedirect(<%#Eval("ID")%>)" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="header" />
        <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
        <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
        <EmptyDataTemplate>
            <center style="margin-top: 20px; margin-bottom: 20px;">
                <%=Resources.Resource.Admin_Catalog_NoRecords%>
            </center>
        </EmptyDataTemplate>
    </adv:AdvGridView>
    <div style="text-align: center">
        <textarea id="redirects" runat="server" cols="125" rows="25"></textarea>
        <br />
        <asp:Button ID="btnAddResirects" runat="server" Text="Добавить редиректы" OnClick="btnAddResirects_Click" />
    </div>
    <input id="hiddenID" class="HiddenDeleteID" runat="server" type="hidden" value="" />
    <asp:Button ID="btnDelete" runat="server" Style="display: none" OnClick="btnDelete_Click" />
    <br />
</asp:Content>
