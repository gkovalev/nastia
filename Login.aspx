<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login"
    MasterPageFile="MasterPage.master" %>

<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<%@ Register TagPrefix="adv" TagName="LoginOpenID" Src="~/UserControls/LoginOpenID.ascx" %>
<asp:Content ID="contentMain" ContentPlaceHolderID="cphMain" runat="server">
    <div class="stroke">
        <div class="content-owner">
            <h1>
                <%= Resources.Resource.Client_Login_Authorization %></h1>
            <div class="form-c">
                <div class="title">
                    <%=Resources.Resource.Client_OrderConfirmation_HaveAccount%></div>
                <ul class="form form-auth">
                    <li>
                        <adv:AdvTextBox ValidationType="Login" SpanClass="input-auth" Placeholder="Email"
                            runat="server" ID="txtEmail" />
                    </li>
                    <li>
                        <adv:AdvTextBox ValidationType="Required" SpanClass="input-auth" Placeholder="<%$ Resources:Resource, Client_MasterPage_Password %>"
                            ID="txtPassword" runat="server" TextMode="Password" DefaultButtonID="btnLogin" />
                    </li>
                </ul>
                <div>
                    <adv:Button ID="btnLogin" runat="server" Type="Action" Size="Middle" Text="<%$ Resources:Resource, Client_MasterPage_SignIn %>"
                        OnClick="btnLogin_Click"></adv:Button>
                    <a href="fogotPassword.aspx" class="link-forget">
                        <%=Resources.Resource.Client_MasterPage_FogotPassword%></a>
                </div>
                <adv:LoginOpenID runat="server" PageToRedirect="login.aspx"/>
            </div>
            <div class="form-addon">
                <div class="form-addon-text">
                    <div class="title">
                        <%=Resources.Resource.Client_OrderConfirmation_NewBuyer%></div>
                    <div class="new-user-text">
                        <adv:StaticBlock ID="staticBlockReg" runat="server" SourceKey="loginRegBlock" />
                    </div>
                    <div class="btn-new-users">
                        <adv:Button Href="registration.aspx" Type="Action" Size="Middle" runat="server" Text="<%$Resources:Resource,Client_MasterPage_Registration%>" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
