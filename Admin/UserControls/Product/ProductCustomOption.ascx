<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductCustomOption.ascx.cs"
    Inherits="Admin_UserControls_Product_ProductCustomOption" EnableViewState="true" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<adv:EnumDataSource runat="server" ID="edsCustomOptionInputTypes" EnumTypeName="AdvantShop.Catalog.CustomOptionInputType">
</adv:EnumDataSource>
<table class="table-p">
    <tbody>
        <tr>
            <td class="formheader">
                <%--<h2>
                        <%=Resources.Resource.Admin_Product_CustomOptions%></h2>--%>
            </td>
        </tr>
        <tr>
            <td>
                <div class="custom-options">
                    <asp:UpdatePanel ID="UpdatePanelCustomOptions" runat="server" ChildrenAsTriggers="true"
                        UpdateMode="Always">
                        <ContentTemplate>
                            <div>
                                <h2 style="float: left; margin-top: 7px">
                                    <%=Resources.Resource.Admin_Product_CustomOptions%></h2>
                                <div style="padding: 5px 0; height: 24px; float: right">
                                    <asp:Button CssClass="right" ID="btnAddCustomOption" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_AddCustomOption %>"
                                        OnClick="btnAddCustomOption_Click" />
                                </div>
                            </div>
                            <asp:Repeater ID="rCustomOptions" runat="server" OnItemCommand="rCustomOptions_ItemCommand"
                                OnItemDataBound="rCustomOptions_ItemDataBound">
                                <ItemTemplate>
                                    <div class="option-box">
                                        <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("CustomOptionsId") %>' />
                                        <asp:HiddenField ID="hfProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                        <table style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th class="opt-title">
                                                        <%=Resources.Resource.Admin_m_Product_Title%><span class="required">&nbsp;*</span>
                                                    </th>
                                                    <th class="opt-type">
                                                        <%=Resources.Resource.Admin_m_Product_InputType%><span class="required">&nbsp;*</span>
                                                    </th>
                                                    <th class="opt-req">
                                                        <%=Resources.Resource.Admin_m_Product_IsRequired%>
                                                    </th>
                                                    <th class="opt-order">
                                                        <%=Resources.Resource.Admin_Product_SortOrder%>
                                                    </th>
                                                    <th class="a-right" style="vertical-align: bottom">
                                                        <asp:Button ID="Button1" runat="server" CommandArgument='<%# Eval("CustomOptionsId") %>'
                                                            CommandName="DeleteCustomOptions" Text="<%$ Resources:Resource, Admin_m_Product_DeleteOption %>"
                                                            EnableViewState="false" />
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pInvalidTitle" Visible="false" CssClass="validation-advice" runat="server"
                                                            EnableViewState="false">
                                                            <%=Resources.Resource.Admin_m_Product_RequiredField%></asp:Panel>
                                                        <asp:TextBox ID="txtTitle" CssClass="input-text" runat="server" Text='<%# Eval("Title") %>' />
                                                    </td>
                                                    <td class="opt-type">
                                                        <asp:DropDownList ID="ddlInputType" runat="server" DataSourceID="edsCustomOptionInputTypes"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlInputType_SelectedIndexChanged"
                                                            DataTextField="LocalizedName" DataValueField="Value">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="cbIsRequired" runat="server" Checked='<%# Eval("IsRequired") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="pInvalidSortOrder" Visible="false" CssClass="validation-advice" runat="server"
                                                            EnableViewState="false">
                                                            <%=Resources.Resource.Admin_Product_EnterValidNumber%></asp:Panel>
                                                        <asp:TextBox ID="txtSortOrder" runat="server" CssClass="input-text" Text='<%# ((CustomOption)Page.GetDataItem()).IsNull(CustomOptionField.SortOrder) ? "" : Eval("SortOrder")  %>'></asp:TextBox>
                                                    </td>
                                            </tbody>
                                        </table>
                                        <asp:Panel ID="Table1" runat="server" Visible='<%# (CustomOptionInputType)Eval("InputType") == CustomOptionInputType.CheckBox %>'
                                            EnableViewState="false">
                                            <table>
                                                <thead>
                                                    <tr>
                                                        <th class="opt-price">
                                                            <%=Resources.Resource.Admin_Product_Price%><span class="required">&nbsp;*</span>
                                                        </th>
                                                        <th class="opt-pricetype">
                                                            <%=Resources.Resource.Admin_m_Product_PriceType%><span class="required">&nbsp;*</span>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr id="Tr2" runat="server">
                                                        <td>
                                                            <asp:TextBox ID="txtPrice" runat="server" CssClass="input-text" Text='<%# ( (List<OptionItem>) Eval("Options") )[0].PriceBc.ToString("#0.##")  %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPriceType" runat="server" EnableViewState="true">
                                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Product_Fixed %>" Value="Fixed"> </asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Product_Percent %>" Value="Percent"> </asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        <br />
                                        <asp:GridView ID="grid" runat="server" AutoGenerateColumns="false" OnRowDeleting="grid_RowDeleting"
                                            Visible='<%# (CustomOptionInputType)Eval("InputType") != CustomOptionInputType.CheckBox && (CustomOptionInputType)Eval("InputType") != CustomOptionInputType.TextBoxMultiLine && (CustomOptionInputType)Eval("InputType") != CustomOptionInputType.TextBoxSingleLine %>'
                                            DataSource='<%# Eval("Options") %>' OnRowDataBound="grid_RowDataBound" CssClass="GridViewStyle optiontable"
                                            BorderWidth="1">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lId" runat="server" Text='<%# Eval("OptionId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <%=Resources.Resource.Admin_m_Product_Title%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTitle" runat="server" Text='<%# ((OptionItem)Page.GetDataItem()).IsNull(OptionField.Title)? "": Eval("Title") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <%=Resources.Resource.Admin_Product_Price%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPriceBC" runat="server" Text='<%#  ((OptionItem)Page.GetDataItem()).IsNull(OptionField.PriceBc)? "": ((decimal)Eval("PriceBC")).ToString("#0.##")  %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <%=Resources.Resource.Admin_m_Product_PriceType%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlPriceType" runat="server">
                                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Product_Fixed %>" Value="Fixed" />
                                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Product_Percent %>" Value="Percent" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <%=Resources.Resource.Admin_Product_SortOrder%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# ((OptionItem)Page.GetDataItem()).IsNull(OptionField.SortOrder)? "": Eval("SortOrder") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbDelete" runat="server" OnClientClick="removeunloadhandler();"
                                                            CommandName="Delete" CssClass="Link" Text="<%$  Resources:Resource, Admin_Product_DeleteOption%>"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle ForeColor="Black" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="GridView_RowStyle" />
                                            <AlternatingRowStyle CssClass="GridView_AlternatingRowStyle" />
                                            <HeaderStyle CssClass="GridView_HeaderStyle" />
                                            <FooterStyle CssClass="GridView_FooterStyle" Font-Bold="True" />
                                        </asp:GridView>
                                        <div class="optiontable a-right">
                                            <asp:Button ID="btnAdd" runat="server" CommandArgument='<%# Eval("CustomOptionsId") %>'
                                                CommandName="AddNewOption" Visible='<%# ((int)Eval("InputType") == 0 ||(int) Eval("InputType") == 1) %>'
                                                Text="<%$ Resources:Resource, Admin_m_Product_AddNewRow %>" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </tbody>
</table>
