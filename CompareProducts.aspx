<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompareProducts.aspx.cs"
    Inherits="CompareProducts_Page" ValidateRequest="false" EnableViewState="false" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Design" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<%@ Register Src="UserControls/LogoImage.ascx" TagName="Logo" TagPrefix="adv" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Resources.Resource.Client_CompareProducts_Header %></title>
    <link rel="stylesheet" type="text/css" href="css/compare.css" />
    <link rel="stylesheet" type="text/css" href="css/styles.css"/>
    <link rel="stylesheet" type="text/css" href="css/styles-extra.css"/>
    <link rel="stylesheet" type="text/css" href="<%= "design/colors/" + DesignService.GetDesign("colorscheme") + "/css/styles.css" %>"
        id="colorcss" />
    <script type="text/javascript" src="js/jq/jquery-1.7.1.min.js"></script>
    <!--[if lt IE 10]>
        <script type="text/javascript" src="js/fix/PIE.js"></script>
    <![endif]-->
    <script type="text/javascript" src="js/fix/PIEInit.js"></script>
    
    <script type="text/javascript">

        $(function () {
            PIELoad(".btn");
        });

        function buyProduct(id) {
            $('#<%=hiddenProductID.ClientID%>').attr("value", id);
            document.getElementById('<%=btnBuyProduct.ClientID%>').click();
        }

        function deleteProduct(_productId) {
            $(".compareLoader_" + _productId).show();
            jQuery.ajax({
                url: "httphandlers/compareproducts/deleteproduct.ashx",
                dataType: "json",
                cache: false,
                data: { productId: _productId },
                success: function (successResult) {
                    if (successResult) {
                        $(".tdProduct_" + _productId).remove();
                        var colspan = $(".cc").attr('colspan');
                        $(".cc").attr('colspan', colspan - 1);
                        deleteProperties();

                        if (colspan == 1) {
                            closeWindow();
                        }
                    }
                }
            });
        }

        function deleteProperties() {
            $(".nCell").each(function () {
                var clearProperties = true;
                $(this).find("td.compare_col").each(function () {
                    if ($.trim($(this).text()) != "-") {
                        clearProperties = false;
                        return;
                    }
                });

                if (clearProperties) {
                    $(this).remove();
                }
            });
        }

        function closeWindow() {
            if (window.opener) {
                window.opener.location.reload(false);
            }
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="display: none">
            <input id="hiddenProductID" runat="server" value="" type="hidden" />
            <asp:Button ID="btnBuyProduct" runat="server" OnClick="btnBuyProduct_Click" />
            <asp:Button ID="btnDeleteProduct" runat="server" OnClick="btnDeleteProduct_Click" />
        </div>
        <table class="comp" style="width:auto">
            <tr>
                <td class="nc">
                    <span class="n">
                        <%=Resources.Resource.Client_CompareProducts_Header %></span>
                    <img src="images/compare/print.jpg" style="vertical-align: middle; cursor: pointer;"
                        onclick="window.print(); return false;" alt="" />
                </td>
                <td colspan="<%=ProductItems.Count%>" class="cc">
                    <img src="images/compare/close.jpg" onclick="closeWindow(); return false;" alt=""
                        style="cursor: pointer;" />
                </td>
            </tr>
            <tr class="cCell">
                <td rowspan="3" class="inf">
                    <adv:Logo ID="Logo" ImgHref='/' runat="server" />
                    <br />
                    <br />
                    <adv:StaticBlock ID="StaticBlockTop" runat="server" SourceKey="CompareProductsTop" />
                </td>
                <%foreach (var item in ProductItems)
                  {%>
                <td class="nb tdProduct_<%=item.ProductId%>">
                    <span class="pName">
                        <%=item.Name %></span><img style="vertical-align: inherit; cursor: pointer" src="images/compare/remove.jpg"
                            alt="<%=Resources.Resource.Client_CompareProducts_Delete %>" onclick="deleteProduct('<%=item.ProductId%>'); return false;" />
                </td>
                <%}%>
            </tr>
            <tr class="cCell">
                <%foreach (var item in ProductItems)
                  {%>
                <td class="nb tdProduct_<%=item.ProductId%>">
                    <img src='<%=FoldersHelper.GetImageProductPath(ProductImageType.Small, item.Photo, false) %>'
                        alt="" />
                </td>
                <%}%>
            </tr>
            <tr class="cCell">
                <%foreach (var item in ProductItems)
                  {%>
                <td class="tdProduct_<%=item.ProductId%>">
                   <%= item.Price > 0 && item.Amount > 0 ? AdvantShop.Controls.Button.RenderHtml(Resource.Client_Catalog_Add, AdvantShop.Controls.Button.eType.Buy, AdvantShop.Controls.Button.eSize.Middle, onClientClick: "buyProduct('" + item.ProductId + "')") : string.Empty%>
                    <%-- <%= AdvantShop.Controls.Button.RenderHtml(Resource.Client_Catalog_Add, AdvantShop.Controls.Button.eType.Buy, AdvantShop.Controls.Button.eSize.Middle, onClientClick: "buyProduct('" + item.ProductId + "')")%> -->
                    <%--<adv:Button runat="server" Type="Add" Size="Small" Text="<%$Resources:Resource, Client_Catalog_Add %>" OnClientClick="buyProduct('<%= item.ProductId%>'); return false;" />--%>
                </td>
                <%}%>
            </tr>
            <tr class="nCell">
                <td class="propN">
                    <%=Resources.Resource.Client_CompareProducts_Price %>
                </td>
                <%foreach (var item in ProductItems)
                  {%>
                <td class="compare_col price tdProduct_<%=item.ProductId%>">
                    <%=CatalogService.GetStringPrice(item.Price) %>
                </td>
                <%}%>
            </tr>
            <%foreach (var propertyName in PropertyNames)
              {%>
            <tr class="nCell">
                <td class="propN">
                    <%=propertyName%>
                </td>
                <%foreach (var item in ProductItems)
                  {%>
                <td class="compare_col tdProduct_<%=item.ProductId%>">
                    <%=item.Properties.Find(p => p.Name == propertyName).Value%>
                </td>
                <%}%>
            </tr>
            <%}%>
        </table>
    </div>
    </form>
</body>
</html>
