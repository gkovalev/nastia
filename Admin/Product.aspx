<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Product.aspx.cs" Inherits="Admin_Product" EnableViewStateMac="false" %>

<%@ Register Src="~/Admin/UserControls/Product/RightNavigation.ascx" TagName="RightNavigation"
    TagPrefix="adv" %>
<%@ Register Src="../UserControls/Catalog/SiteNavigation.ascx" TagName="SiteNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductProperties.ascx" TagName="ProductProperties"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductPhotos.ascx" TagName="ProductPhotos"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductVideos.ascx" TagName="ProductVideos"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/RelatedProducts.ascx" TagName="RelatedProducts"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Product/ProductCustomOption.ascx" TagName="ProductCustomOption"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/PopupGridBrand.ascx" TagName="PopupGridBrand"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <title>
        <%=GetPageTitle()%></title>
    <link rel="stylesheet" href="~/Admin/css/slimbox2.css" type="text/css" media="screen"
        id="slimboxStyle" runat="server" />
    <script type="text/javascript" src="js/slimbox2.js"></script>
    <script type="text/javascript" src="js/ajaxfileupload.js"></script>
    <script type="text/javascript">

        function focusoninput(sender) {
            $(sender).parent().find("td:last input").focus();
            $(sender).parent().find("td:last textarea").focus();
        }

        function removeunloadhandler(a) {
            window.onbeforeunload = null;
        }

        function endRequest() {
            window.onbeforeunload = beforeunload;
            $(".photoinput").val("");
        }

        var skip = false;
        var dirty = false;

        function beforeunload(e) {
            if (!skip) {
                if ($("img.floppy:visible, img.exclamation:visible").length > 0) {
                    var evt = window.event || e;
                    evt.returnValue = '<%=Resources.Resource.Admin_Product_LosingChanges%>';
                }
            } else {
                skip = false;
            }
        }

        function addbeforeunloadhandler() {
            window.onbeforeunload = beforeunload;
        }

        $(document).ready(function () {
            var szCookieString = document.cookie;
            if ($.cookie("isVisibleRightPanel") != "false") {
                showRightPanel();
            }

            window.onbeforeunload = beforeunload;

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(endRequest);

            $("#<%= btnSave.ClientID %>").click(function () { skip = true; });

            window.onbeforeunload = beforeunload;
        });

        function showRightPanel() {
            document.getElementById("rightPanel").style.display = "block";
            document.getElementById("right_divHide").style.display = "block";
            document.getElementById("right_divShow").style.display = "none";
        }

        function toggleRightPanel() {
            if ($.cookie("isVisibleRightPanel") == "true") {
                $("div:.rightPanel").hide("fast");
                $("div:.right_hide_rus").hide("fast");
                $("div:.right_show_rus").show("fast");
                $("div:.right_hide_en").hide("fast");
                $("div:.right_show_en").show("fast");
                $.cookie("isVisibleRightPanel", "false", { expires: 7 });
            } else {
                $("div:.rightPanel").show("fast");
                $("div:.right_show_rus").hide("fast");
                $("div:.right_hide_rus").show("fast");
                $("div:.right_show_en").hide("fast");
                $("div:.right_hide_en").show("fast");
                $.cookie("isVisibleRightPanel", "true", { expires: 7 });
            }
        }
        
    </script>
    <style type="text/css">
        h2
        {
            margin: 1px 0;
            font-size: 11pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <asp:UpdateProgress runat="server" ID="uprogress">
        <ProgressTemplate>
            <div id="inprogress">
                <div id="curtain" class="opacitybackground">
                    &nbsp;</div>
                <div class="loader">
                    <table width="100%" style="font-weight: bold; text-align: center;">
                        <tbody>
                            <tr>
                                <td align="center">
                                    <img src="images/ajax-loader.gif" alt="" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="color: #0D76B8;">
                                    <asp:Localize ID="Localize_Admin_Product_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Product_PleaseWait %>"
                                        EnableViewState="false"></asp:Localize>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <input type="hidden" name="tabid" id="tabid" class="tabid" runat="server" />
    <table style="width: 100%;">
        <tr>
            <td style="width: 10px;">
            </td>
            <td style="vertical-align: top; width: 100%">
                <div style="width: 800px; font-size: 0px;">
                </div>
                <table style="width: 100%; table-layout: fixed;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="135" rowspan="2">
                            <asp:UpdatePanel ID="upPhoto" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="productPhotos" EventName="MainPhotoUpdate" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Literal ID="ltPhoto" runat="server" Text='<%# HtmlProductImage()%>' EnableViewState="false"></asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top" style="padding-left: 10px;">
                            <asp:Label ID="lProductName" CssClass="AdminHead" runat="server" EnableViewState="false"></asp:Label>
                            <asp:Label ID="lbIsProductActive" runat="server" EnableViewState="false"></asp:Label><br />
                            <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Product_SubHeader %>"
                                EnableViewState="false"></asp:Label><br />
                        </td>
                        <td align="right" valign="bottom" style="padding-bottom: 5px; width: 400px; padding-right: 3px;"
                            rowspan="2">
                            <a target="_blank" class="Link" runat="server" id="aToClient" enableviewstate="false">
                            <%=Resources.Resource.Admin_m_Product_Main%></a>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;<asp:LinkButton ID="lnkCopy" CssClass="Link" runat="server" Text="Копировать продукт"
                                            OnClientClick="" onmousedown="window.onbeforeunload=null;" 
                                            EnableViewState="false" onclick="lnkCopy_Click" /> 
                                        &nbsp;</td>
                                    <td>
                                        <adv:OrangeRoundedButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="encodeFormData();"
                                            onmousedown="window.onbeforeunload=null;" EnableViewState="false" />
                                        <asp:LinkButton ID="btnAdd" CssClass="Link" runat="server" Text="<%$ Resources:Resource, Admin_Product_AddNewProduct %>"
                                            OnClientClick="" onmousedown="window.onbeforeunload=null;" EnableViewState="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom" style="padding-left: 10px;">
                            <span style="font-weight: bold;">
                                <asp:Localize ID="Localize_Admin_Catalog_CategoryLocation" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_CategoryLocation %>"
                                    EnableViewState="false"></asp:Localize></span><br />
                            <uc1:SiteNavigation ID="sn" runat="server" EnableViewState="false" />
                            <asp:Label ID="lMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr style="height: 7px;">
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table id="tabs">
                                <tr>
                                    <td style="width: 225px;">
                                        <ul id="tabs-headers">
                                            <li id="general">
                                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Main %>"
                                                    EnableViewState="false" />
                                                <asp:Image ID="imgExcl1" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                                    CssClass="exclamation" EnableViewState="false" />
                                                <img id="itab1floppy" name="itab1floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                            </li>
                                            <li id="cat" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, Admin_Product_Categories%>"
                                                    EnableViewState="false" /></span> </li>
                                            <li id="price">
                                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, Admin_Product_PriceAvailability  %>"
                                                    EnableViewState="false" />
                                                <asp:Image ID="imgExcl2" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                                    CssClass="exclamation" />
                                                <img id="itab2floppy" name="itab2floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                            </li>
                                            <li id="photos" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Photos %>"
                                                    EnableViewState="false" /></span> </li>
                                            <li id="videos" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                                <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Videos %>"
                                                    EnableViewState="false" /></span> </li>
                                            <li id="descr">
                                                <asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:Resource, Admin_Product_Description  %>"
                                                    EnableViewState="false" />
                                                <input type="hidden" id="tab4floppy" name="tab4floppy" value="false" /><img id="itab4floppy"
                                                    name="itab4floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                            </li>
                                            <li id="prop" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Resource, Admin_Product_ProductProperties %>"
                                                    EnableViewState="false" /></span> </li>
                                            <li id="customopt" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>">
                                                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Resource, Admin_Product_CustomOptions%>"
                                                    EnableViewState="false" />
                                                <asp:Image ID="imgExcl6" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                                    CssClass="exclamation" EnableViewState="false" />
                                                <img id="itab6floppy" name="itab6floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                            </li>
                                            <li id="seo">
                                                <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Resource, Admin_Product_SEO  %>"
                                                    EnableViewState="false" />
                                                <asp:Image ID="imgExcl7" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                                    CssClass="exclamation" EnableViewState="false" />
                                                <img id="itab7floppy" name="itab7floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                            </li>
                                            <li id="related" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                                <asp:Literal ID="lRelatedProduct" runat="server" Text="" EnableViewState="false" /></span>
                                            </li>
                                            <li id="alt" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>"><span>
                                                <asp:Literal ID="lAlternativeProduct" runat="server" Text="" EnableViewState="false" /></span>
                                            </li>
                                            <li id="taxes" style="<%= (AddingNewProduct ? "display:none;": String.Empty)%>">
                                                <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:Resource, Admin_Product_Taxes  %>" />
                                                <asp:Image ID="Image2" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                                    CssClass="exclamation" EnableViewState="false" />
                                                <img id="Img1" name="itab10floppy" style="display: none" class="floppy" src="images/floppy.gif" />
                                            </li>
                                        </ul>
                                    </td>
                                    <td id="tabs-contents">
                                        <asp:Label ID="lblError" runat="server" CssClass="mProductLabelInfo" ForeColor="Red"
                                            Visible="False" Font-Names="Verdana" Font-Size="14px" EnableViewState="false"></asp:Label>
                                        <!-- Main -->
                                        <div class="tab-content">
                                            <table class="table-p">
                                                <tr>
                                                    <td class="formheader">
                                                        <h2>
                                                            <%=Resources.Resource.Admin_m_Product_UrlSynonym%></h2>
                                                    </td>
                                                </tr>
                                                <tr class="formheaderfooter">
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table-p">
                                                <tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="Label5" runat="server" Text="ID" EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px;">
                                                        &nbsp;<asp:Label ID="lblProductId" runat="server" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_Product_StockNumber %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px;">
                                                        <asp:TextBox ID="txtStockNumber" CssClass="toencode" runat="server" Width="226px" />
                                                        <asp:Label runat="server" ID="lStockNumberError" ForeColor="Red" Visible="false"
                                                            EnableViewState="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_Product_Name %>"
                                                            EnableViewState="false"></asp:Label><span style="color: Red">*</span>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px;">
                                                        <asp:TextBox ID="txtName" CssClass="toencode" runat="server" Width="339px" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                            EnableClientScript="false" Display="Dynamic" ValidationGroup="1" ErrorMessage='<%$ Resources:Resource,Admin_m_Product_RequiredField %>'> </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;" onclick="focusoninput(this)">
                                                        <%=Resources.Resource.Admin_Product_Additional%>
                                                        <span style="color: Red">*</span>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px;">
                                                        <asp:TextBox ID="txtSynonym" CssClass="toencode" runat="server" Width="339px" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSynonym"
                                                            EnableClientScript="false" Display="Dynamic" ValidationGroup="1" ErrorMessage='<%$ Resources:Resource,Admin_m_Product_RequiredField %>'> </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSynonym"
                                                            EnableClientScript="false" Display="Dynamic" ValidationGroup="1" ErrorMessage='<%$ Resources:Resource,Admin_m_Category_SynonymInfo %>'
                                                            ValidationExpression="^[a-zA-Z0-9_-]*$"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Active %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formheader" colspan="2">
                                                        <h2>
                                                            <% = Resources.Resource.Admin_Product_SizeDimension%></h2>
                                                    </td>
                                                </tr>
                                                <tr class="formheaderfooter">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_ProductBrand %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="popUpBrand" />
                                                                            <asp:AsyncPostBackTrigger ControlID="ibRemoveBrand" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:Label runat="server" ID="lBrand" EnableViewState="false"></asp:Label>
                                                                            <asp:ImageButton runat="server" ID="ibRemoveBrand" ImageUrl="~/Admin/images/remove.jpg"
                                                                                Style="margin-left: 2px; margin-right: 2px;" OnClick="ibRemoveBrand_Click" EnableViewState="false" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="ShowModalPopupBrand();return false"
                                                                        Text="<%$Resources:Resource, Admin_Product_Select %>" Style="margin-left: 5px"
                                                                        EnableViewState="false"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_ProductWeight %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:TextBox ID="txtWeight" runat="server" Width="160px" Text="0" />
                                                        <asp:Localize Text="<%$ Resources:Resource,Admin_Product_Kg %>" ID="Localize1" runat="server"
                                                            EnableViewState="false"></asp:Localize>
                                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtWeight"
                                                            ValidationGroup="1" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="3000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                        <asp:RequiredFieldValidator ID="RangeValidator7" runat="server" ControlToValidate="txtWeight"
                                                            ValidationGroup="1" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'> </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Size %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:TextBox ID="txtSizeLength" runat="server" Width="50px" Text='0' />&nbsp;x
                                                        <asp:TextBox ID="txtSizeWidth" runat="server" Width="50px" Text='0' />&nbsp;x
                                                        <asp:TextBox ID="txtSizeHeight" runat="server" Width="50px" Text='0' />&nbsp;<span><%--                                                    <tr>
                                                        <td style="width: 30%; height: 29px;">
                                                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_FirstPageProduct %>"></asp:Label>
                                                        </td>
                                                        <td style="vertical-align: middle; height: 29px">
                                                            <asp:CheckBox ID="chkSpecOffer" runat="server" />
                                                        </td>
                                                    </tr>--%></span>
                                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtSizeLength"
                                                            ValidationGroup="3" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="3000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                        <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtSizeWidth"
                                                            ValidationGroup="3" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="3000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                        <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtSizeHeight"
                                                            ValidationGroup="3" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="3000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                        <asp:Label ID="lSize" runat="server" Visible="false" Text="Error size" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formheaderfooter">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, Admin_Product_ShowRating %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:CheckBox ID="chkShowRating" runat="server" Checked="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Resource, Admin_Product_EnableRateProduct %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:CheckBox ID="chkEnableRating" runat="server" Checked="true" />
                                                    </td>
                                                </tr>--%>                                                <%=Resources.Resource.Admin_Product_Markers%>
                                                <tr>
                                                    <td class="formheader" colspan="2">
                                                        <h2>
                                                            <%=Resources.Resource.Admin_Product_Price%></h2>
                                                    </td>
                                                </tr>
                                                <tr class="formheaderfooter">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="lblBestSeller" runat="server" Text="<%$ Resources:Resource, Admin_Product_BestSeller %>"
                                                            EnableViewState="false" />
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:CheckBox ID="chkBestseller" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_Product_Recomended %>"
                                                            EnableViewState="false" />
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:CheckBox ID="chkRecommended" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_Product_New %>"
                                                            EnableViewState="false" />
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:CheckBox ID="chkNew" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px;">
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Resource, Admin_Product_OnSale %>"
                                                            EnableViewState="false" />
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        <asp:CheckBox ID="chkOnSale" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label runat="server" ID="lblMarkersDisabled" Text="<%$ Resources:Resource, Admin_Product_CanNotChange %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <!-- Categories -->
                                        <div class="tab-content">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lnAddLink" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnDelLink" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnMainLink" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="LinksProductTree" EventName="TreeNodePopulate" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <table class="table-p">
                                                        <tr>
                                                            <td colspan="3" class="formheader">
                                                                <h2>
                                                                    <asp:Localize ID="Localize_Admin_Product_CategoriesContainsProducts" runat="server"
                                                                        Text="<%$ Resources:Resource, Admin_Product_CategoriesContainsProducts %>" EnableViewState="false"></asp:Localize></h2>
                                                            </td>
                                                        </tr>
                                                        <tr class="formheaderfooter">
                                                            <td colspan="3">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 300px; height: 300px;">
                                                                <div style="height: 100%; width: 100%; overflow: scroll; background-color: White"
                                                                    id="divScroll">
                                                                    <asp:TreeView ID="LinksProductTree" ForeColor="Black" SelectedNodeStyle-BackColor="Blue"
                                                                        EnableClientScript="true" PopulateNodesFromClient="true" runat="server" ShowLines="True"
                                                                        ExpandImageUrl="images/loading.gif" BackColor="White" OnTreeNodePopulate="PopulateNode"
                                                                        EnableViewState="true">
                                                                        <SelectedNodeStyle BackColor="Yellow" CssClass="selectedNodeClass" />
                                                                    </asp:TreeView>
                                                                </div>
                                                            </td>
                                                            <td style="width: 70px; text-align: center">
                                                                <asp:Button ID="lnAddLink" runat="server" Text=">>>" OnClick="lnAddLink_Click" EnableViewState="false" />
                                                                <br />
                                                                <br />
                                                                <br />
                                                                <asp:Button ID="btnDelLink" runat="server" Text="<<<" OnClick="btnDelLink_Click"
                                                                    EnableViewState="false" />
                                                            </td>
                                                            <td style="vertical-align: top">
                                                                <div style="width: 99%">
                                                                    <div style="width: 285px; overflow-x: auto;">
                                                                        <asp:ListBox ID="ListlinkBox" runat="server" Height="283px" CssClass="TableContainer">
                                                                        </asp:ListBox>
                                                                    </div>
                                                                    <div>
                                                            </td>
                                                            <td style="width: 70px; text-align: center">
                                                                <asp:Button ID="btnMainLink" runat="server" Text="<%$ Resources: Resource, Admin_Product_MainLink %>"
                                                                    OnClick="btnMainLink_Click" EnableViewState="false" />
                                                                <br />
                                                                <br />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <script type="text/javascript">
                                                        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                                                            if ($("#divScroll").length && $(".selectedNodeClass").length) {
                                                                $("#divScroll").scrollTop($(".selectedNodeClass").position().top);
                                                            }
                                                        });
                                                    </script>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <!-- Prices -->
                                        <div class="tab-content">
                                            <table class="table-p">
                                                <tr>
                                                    <td class="formheader" colspan="2">
                                                        <h2>
                                                            <%= Resources.Resource.Admin_Product_SupplyPrice %></h2>
                                                    </td>
                                                </tr>
                                                <tr class="formheaderfooter">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 26px; vertical-align: middle;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_Product_Price %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 26px;">
                                                        &nbsp;<asp:TextBox ID="txtPrice" runat="server" Width="160px" Text="0" />
                                                        <asp:RangeValidator ID="rvPrice" runat="server" ControlToValidate="txtPrice" ValidationGroup="2"
                                                            Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="1000000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                        <asp:RequiredFieldValidator ID="RangeValidator9" runat="server" ControlToValidate="txtPrice"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 26px; vertical-align: middle;" onclick="focusoninput(this)">
                                                        <%=Resources.Resource.Admin_Product_Availability%>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 26px;">
                                                        &nbsp;<asp:TextBox ID="txtSupplyPrice" runat="server" Width="160px" Text="0" />
                                                        <asp:RangeValidator ID="rvSupplyPrice" runat="server" ControlToValidate="txtSupplyPrice"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="1000000000" MinimumValue="0" Type="Double" />
                                                        <asp:RequiredFieldValidator ID="RangeValidator10" runat="server" ControlToValidate="txtSupplyPrice"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 26px;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_DeliveryCost %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top; height: 26px;">
                                                        &nbsp;<asp:TextBox ID="txtShippingPrice" runat="server" Width="160px" Text="0" />
                                                        <asp:RangeValidator ID="rvShippingPrice" runat="server" ControlToValidate="txtShippingPrice"
                                                            ValidationGroup="1" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="1000000000" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                        <asp:RequiredFieldValidator ID="RangeValidator11" runat="server" ControlToValidate="txtShippingPrice"
                                                            ValidationGroup="1" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources:Resource,Admin_Product_EnterValidNumber %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px; vertical-align: middle;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Discount %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        &nbsp;<asp:TextBox ID="txtDiscount" runat="server" Width="160px" Text="0" /><asp:Label
                                                            ID="Label13" runat="server" Text="%"></asp:Label>
                                                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtDiscount"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber  %>'
                                                            MaximumValue="100" MinimumValue="0" Type="Double"> </asp:RangeValidator>
                                                        <asp:RequiredFieldValidator ID="RangeValidator8" runat="server" ControlToValidate="txtDiscount"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="true" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber  %>'>
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formheader" colspan="2">
                                                        <h2>
                                                            <%=Resources.Resource.Admin_Product_Description%></h2>
                                                    </td>
                                                </tr>
                                                <tr class="formheaderfooter">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 26px; vertical-align: middle;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_Product_Amount %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 26px;">
                                                        &nbsp;<asp:TextBox ID="txtAmount" runat="server" Width="160px" Text="1" />
                                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtAmount"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="100000" MinimumValue="0" Type="Integer"> </asp:RangeValidator>
                                                        <asp:RequiredFieldValidator ID="RangeValidator12" runat="server" ControlToValidate="txtAmount"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 26px; vertical-align: middle;" onclick="focusoninput(this)">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_Product_Unit %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 26px;">
                                                        &nbsp;<asp:TextBox ID="txtUnit" runat="server" Width="160px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px; vertical-align: middle;">
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_OrderByRequest %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        &nbsp;<asp:CheckBox ID="chkOrderByRequest" runat="server" Checked="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px; vertical-align: middle;">
                                                        <asp:Label ID="Label15" runat="server" Text="Ограничения на минимальное и максимальное количество товара в заказе"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        &nbsp;<asp:TextBox ID="txtMinAmount" runat="server" Width="70px"></asp:TextBox>
                                                        -
                                                        <asp:TextBox ID="txtMaxAmount" runat="server" Width="70px"></asp:TextBox>
                                                        <asp:RangeValidator ID="RangeValidator13" runat="server" ControlToValidate="txtMinAmount"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="100000" MinimumValue="0" Type="Integer"> </asp:RangeValidator>
                                                        <asp:RangeValidator ID="RangeValidator14" runat="server" ControlToValidate="txtMaxAmount"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="100000" MinimumValue="0" Type="Integer"> </asp:RangeValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%; height: 29px; vertical-align: middle;">
                                                        <asp:Label ID="Label16" runat="server" Text="Кратность количества" EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: middle; height: 29px">
                                                        &nbsp;<asp:TextBox ID="txtMultiplicity" runat="server" Width="160px" Text="1"></asp:TextBox>
                                                        <asp:RangeValidator ID="RangeValidator15" runat="server" ControlToValidate="txtMultiplicity"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>'
                                                            MaximumValue="100000" MinimumValue="1" Type="Integer"> </asp:RangeValidator>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMultiplicity"
                                                            ValidationGroup="2" Display="Dynamic" EnableClientScript="false" ErrorMessage='<%$ Resources: Resource, Admin_Product_EnterValidNumber %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <!-- Photos -->
                                        <div class="tab-content">
                                            <adv:ProductPhotos runat="server" ID="productPhotos" OnMainPhotoUpdate="productPhotos_OnMainPhotoUpdate"
                                                OnPhotoMessage="productPhotos_OnPhotoMessage" />
                                        </div>
                                        <!-- Videos -->
                                        <div class="tab-content">
                                            <adv:ProductVideos runat="server" ID="productVideos" />
                                        </div>
                                        <!-- Description -->
                                        <div class="tab-content">
                                            <table class="table-p">
                                                <tbody>
                                                    <tr>
                                                        <td class="formheader">
                                                            <h2>
                                                                <%=AdvantShop.Helpers.HtmlHelper.RenderSplitter()%></h2>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <CKEditor:CKEditorControl ID="fckDescription" runat="server" BasePath="~/ckeditor/"
                                                                Height="400px" Width="100%" EnableViewState="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formheader">
                                                            <h2>
                                                                <%=Resources.Resource.Admin_Product_ShortDescription%></h2>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <CKEditor:CKEditorControl runat="server" ID="fckBriefDescription" BasePath="~/ckeditor/"
                                                                Height="400px" Width="100%" EnableViewState="false" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <!-- Properties -->
                                        <div class="tab-content">
                                            <adv:ProductProperties ID="productProperties" runat="server" />
                                        </div>
                                        <!-- Custom options -->
                                        <div class="tab-content">
                                            <adv:ProductCustomOption ID="productCustomOption" runat="server" />
                                        </div>
                                        <!-- Meta -->
                                        <div class="tab-content">
                                            <asp:Label ID="lResult" runat="server" CssClass="mProductLabelInfo" ForeColor="Blue"
                                                Text="<%$ Resources:Resource, Admin_m_Product_Saved %>" Visible="false" EnableViewState="false"></asp:Label>
                                            <table class="table-p">
                                                <tbody>
                                                    <tr>
                                                        <td class="formheader" colspan="2">
                                                            <h2>
                                                                <%=Resources.Resource.Admin_Product_SEO%></h2>
                                                        </td>
                                                    </tr>
                                                    <tr class="formheaderfooter">
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; height: 26px; vertical-align: middle;">
                                                            <%=Resources.Resource.Admin_Catalog_UseDefaultMeta%>
                                                        </td>
                                                        <td style="vertical-align: middle; height: 26px;">
                                                            <asp:CheckBox runat="server" ID="chbDefaultMeta" Checked="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; height: 26px; vertical-align: middle;" onclick="focusoninput(this)">
                                                            <%=Resources.Resource.Admin_m_Product_HeadTitle%>
                                                        </td>
                                                        <td style="vertical-align: middle; height: 26px;">
                                                            <asp:TextBox ID="txtTitle" runat="server" Width="354" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; height: 29px;" onclick="focusoninput(this)">
                                                            <%=Resources.Resource.Admin_m_Product_MetaKeywords%>
                                                        </td>
                                                        <td style="vertical-align: middle; height: 29px;">
                                                            <asp:TextBox ID="txtMetaKeywords" runat="server" TextMode="MultiLine" Width="354" />
                                                        </td>
                                                    </tr>
                                                    <tr style="background-color: #eff0f1;">
                                                        <td style="width: 30%; height: 26px; vertical-align: middle;" onclick="focusoninput(this)">
                                                            <%=Resources.Resource.Admin_m_Product_MetaDescription%>
                                                        </td>
                                                        <td style="vertical-align: middle; height: 26px;">
                                                            <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" Width="354" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="PaymentMethod_description" ID="Label37" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_UseGlobalVariables %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <!-- Alternative -->
                                        <div class="tab-content">
                                            <adv:RelatedProducts runat="server" ID="relatedProducts" RelatedType="0"></adv:RelatedProducts>
                                        </div>
                                        <div class="tab-content">
                                            <adv:RelatedProducts runat="server" ID="alternativeProducts" RelatedType="1"></adv:RelatedProducts>
                                        </div>
                                        <!-- Taxes -->
                                        <div class="tab-content">
                                            <table class="table-p">
                                                <tr>
                                                    <td class="formheader">
                                                        <h2>
                                                            <asp:Localize ID="Localize_Admin_Product_Taxes" runat="server" Text="<%$ Resources:Resource, Admin_Product_Taxes  %>"
                                                                EnableViewState="false"></asp:Localize></h2>
                                                    </td>
                                                </tr>
                                                <tr class="formheaderfooter">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="margin-top: 20px">
                                                        <asp:DataList ID="datalistTaxes" runat="server" RepeatDirection="Horizontal" RepeatColumns="6"
                                                            RepeatLayout="Table" DataSourceID="SqlDataSourceTax">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <asp:HiddenField ID="hfTaxId" runat="server" Value='<%# Eval("TaxId")%>' />
                                                                    <asp:CheckBox ID="chbTax" runat="server" Text='<%# Eval("Name")%>' Checked='<%# Convert.ToBoolean(Eval("isUse")) %>' />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:SqlDataSource ID="SqlDataSourceTax" runat="server" SelectCommand="Select Tax.Name,Tax.TaxId , (select count(*) from Catalog.TaxMappingOnProduct where TaxMappingOnProduct.TaxId =Tax.TaxId and TaxMappingOnProduct.ProductId=@productId ) as isUse from Catalog.Tax where Catalog.Tax.enabled = 'true'"
                                                OnInit="sds_Init">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="productId" QueryStringField="ProductID" DbType="Int32" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <%=AdvantShop.Helpers.HtmlHelper.RenderSplitter()%>
            <td class="rightNavigation">
                <div id="rightPanel" class="rightPanel">
                    <adv:RightNavigation runat="server" ID="rightNavigation"></adv:RightNavigation>
                </div>
            </td>
            <td style="width: 10px;">
            </td>
        </tr>
    </table>
    <adv:PopupGridBrand ID="popUpBrand" runat="server"></adv:PopupGridBrand>
    <script type="text/javascript">

        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });

            $(".imgtooltip[abbr]").tooltip({
                delay: 10,
                showURL: false,
                bodyHandler: function () {
                    return $("<img/>").attr("src", $(this).attr("abbr"));
                }
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });

        function fillUrl() {
            var text = $('#<%=txtName.ClientID %>').val();
            var url = $('#<%=txtSynonym.ClientID %>').val();
            if ((text != "") & (url == "")) {
                $('#<%=txtSynonym.ClientID %>').val(translite(text));
            }
        }

        $(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");
            }
        });

        $('#<%= chbDefaultMeta.ClientID %>').click(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').val("");
                $('#<%=txtMetaDescription.ClientID %>').val("");
                $('#<%=txtMetaKeywords.ClientID %>').val("");

                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");

            } else {
                $('#<%=txtTitle.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaDescription.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaKeywords.ClientID %>').removeAttr("disabled");
            }
        });


        $('#<%=txtSynonym.ClientID %>').focus(fillUrl);
    </script>
</asp:Content>
