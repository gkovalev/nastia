<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StepAddress.ascx.cs" Inherits="UserControls_OrderConfirmation_FirstStep" %>
<asp:Literal ID="ltSteps" runat="server"></asp:Literal>
<div class="adress-change">
    <div id="DivNoReg" runat="server">
        <div id="dvDemoDataUserNotification" runat="server" class="OrderConfirmation_NotifyLable">
            <%=Resources.Resource.Client_OrderConfirmation_WithDemoMode%></div>
        <div id="dvDemoDataUserNotificationLoginPass" runat="server" class="OrderConfirmation_NotifyLable">
            <%=Resources.Resource.Client_OrderConfirmation_MustEnterPasswordForNewAccount%></div>
        <br />
        <div id="dvLoginPanel" runat="server">
            <ul class="form form-vr">
                <li>
                    <div class="param-name">
                        <label for="txtEmail">
                            E-mail:</label></div>
                    <div class="param-value">
                        <adv:AdvTextBox ValidationType="NewEmail" ID="txtEmail" runat="server" />
                    </div>
                </li>
            </ul>
            <div id="tblLoginTable" runat="server">
                <ul class="form form-vr">
                    <li>
                        <div class="param-name">
                            <label for="txtPassword">
                                <%= Resources.Resource.Client_Registration_Password %>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ValidationType="CompareSource" ID="txtPassword" runat="server" TextMode="Password" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtPasswordConfirm">
                                <%= Resources.Resource.Client_Registration_PasswordAgain %>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ValidationType="Compare" ID="txtPasswordConfirm" runat="server" TextMode="Password" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <ul class="form form-vr">
            <li>
                <div class="param-name">
                    <label for="txtFirstName">
                        <%= Resources.Resource.Client_Registration_Name%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtFirstName" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtLastName">
                        <%= Resources.Resource.Client_Registration_Surname%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtLastName" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtPhone">
                        <%= Resources.Resource.Client_Registration_Phone%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtPhone" runat="server" />
                </div>
            </li>
        </ul>
        <br />
        <ul class="form form-vr">
            <li class="title">
                <div class="param-name">
                </div>
                <div class="param-value">
                    <%=Resources.Resource.Client_Registration_DeliveryInf%>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="cboCountry">
                        <%= Resources.Resource.Client_Registration_Country%>:</label></div>
                <div class="param-value">
                    <asp:DropDownList ID="cboCountry" runat="server" DataTextField="Name" DataValueField="CountryID">
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtState">
                        <%= Resources.Resource.Client_Registration_State%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtState" runat="server" CssClass="autocompleteRegion" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtCity">
                        <%= Resources.Resource.Client_Registration_City%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtCity" runat="server" CssClass="autocompleteCity" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtZip">
                        <%= Resources.Resource.Client_Registration_Zip%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Zip" ID="txtZip" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtAdress">
                        <%= Resources.Resource.Client_Registration_Address%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtAdress" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                </div>
                <div class="param-value">
                    <asp:CheckBox runat="server" ID="chkBillingIsShipping" Checked="true" Text="<%$ Resources: Resource, Client_OrderConfirmation_BillingIsShipping%>"
                        onclick="showHideBillingPanel('#pnBilling');" />
                </div>
            </li>
        </ul>
        <ul class="form form-vr" runat="server" id="pnBilling" style="display: none;">
            <li class="title">
                <div class="param-name">
                </div>
                <div class="param-value">
                    <%=Resources.Resource.Client_Registration_BillingInf%>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingName">
                        <%= Resources.Resource.Client_Registration_Name%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtBillingName" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="cboBillingCountry">
                        <%= Resources.Resource.Client_Registration_Country%>:</label></div>
                <div class="param-value">
                    <asp:DropDownList ID="cboBillingCountry" runat="server" DataTextField="Name" DataValueField="CountryID">
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingState">
                        <%= Resources.Resource.Client_Registration_State%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtBillingState" runat="server" CssClass="autocompleteRegion" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingCity">
                        <%= Resources.Resource.Client_Registration_City%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtBillingCity" runat="server" CssClass="autocompleteCity" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingZip">
                        <%= Resources.Resource.Client_Registration_Zip%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Zip" ID="txtBillingZip" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingAddress">
                        <%= Resources.Resource.Client_Registration_Address%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtBillingAddress" runat="server" />
                </div>
            </li>
        </ul>
        <ul class="form form-vr">
            <li>
                <div class="param-name">
                </div>
                <div class="param-value">
                    <input type="checkbox" runat="server" id="chkAgree" class="valid-required" />
                    <label for="chkAgree">
                        <%= Resources.Resource.Client_OrderConfirmation_Agree%></label>
                </div>
            </li>
        </ul>
    </div>
    <div id="DivReg" runat="server">
        <div id="divNoAddress" runat="server" visible="false" style="margin-bottom: 15px;
            color: Red; font-weight: bold;">
            <% = Resources.Resource.Client_OrderConfirmation_NoAddress%>
        </div>
        <div id="contactsDivOc" class="contactsDiv">
        </div>
        <asp:HiddenField ID="hfOcContactShippingId" runat="server" />
        <asp:HiddenField ID="hfOcContactBillingId" runat="server" />
        <adv:Button ID="btnAddAddress" Type="Submit" Size="Middle" CssClass="btn-add-adr-my"
            runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_Add %>" />
        <div style="display: none">
            <div id="modal">
                <ul class="form form-vr">
                    <li>
                        <div class="param-name">
                            <label for="txtContactNameOc">
                                <%=Resources.Resource.Client_MyAccount_FIO%></label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtContactNameOc" ValidationType="Required" ValidationGroup="address"
                                DefaultButtonID="btnAddChangeContactOc" runat="server"></adv:AdvTextBox>
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="cboCountryOcs">
                                <%=Resources.Resource.Client_MyAccount_Country%></label></div>
                        <div class="param-value">
                            <asp:DropDownList ID="cboCountryOc" DataTextField="Name" DataValueField="CountryID"
                                runat="server">
                            </asp:DropDownList>
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtContactZoneOc">
                                <%=Resources.Resource.Client_MyAccount_Region%></label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtContactZoneOc" ValidationType="Required" ValidationGroup="address"
                                runat="server" CssClass="autocompleteRegion" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtContactCityOc">
                                <%=Resources.Resource.Client_MyAccount_City%></label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtContactCityOc" ValidationType="Required" ValidationGroup="address"
                                runat="server" CssClass="autocompleteCity" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtContactAddressOc">
                                <%=Resources.Resource.Client_MyAccount_Address%></label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtContactAddressOc" SpanClass="adress-mainindex" ValidationGroup="address"
                                DefaultButtonID="btnAddChangeContactOc" ValidationType="Required" runat="server" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtContactZipOc">
                                <%=Resources.Resource.Client_MyAccount_Postcode%></label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtContactZipOc" ValidationType="Zip" ValidationGroup="address"
                                DefaultButtonID="btnAddChangeContactOc" runat="server" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                        </div>
                        <div class="param-value">
                            <adv:Button ID="btnAddChangeContactOc" Size="Big" Type="Submit" ValidationGroup="address"
                                CssClass="btn-save-pass-my" runat="server" Text="<%$ Resources:Resource, Client_MyAccount_Add %>" />
                        </div>
                    </li>
                </ul>
                <asp:HiddenField ID="hfContactIdOc" runat="server" />
                <asp:HiddenField ID="hfBillingIsShippingOc" runat="server" />
                <asp:Label class="ContentText" ID="lblAddressBookMessage" runat="server" Visible="False"
                    ForeColor="black"></asp:Label>
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                getContactsForOC("#contactsDivOc", '<%= ShippingContact != null ? ShippingContact.CustomerContactID.ToString() : "0" %>', '<%= BillingContact != null ? BillingContact.CustomerContactID.ToString() : "0" %>', '<%= BillingIsShipping ? 1 : 0 %>');
            });

        </script>
    </div>
    <div id="DivRegWithoutAddress" runat="server">
        <ul class="form form-vr">
            <li class="title">
                <div class="param-name">
                </div>
                <div class="param-value">
                    <%=Resources.Resource.Client_Registration_DeliveryInf%>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="ddlCountryReg">
                        <%= Resources.Resource.Client_Registration_Country%>:</label></div>
                <div class="param-value">
                    <asp:DropDownList ID="ddlCountryReg" runat="server" DataTextField="Name" DataValueField="CountryID"
                        EnableViewState="True">
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtRegionReg">
                        <%= Resources.Resource.Client_Registration_State%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtRegionReg" runat="server" CssClass="autocompleteRegion" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtCityReg">
                        <%= Resources.Resource.Client_Registration_City%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtCityReg" runat="server" CssClass="autocompleteCity" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtZipReg">
                        <%= Resources.Resource.Client_Registration_Zip%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Zip" ID="txtZipReg" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtAddressReg">
                        <%= Resources.Resource.Client_Registration_Address%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtAddressReg" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                </div>
                <div class="param-value">
                    <asp:CheckBox runat="server" ID="ckbBillingIsShippingReg" Checked="true" Text="<%$ Resources: Resource, Client_OrderConfirmation_BillingIsShipping%>"
                        onclick="showHideBillingPanel('#pnBillingReg');" />
                </div>
            </li>
        </ul>
        <br />
        <br />
        <ul class="form form-vr" runat="server" id="pnBillingReg" style="display: none;">
            <li class="title">
                <div class="param-name">
                </div>
                <div class="param-value">
                    <%=Resources.Resource.Client_Registration_BillingInf%>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingNameReg">
                        <%= Resources.Resource.Client_Registration_Name%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtBillingNameReg" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="ddlBillingCountryReg">
                        <%= Resources.Resource.Client_Registration_Country%>:</label></div>
                <div class="param-value">
                    <asp:DropDownList ID="ddlBillingCountryReg" runat="server" DataTextField="Name" DataValueField="CountryID">
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingRegionReg">
                        <%= Resources.Resource.Client_Registration_State%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtBillingRegionReg" runat="server"
                        CssClass="autocompleteRegion" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingCityReg">
                        <%= Resources.Resource.Client_Registration_City%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtBillingCityReg" runat="server" CssClass="autocompleteCity" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingZipReg">
                        <%= Resources.Resource.Client_Registration_Zip%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Zip" ID="txtBillingZipReg" runat="server" />
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtBillingAddressReg">
                        <%= Resources.Resource.Client_Registration_Address%>:</label></div>
                <div class="param-value">
                    <adv:AdvTextBox ValidationType="Required" ID="txtBillingAddressReg" runat="server" />
                </div>
            </li>
        </ul>
    </div>
