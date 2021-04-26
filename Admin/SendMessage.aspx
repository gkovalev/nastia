<%@ Page AutoEventWireup="true" CodeFile="SendMessage.aspx.cs" Inherits="Admin_SendMessage"
    Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" Title="<%$ Resources:Resource, Admin_SendMessage_Title %>"
    ValidateRequest="false" %>

<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div style="text-align: center;">
        <asp:Label ID="lblCustomer" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_Header %>"></asp:Label>
        <br />
        <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_SubHeader %>"></asp:Label>&nbsp;
        <br />
        <br />
    </div>
    <div style="text-align: center;">
        <asp:Label ID="lblInfo" runat="server" ForeColor="Blue"></asp:Label>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        <br />
    </div>
    <asp:MultiView ID="mvSendingMessages" runat="server">
        <asp:View ID="vSendForm" runat="server">
            <div id="divAdd" style="margin: 0px 5px 0px 5px">
                <div id="pnlAdd" runat="server" width="100%">
                    <div style="text-align: center;">
                        <table id="table2" style="width: 100%; height: 50px; background-color: #eff0f1; margin: 0px;
                            padding: 0px; border: none;">
                            <tr>
                                <td style="height: 27px; text-align: center;">
                                    <div style="text-align: left; width: 800px; margin: auto;">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_TitleAnnotation %>"
                                            Font-Bold="True"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_SendMessage_Required %>"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtTitle" runat="server" Width="309px"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%; margin: 0px; padding: 0px; border: none;">
                            <tr>
                                <td style="width: 100%; text-align: center;">
                                    <div style="text-align: left; width: 800px; margin: auto;">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_Text %>"
                                            Font-Bold="True"></asp:Label>
                                        <asp:Label ID="Label13" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_SendMessage_Required %>"></asp:Label>
                                        <br />
                                        <CKEditor:CKEditorControl ID="fckMailContent" BasePath="~/ckeditor/" runat="server"  Width="800px" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div style="text-align: center;">
                        <div style="width: 800px; margin: auto; text-align: left;">
                            <span style="font-weight: bold;">
                                <%=Resources.Resource.Admin_SendMessage_SendNews%></span>
                            <br />
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 625px;">
                                        <asp:RadioButton ID="rbToAll" runat="server" Checked="true" GroupName="Type" Text="<%$ Resources:Resource, Admin_SendMessage_All %>" />
                                    </td>
                                    <td style="text-align: right; padding-right: 20px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rbToReg" runat="server" GroupName="Type" Text="<%$ Resources:Resource, Admin_SendMessage_RegUsers %>" />
                                    </td>
                                    <td style="text-align: right; padding-right: 20px;">
                                        <span><a href="Subscription.aspx" target="_blank" class="Link">
                                            <%=Resources.Resource.Admin_SendMessage_ViewAddresses%></a></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rbToUnReg" runat="server" GroupName="Type" Text="<%$ Resources:Resource, Admin_SendMessage_UnRegUsers %>" />
                                    </td>
                                    <td style="text-align: right; padding-right: 20px;">
                                        <span><a href="Subscription_Unreg.aspx" target="_blank" class="Link">
                                            <%=Resources.Resource.Admin_SendMessage_ViewAddresses%></a></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <br />
                    <div style="text-align: center;">
                        <asp:Button ID="btnSend" runat="server" Text="<%$ Resources:Resource, Admin_SendMessage_Send %>"
                            Width="101px" OnClick="btnSend_Click" />
                    </div>
                    <br />
                </div>
            </div>
        </asp:View>
        <asp:View ID="vFinishForm" runat="server">
            <div style="text-align: center; width: 800px; margin: auto;">
                <span style="color: blue;">
                    <%=Resources.Resource.Admin_m_News_MailChimpMessageIsSend%></span>
                <br />
                <br />
                <a href="<%= UrlService.GetAdminAbsoluteLink("SendMessage.aspx") %>">Послать еще
                </a>
            </div>
        </asp:View>
        <asp:View ID="vErrorForm" runat="server">
            <div style="text-align: center; width: 800px; margin: auto;">
                <span style="color: red;">
                    <%=Resources.Resource.Admin_m_News_MailChimpOff%></span>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
