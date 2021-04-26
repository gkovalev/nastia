<%@ Page Language="C#" MasterPageFile="MasterPage.master" CodeFile="SendRequestOnProduct.aspx.cs"
    Inherits="SendRequestOnProduct" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Register Src="UserControls/Captcha.ascx" TagName="CaptchaControl" TagPrefix="adv" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="content-owner">
            <h1>
                <%=Resources.Resource.Client_SendRequestOnProduct_Header%></h1>
            <div class="form-top">
                <adv:StaticBlock runat="server" SourceKey="requestOnProduct" />
            </div>
            <ul id="ulValidationFailed" runat="server" visible="false" class="ulValidFaild">
                </ul>
            <div class="form-c">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="ViewForm" runat="server">
                        <ul class="form form-vr">
                            <li>
                                <div class="param-name">
                                    <label for="spanName">
                                        <%=Resources.Resource.Client_SendRequestOnProduct_ProductName%>:</label>
                                        <span id="spanName" class="bold">
                                        <%=product.Name %></span></div>
                            </li>
                            <li>
                                <div class="param-name">
                                    <label for="txtAmount">
                                        <%=Resources.Resource.Client_SendRequestOnProduct_Amount%>:</label></div>
                                <div class="param-value">
                                    <adv:AdvTextBox ValidationType="Amount" ID="txtAmount" runat="server" Text="1" />
                                </div>
                            </li>
                            <li>
                                <div class="param-name">
                                    <label for="txtName">
                                        <%=Resources.Resource.Client_SendRequestOnProduct_Name%>:</label></div>
                                <div class="param-value">
                                    <adv:AdvTextBox ValidationType="Required" ID="txtName" runat="server" />
                                </div>
                            </li>
                            <li>
                                <div class="param-name">
                                    <label for="txtEmail">
                                        E-mail:</label></div>
                                <div class="param-value">
                                    <adv:AdvTextBox ValidationType="Email" ID="txtEmail" runat="server" />
                                </div>
                            </li>
                            <li>
                                <div class="param-name">
                                    <label for="txtPhone">
                                        <%=Resources.Resource.Client_SendRequestOnProduct_Phone%>:</label></div>
                                <div class="param-value">
                                    <adv:AdvTextBox ID="txtPhone" runat="server" ValidationType="Required" />
                                </div>
                            </li>
                            <li>
                                <div class="param-name">
                                    <label for="txtComment">
                                        <%=Resources.Resource.Client_SendRequestOnProduct_Comment%>:</label></div>
                                <div class="param-value-textarea  param-value">
                                    <adv:AdvTextBox ID="txtComment" TextMode="MultiLine" runat="server"></adv:AdvTextBox>
                                </div>
                            </li>
                            <li>
                                <div class="param-name">
                                    <label>
                                        <%=Resources.Resource.Client_Details_Code%>:
                                    </label>
                                </div>
                                <div class="param-value">
                                    <adv:CaptchaControl ID="CaptchaControl1" runat="server"/>
                                </div>
                            </li>
                            <li>
                                <div class="param-name">
                                </div>
                                <div class="param-value">
                                    <adv:Button ID="btnSend" Type="Submit" Size="Middle" runat="server" Text="<%$ Resources:Resource, Client_SendRequestOnProduct_Send%>"
                                        OnClick="btnSend_Click"></adv:Button>
                                </div>
                            </li>
                        </ul>
                    </asp:View>
                    <asp:View ID="ViewResult" runat="server">
                        <span class="ContentText">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </span>
                    </asp:View>
                </asp:MultiView>
            </div>
            <div class="form-addon">
                <div class="form-center">
                    <img src="<%=FoldersHelper.GetImageProductPath(ProductImageType.Middle, product.Photo, false) %>"
                        alt="<%=product.Name%>" />
                    <div>
                        <a href="<%= UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId) %>">
                            <%= product.Name %></a>
                    </div>
                    <div class="sku">
                        <%= product.ArtNo %>
                    </div>
                    <div>
                        <%= CatalogService.RenderPrice(product.Price, product.Discount, true, AdvantShop.Customers.CustomerSession.CurrentCustomer.CustomerGroup) %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