</div>
<asp:HiddenField ID="hfSelectedCountry" runat="server" />
<asp:HiddenField ID="hfSelectedCountryBilling" runat="server" />
<div class="oc-panel-wr" runat="server" id="divButtonsForRegUser">
    <div class="oc-panel pie">
        <div class="oc-continue">
            <adv:Button ID="btnRegUserGoNext" runat="server" Size="Big" Type="Confirm" Text="<%$ Resources:Resource, Client_OrderConfirmation_ContinueOrder %>"
                OnClick="btnRegUserGoNext_Click" />
        </div>
        <br class="clear" />
    </div>
</div>
<div class="oc-panel-wr" runat="server" id="divButtonsForRegUserWithoutAddress">
    <div class="oc-panel pie">
        <div class="oc-continue">
            <adv:Button ID="btnRegUserWithounAddressGoNext" runat="server" Size="Big" Type="Confirm"
                Text="<%$ Resources:Resource, Client_OrderConfirmation_ContinueOrder %>" OnClientClick="setSelectedCountry('ddlCountryReg', 'ddlBillingCountryReg', 'ckbBillingIsShippingReg')"
                OnClick="btnRegUserWithounAddressGoNext_Click" />
        </div>
        <br class="clear" />
    </div>
</div>
<div class="oc-panel-wr" runat="server" id="divButtonsForNoRegUser">
    <div class="oc-panel pie">
        <div class="oc-back">
            <adv:Button ID="btn_GoBack" runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_Back %>"
                Size="Big" Type="Confirm" OnClick="btn_GoBack_Click" DisableValidation="True" />
        </div>
        <div class="oc-continue">
            <adv:Button ID="btnConfirm" runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_ContinueOrder %>"
                Size="Big" Type="Confirm" OnClientClick="setSelectedCountry('cboCountry', 'cboBillingCountry', 'ckbBillingIsShippingReg')"
                OnClick="btnConfirm_Click" />
        </div>
        <br class="clear" />
    </div>
</div>
