<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="m_MasterPage.master"
    CodeFile="m_Certificate.aspx.cs" Inherits="Admin_m_Certificate" ValidateRequest="false" %>

<asp:Content ID="contentCenter" runat="server" ContentPlaceHolderID="cphCenter">
    <div>
        <div style="text-align: center;">
            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_SubHeader %>"></asp:Label>
        </div>
        <asp:Panel ID="pnlAdd" runat="server" Height="84px" Width="100%">
            <div style="text-align: center;">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>&nbsp;
            </div>
            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right">
                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Code %>" />&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 27px;">
                        <asp:Label ID="lblCertificateCode" runat="server" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 27px; text-align: right">
                        <asp:Label ID="lblFromName" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_From %>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 27px;">
                        <asp:TextBox ID="txtFromName" runat="server" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="lblToName" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_To %>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtToName" runat="server" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 12px; text-align: right">
                        <asp:Label ID="lblSum" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Sum %>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 12px">
                        <asp:TextBox ID="txtSum" runat="server" Width="224px"></asp:TextBox>
                        <span class="OrderConfirmation_ValidationPoint">*</span><span id="validSum" style="display: none;
                            color: Red;">
                            <%= Resources.Resource.Admin_m_Certificate_WrongFormat %></span>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; text-align: right; height: 29px;">
                        <asp:Label ID="lblType" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Type %>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtnEmail" runat="server" Text="E-mail" Checked="true" GroupName="PostType" />
                        <asp:RadioButton ID="rbtnMail" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Post %>"
                            GroupName="PostType" />
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;" id="tableEmail">
                    <td style="width: 45%; text-align: right; height: 29px;">
                        <%= Resources.Resource.Admin_m_Certificate_RecipientEmail %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                        <span class="OrderConfirmation_ValidationPoint">*</span><span id="validEmail" style="display: none;
                            color: Red;"><%= Resources.Resource.Admin_m_Certificate_WrongFormat %></span>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;" class="tableMail">
                    <td style="width: 45%; text-align: right; height: 29px;">
                        <%=Resources.Resource.Client_Registration_Country%>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelCountry" runat="server" ChildrenAsTriggers="False"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="cboCountry" DataTextField="Name" DataValueField="CountryID"
                                    runat="server" Width="285px" AutoPostBack="True" OnSelectedIndexChanged="cboCountry_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr class="tableMail">
                    <td style="width: 45%; text-align: right; height: 29px;">
                        <%=Resources.Resource.Client_Registration_State%>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="regionsUpdatePanel" runat="server">
                            <ContentTemplate>
                                <asp:MultiView ID="MultiViewRegion" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="ViewDdl" runat="server">
                                        <asp:DropDownList ID="cboRegion" AppendDataBoundItems="true" DataTextField="Name"
                                            DataValueField="RegionID" runat="server" Width="285px">
                                            <asp:ListItem Value="-1" Selected="True" Text="<%$ Resources: Resource, Client_NotSelected %>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:View>
                                    <asp:View ID="ViewTb" runat="server">
                                        <asp:TextBox ID="txtState" runat="server" Width="285px" ValidationGroup="1"></asp:TextBox>
                                        <span class="OrderConfirmation_ValidationPoint">*</span>
                                    </asp:View>
                                </asp:MultiView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;" class="tableMail">
                    <td style="width: 45%; text-align: right; height: 29px;">
                        <%=Resources.Resource.Client_Registration_City%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="tableMail">
                    <td style="width: 45%; text-align: right; height: 29px;">
                        <%=Resources.Resource.Client_Registration_Zip%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;" class="tableMail">
                    <td style="width: 45%; text-align: right; height: 29px;">
                        <%=Resources.Resource.Client_Registration_Address%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 37px; text-align: right">
                        <%= Resources.Resource.Admin_m_Certificate_SenderEmail %>
                    </td>
                    <td style="height: 37px">
                        <asp:TextBox ID="txtFromEmail" runat="server"></asp:TextBox>
                        <span class="OrderConfirmation_ValidationPoint">*</span><span id="validFromEmail"
                            style="display: none; color: Red;">
                            <%= Resources.Resource.Admin_m_Certificate_WrongFormat %></span>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 37px; text-align: right">
                        <asp:Label ID="lblUsed" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Used %>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 37px">
                        <asp:CheckBox ID="chkUsed" runat="server" Checked="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 37px; text-align: right">
                        <asp:Label ID="lblPaid" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Paid %>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 37px">
                        <asp:CheckBox ID="chkPaid" runat="server" Checked="false" />
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 37px; text-align: right">
                        <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Availible %>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 37px">
                        <asp:CheckBox ID="chkEnable" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 37px; text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_m_Certificate_Message %>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 37px">
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Width="300px" Rows="4"></asp:TextBox>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 49%; height: 29px; text-align: right">
                        &nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 29px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <!-- editor -->
            <br />
            <div style="text-align: center;">
                <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Ok %>"
                    Width="110px" OnClientClick="return Validation();" OnClick="btnOK_Click" />&nbsp;
            </div>
            <br />
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="contentScript" runat="server" ContentPlaceHolderID="cphScript">
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {

            if ($("#<%= rbtnEmail.ClientID %>").attr("checked")) {
                $("#tableEmail").show();
                $(".tableMail").hide();
            }
            if ($("#<%= rbtnMail.ClientID %>").attr("checked")) {
                $("#tableEmail").hide();
                $(".tableMail").show();
            }
        });

        $("#<%= rbtnEmail.ClientID %>").click(function () {
            if ($(this).attr("checked")) {
                $("#tableEmail").show();
                $(".tableMail").hide();
            }
        });
        $("#<%= rbtnMail.ClientID %>").click(function () {
            if ($(this).attr("checked")) {
                $("#tableEmail").hide();
                $(".tableMail").show();
            }
        });

        function Validation() {
            var valid = true;
            if ($("#<%= txtSum.ClientID %>").val() == "") {
                $("#validSum").show();
                valid = false;
            }
            else {
                $("#validSum").hide();
            }
            if (!$("#<%= txtSum.ClientID %>").val().match(/^[\d.,\s]+$/)) {
                $("#validSum").show();
                valid = false;
            }
            else {
                $("#validSum").hide();
            }
            if ($("#<%= rbtnEmail.ClientID %>").attr("checked") && $("#<%= txtEmail.ClientID %>").val() == "") {
                $("#validEmail").show();
                valid = false;
            }
            else {
                $("#validEmail").hide();
            }
            if ($("#<%= rbtnEmail.ClientID %>").attr("checked") && !$("#<%= txtEmail.ClientID %>").val().match(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/)) {
                $("#validEmail").show();
                valid = false;
            }
            else {
                $("#validEmail").hide();
            }

            if ($("#<%= txtFromEmail.ClientID %>").val() == "") {
                $("#validFromEmail").show();
                valid = false;
            }
            else {
                $("#validFromEmail").hide();
            }
            if (!$("#<%= txtFromEmail.ClientID %>").val().match(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/)) {
                $("#validFromEmail").show();
                valid = false;
            }
            else {
                $("#validFromEmail").hide();
            }

            return valid;
        }    
    </script>
</asp:Content>
