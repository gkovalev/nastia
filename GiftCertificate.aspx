<%@ Page Language="C#" MasterPageFile="MasterPage.master" CodeFile="GiftCertificate.aspx.cs"
    Inherits="GiftCertificate_Page" EnableViewState="false" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="UserControls/Captcha.ascx" TagName="CaptchaControl" TagPrefix="adv" %>
<%@ Register tagPrefix="adv" tagName="GiftCertificate" src="~/UserControls/GiftCertificate.ascx" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="content-owner">
            <h1>
                <% = Resource.Client_GiftCertificate_Header %></h1>
            <div class="form-c">
                <ul class="form form-vr">
                    <li>
                        <div class="param-name">
                            <label for="txtTo">
                                <%= Resource.Client_GiftCertificate_To%>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtTo" runat="server" ValidationType="Required" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtFrom">
                                <%= Resource.Client_GiftCertificate_From%>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtFrom" runat="server" ValidationType="Required" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtSum">
                                <%= Resource.Client_GiftCertificate_Sum%>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtSum" runat="server" ValidationType="Money" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtMessage">
                                <%= Resource.Client_GiftCertificate_Message%>:</label></div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtMessage" CssClassWrap="certificate-message" ValidationType="Required" TextMode="MultiLine" runat="server">
                            </adv:AdvTextBox>
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="rbtnEmail">
                                <%= Resource.Client_GiftCertificate_DeliveryMethod%>:
                            </label>
                        </div>
                        <div class="param-value">
                            <asp:RadioButton ID="rbtnEmail" runat="server" Text='<%$ Resources:Resource, Client_GiftCertificate_DeliveryByEmail%>'
                                Checked="true" GroupName="PostType" />
                            <asp:RadioButton ID="rbtnMail" runat="server" Text='<%$ Resources:Resource, Client_GiftCertificate_DeliveryByPost%>'
                                GroupName="PostType" />
                        </div>
                    </li>
                    <li class="emailDelivery">
                        <div class="param-name">
                            <label for="txtEmail">
                                <%= Resource.Client_GiftCertificate_RecipientEmail%>:
                            </label>
                        </div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtEmail" ValidationType="Email" runat="server"></adv:AdvTextBox>
                        </div>
                    </li>
                    <li class="postDelivery">
                        <div class="param-name">
                            <label for="cboCountry">
                                <%=Resource.Client_Registration_Country%>:
                            </label>
                        </div>
                        <div class="param-value">
                            <asp:DropDownList ID="cboCountry" runat="server" DataTextField="Name" DataValueField="CountryID" onchange="$('#hfCountry').val($(this).val());">
                            </asp:DropDownList>
                            <asp:HiddenField runat="server" ID="hfCountry"/>
                        </div>
                    </li>
                    <li class="postDelivery">
                        <div class="param-name">
                            <label for="txtMessage">
                                <%= Resource.Client_Registration_State%>:
                            </label>
                        </div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtState" ValidationType="Required" runat="server" CssClass="autocompleteRegion">
                            </adv:AdvTextBox>
                        </div>
                    </li>
                    <li class="postDelivery">
                        <div class="param-name">
                            <label for="txtMessage">
                                <%=Resource.Client_Registration_City%>:
                            </label>
                        </div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtCity" ValidationType="Required" runat="server" CssClass="autocompleteCity">
                            </adv:AdvTextBox>
                        </div>
                    </li>
                    <li class="postDelivery">
                        <div class="param-name">
                            <label for="txtZip">
                                <%=Resource.Client_Registration_Zip%>:
                            </label>
                        </div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtZip" ValidationType="Zip" runat="server"></adv:AdvTextBox>
                        </div>
                    </li>
                    <li class="postDelivery">
                        <div class="param-name">
                            <label for="txtAdress">
                                <%=Resource.Client_Registration_Address%>:
                            </label>
                        </div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtAdress" ValidationType="Required" runat="server"></adv:AdvTextBox>
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                            <label for="txtMessage">
                                <%=Resource.Client_Details_Code%>:
                            </label>
                        </div>
                        <div class="param-value">
                            <adv:CaptchaControl ID="validShield" runat="server" />
                        </div>
                    </li>
                    <li>
                        <div class="param-name">
                        </div>
                        <div class="param-value">
                            <adv:Button ID="btnBuy" Type="Submit" Size="Middle" runat="server" Text="<%$ Resources:Resource, Client_GiftCertificate_Buy %>"
                                OnClick="btnBuy_Click"></adv:Button>
                            <a href="javascript:void(0)" id="printCert">
                                <%=Resource.Client_GiftCertificate_PreView%></a>
                        </div>
                    </li>
                </ul>
            </div>
            <div class="form-addon">
                <div class="form-addon-text">
                    <p>
                        <%=Resource.Client_GiftCertificate_Limits%>
                        <br />
                        <%=Resource.Client_GiftCertificate_MinimalPrice%>:
                        <%= CatalogService.GetStringPrice(SettingsOrderConfirmation.MinimalPriceCertificate) %><br />
                        <%=Resource.Client_GiftCertificate_MaximumPrice%>:
                        <%= CatalogService.GetStringPrice(SettingsOrderConfirmation.MaximalPriceCertificate) %><br />
                        <%=Resource.Client_GiftCertificate_MinimumOrderPrice%>:
                        <%= CatalogService.GetStringPrice(SettingsOrderConfirmation.MinimalOrderPrice) %><br />
                    </p>
                </div>
            </div>
        </div>
    </div>
    <adv:GiftCertificate ID="GiftCertificate" runat="server" isModal="true"/>
    <script type="text/javascript">
        $(function () {
            if ($("#rbtnEmail").is(":checked")) {
                $(".postDelivery").hide();
                $(".emailDelivery").show();
                
            } else {
                $(".postDelivery").show();
                $(".emailDelivery").hide();
            }
            
        });

        $("#rbtnEmail").click(function () {
            if ($(this).is(":checked")) {
                $(".postDelivery").hide();
                $(".emailDelivery").show();
            }
            validateControlsPos();
        });
        $("#rbtnMail").click(function () {
            if ($(this).is(":checked")) {
                $(".postDelivery").show();
                $(".emailDelivery").hide();
            }
            validateControlsPos();
        });
    </script>
</asp:Content>
