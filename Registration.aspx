<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    CodeFile="Registration.aspx.cs" Inherits="Registration" %>

<%@ Register Src="UserControls/Captcha.ascx" TagName="CaptchaControl" TagPrefix="adv" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="content-owner">
            <h1>
                <%= Resources.Resource. Client_Registration_Registration %></h1>
            <div id="dvDemoDataUserNotification" runat="server" visible="false" class="OrderConfirmation_NotifyLable">
                <%=Resources.Resource.Client_OrderConfirmation_WithDemoMode%></div>
            <ul id="ulUserRegistarionValidation" runat="server" visible="false" class="ulValidFaild">
                <li>Error1</li>
                <li>Error2</li>
            </ul>
            <div class="form-c">
                <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="red"></asp:Label>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                <ul class="form">
                    <li>
                        <div class="param-name">
                            <label for="txtFirstName">
                                <%=Resources.Resource.Client_Registration_Name%>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ValidationType="Required" ID="txtFirstName" runat="server" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtLastName">
                                <%=Resources.Resource.Client_Registration_Surname%>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ValidationType="Required" ID="txtLastName" runat="server"></adv:AdvTextBox>
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtEmail">
                                E-Mail:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ValidationType="NewEmail" ID="txtEmail" runat="server" DefaultButtonID="btnRegister" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtPassword">
                                <%=Resources.Resource.Client_Registration_Password%>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ValidationType="CompareSource" ID="txtPassword" runat="server" TextMode="Password" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtPasswordConfirm">
                                <%=Resources.Resource.Client_Registration_PasswordAgain%>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ValidationType="Compare" ID="txtPasswordConfirm" runat="server" TextMode="Password" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtPhone">
                                <%=Resources.Resource.Client_Registration_Phone%>:</label></div>
                                <!--Changed by Evgeni to Set ValidationType="Required-->
                        <div class="param-value">
                            <adv:AdvTextBox ValidationType="Required" ID="txtPhone" runat="server"></adv:AdvTextBox>
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                        </div>
                        <!--Changed by Evgeni to Set checked to true-->
                        <div class="param-value"> 
                            <asp:CheckBox ID="chkSubscribed4News" runat="server" Text="<%$ Resources: Resource, Client_Registration_NewsSubscribe%>" Checked="True" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtValidCode">
                                <%=Resources.Resource.Client_Details_Code%>:</label></div>
                        <div class="param-value">
                            <adv:CaptchaControl ID="dnfValid" runat="server" DefaultButtonID="btnRegister" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                        </div>
                        <div class="param-value">
                            <input type="checkbox" runat="server" id="chkAgree" class="valid-required" />
                            <label for="chkAgree">
                                <%= Resources.Resource.Client_Registration_Agree %></label>
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                        </div>
                        <div class="param-value">
                            <br />
                            <adv:Button ID="btnRegister" Type="Action" Size="Middle" runat="server" Text="<%$ Resources:Resource, Client_Registration_Reg %>"
                                OnClick="btnRegister_Click"></adv:Button>
                        </div>
                    </li>
                </ul>
            </div>
            <div class="form-addon">
                <div class="form-addon-text">
                    <adv:StaticBlock ID="staticBlock" runat="server" SourceKey="textOnReg" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
