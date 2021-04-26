<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyAccountAddressBook.ascx.cs"
    Inherits="UserControls_MyAccountAddressBook" %>
<%@ Import Namespace="Resources" %>
<div class="subtitle">
    <%= Resource.Client_MyAccount_AddressBook %></div>
<div id="contactsDivForm" class="containerDiv">
</div>
<br />
<adv:Button ID="btnAddNew" Type="Submit" Size="Middle" runat="server" OnClientClick="ShowModalAddAddress();"
    Text="<%$ Resources: Resource, Client_MyAccount_Add %>" />
<div style="display: none">
    <div id="modalMyAccountAddress">
        <ul class="form form-vr">
            <li>
                <div class="param-name">
                    <label for="txtContactName">
                        <%=Resource.Client_MyAccount_FIO%></label></div>
                <div class="param-value">
                    <adv:AdvTextBox ID="txtContactName" ValidationType="Required" runat="server" ValidationGroup="maAddress"  DefaultButtonID="btnAddChangeContact"></adv:AdvTextBox></div>
            </li>
            <li>
                <div class="param-name">
                    <label for="cboCountry">
                        <%=Resource.Client_MyAccount_Country%></label></div>
                <div class="param-value">
                    <asp:DropDownList ID="cboCountry" DataTextField="Name" DataValueField="CountryID"
                        runat="server">
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtContactZone">
                        <%=Resource.Client_MyAccount_Region%></label></div>
                <div class="param-value">
                    <adv:AdvTextBox ID="txtContactZone" ValidationType="Required" runat="server" CssClass="autocompleteRegion" ValidationGroup="maAddress"></adv:AdvTextBox></div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtContactCity">
                        <%=Resource.Client_MyAccount_City%></label></div>
                <div class="param-value">
                    <adv:AdvTextBox ID="txtContactCity" ValidationType="Required" runat="server" CssClass="autocompleteCity" ValidationGroup="maAddress"></adv:AdvTextBox></div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtContactAddress">
                        <%=Resource.Client_MyAccount_Address%></label></div>
                <div class="param-value">
                    <adv:AdvTextBox ID="txtContactAddress" SpanClass="adress-mainindex" ValidationType="Required"
                        runat="server" ValidationGroup="maAddress" DefaultButtonID="btnAddChangeContact"></adv:AdvTextBox></div>
            </li>
            <li>
                <div class="param-name">
                    <label for="txtContactZip">
                        <%=Resource.Client_MyAccount_Postcode%></label></div>
                <div class="param-value">
                    <adv:AdvTextBox ID="txtContactZip" ValidationType="Zip" runat="server" ValidationGroup="maAddress" DefaultButtonID="btnAddChangeContact"></adv:AdvTextBox></div>
            </li>
            <li>
                <div class="param-name">
                </div>
                <div class="param-value">
                    <adv:Button ID="btnAddChangeContact" Size="Big" Type="Submit" CssClass="btn-save-pass-my"
                        runat="server" Text="<%$ Resources:Resource, Client_MyAccount_Add %>" ValidationGroup="maAddress" />
                </div>
            </li>
        </ul>
        <asp:HiddenField ID="hfContactId" runat="server" />
        <asp:Label class="ContentText" ID="lblAddressBookMessage" runat="server" Visible="False"
            ForeColor="black"></asp:Label>
    </div>
</div>
