<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductVideos.ascx.cs"
    Inherits="Admin_UserControls_ProductVideos" %>
    <table class="table-p">
        <tr>
            <td class="formheader" colspan="2">
                <h2>
                    <%=Resources.Resource.Admin_m_Product_Videos%>
                </h2>
            </td>
        </tr>
        <tr class="formheaderfooter">
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="float: right">
                    <adv:AdvButton ID="btnAdd" runat="server" Width="170" Text="<%$ Resources:Resource, Admin_HeadCmdInsert %>"
                        CssMain="adv_OrangeButton_Main" CssInput="adv_OrangeButton_Input" CssInputMozz="adv_OrangeButton_Input-mozz"
                        CssLeftDiv="adv_OrangeButton_LeftDiv" CssRightDiv="adv_OrangeButton_RightDiv"
                        CssCenterDiv="adv_OrangeButton_CenterDiv"  EnableViewState="false"/>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-bottom: 10px;">
                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_CurrentVideos %>" EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lPhotoMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false"></asp:Label>
                <asp:LinkButton ID="lnkUpdateVideo" runat="server" OnClick="lnkUpdateVideo_Click" EnableViewState="false" />
                <div style="width: 100%">
                    <div style="border: 1px #c9c9c7 solid; width: 100%; background-color: White;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <adv:AdvGridView ID="grid_video" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Currencies_QDelete %>"
                                    CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" EditURL=""
                                    GridLines="None" OnRowCommand="grid_RowCommand" OnRowDeleting="grid_RowDeleting"
                                    OnSorting="grid_Sorting" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="ProductVideoID" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label0" runat="server" Text='<%# Bind("ProductVideoID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("ProductVideoID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="10px" ItemStyle-Width="10px"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-Width="360px" ItemStyle-Width="360px"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="width: 360px; padding-left: 6px;">
                                                    <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">
                                                        <%= Resources.Resource.Admin_m_Product_VideoName %>
                                                        <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" />
                                                    </asp:LinkButton>
                                                </div>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' Width="99%" Style="padding-left: 6px;" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lName" runat="server" Text='<%# Bind("Name") %>' Style="padding-left: 6px;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="VideoSortOrder" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="width: 100px;">
                                                </div>
                                                <asp:LinkButton ID="lbVideoSortOrder" runat="server" CommandName="Sort" CommandArgument="VideoSortOrder">
                                                    <%= Resources.Resource.Admin_m_Product_Order %>
                                                    <asp:Image ID="arrowVideoSortOrder" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtVideoSortOrder" runat="server" Text='<%# Eval("VideoSortOrder") %>'
                                                    Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lVideoSortOrder" runat="server" Text='<%# Bind("VideoSortOrder") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="90px">
                                            <EditItemTemplate>
                                                <%# "<a href=\"javascript:open_window('m_ProductVideos.aspx?ID=" + HttpUtility.UrlEncode(HttpUtility.UrlEncode(Eval("ProductVideoID").ToString())) + "&ProductID=" + ProductID + "',780,600);\" class='editbtn showtooltip' title=" + Resources.Resource.Admin_MasterPageAdminCatalog_Edit + "><img src='images/editbtn.gif' style='border: none;' /></a>"%>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="~/admin/images/updatebtn.png" onclick="<%# this.Page.ClientScript.GetPostBackEventReference(grid_video, "Update$" + Container.DataItemIndex)%>;return false;"
                                                    style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                                <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="~/admin/images/deletebtn.png"
                                                    CssClass="deletebtn showtooltip" CommandName="Delete" CommandArgument='<%# Eval("ProductVideoID")%>'
                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="buttonDelete"
                                                    ConfirmText="<%$ Resources:Resource, Admin_Product_ConfirmDeletingVideo %>">
                                                </ajaxToolkit:ConfirmButtonExtender>
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                                    style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#ccffcc" />
                                    <HeaderStyle CssClass="header" />
                                    <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                    <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                    <EmptyDataTemplate>
                                        <center style="margin-top: 20px; margin-bottom: 20px;">
                                            <%=Resources.Resource.Admin_m_Product_NoVideo%>
                                        </center>
                                    </EmptyDataTemplate>
                                </adv:AdvGridView>
                                <input type="hidden" id="SelectedIdsVideo" name="SelectedIdsVideo" />
                                <div style="border-top: 1px #c9c9c7 solid;">
                                </div>
                                <table class="results2">
                                    <tr>
                                        <td style="width: 157px; padding-left: 6px;">
                                            <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                                runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>20</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>100</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                                UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                        </td>
                                        <td style="width: 157px; text-align: right; padding-right: 12px">
                                            <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                                <span style="color: #494949">
                                                    <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                        Width="30" /></span>
                                                <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                                    OnClick="linkGO_Click" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
