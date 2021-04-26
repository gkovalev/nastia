<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintOrder.aspx.cs" Inherits="PrintOrder" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Admin/css/AdminStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function position_this_window() {
            var x = (screen.availWidth - 770) / 2;
            window.resizeTo(762, 662);
            window.moveTo(Math.floor(x), 50);
        }
    </script>
    <title></title>
</head>
<body onload="position_this_window();window.print();">
    <form id="form1" runat="server">
    <div style="font-family: Arial; text-align: center;">
        <table style="width: 98%; padding-left: 10px;">
            <tr>
                <td>
                    <center>
                        <asp:Label ID="lblOrderID" CssClass="AdminHead" runat="server"></asp:Label><br />
                        <asp:Label ID="lblOrderStatus" CssClass="AdminSubHead" runat="server"></asp:Label><br />
                        <br />
                    </center>
                    <b>
                    <%=Resources.Resource.Admin_ViewOrder_Date%>
                    </b>
                    <asp:Label ID="lOrderDate" runat="server"></asp:Label>
                    <br />
                    <b>
                        <%=Resources.Resource.Admin_ViewOrder_Number%></b>
                    <asp:Label ID="lNumber" runat="server"></asp:Label>
                    <br />
                    <b>
                        <%=Resources.Resource.Admin_ViewOrder_StatusComment%></b>
                    <asp:Label ID="lStatusComment" runat="server"></asp:Label>
                    <br />
                    <br />
                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 34%; vertical-align: top">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Billing %>"></asp:Label>
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblBilling" runat="server"></asp:Label>
                            </td>
                            <td style="width: 33%; vertical-align: top">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Shipping %>"></asp:Label>
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblShipping" runat="server"></asp:Label>
                            </td>
                            <td style="width: 32%; vertical-align: top">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ShippingMethod %>"></asp:Label>
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblShippingMethod" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_PaymentType %>"></asp:Label>
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblPaymentType" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <!--Added by Evgeni
                    <div style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 13px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">
                        Для оплаты в банке по системе РАСЧЕТ (ЕРИП) сообщите кассиру следующую 
                        информацию:</div>
                    <div style="color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 13px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);">
                        <div style="line-height: 14px; font-size: 12px; font-family: Arial, Helvetica, sans-serif;">
                            <span style="font-size: 16px;">
                            <span style="font-family: tahoma, geneva, sans-serif;"><strong>название 
                            интернет-магазина в ЕРИП:<span class="Apple-converted-space">&nbsp;</span><a 
                                href="http://instrument-opt.by/" style="color: rgb(17, 85, 204);" target="_blank">instrument-opt.by</a></strong></span></span></div>
                        <div style="line-height: 14px; font-size: 12px; font-family: Arial, Helvetica, sans-serif;">
                            <span style="font-size: 16px;">
                            <span style="font-family: tahoma, geneva, sans-serif;"><strong>организация:ООО 
                            «НесТулс»&nbsp;
                            <span style="color: rgb(34, 34, 34); font-family: Calibri; font-size: 16px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none;">
                            <span class="Apple-converted-space">&nbsp;</span></span><b 
                                style="color: rgb(34, 34, 34); font-family: Calibri; font-size: 16px; font-style: normal; font-variant: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255);"><span><font 
                                face="Times New Roman">р/с № 3012062986615 ЗАО «Дельта Банк» МФО 153001281</font></span></b></strong></span></span></div>
                        <div style="line-height: 14px; font-size: 12px; font-family: Arial, Helvetica, sans-serif;">
                            <span style="font-size: 16px;">
                            <span style="font-family: tahoma, geneva, sans-serif;"><strong>номер Вашего 
                            заказа :
                    <asp:Label ID="lOrder0" runat="server"></asp:Label>
                            </strong></span></span>
                        </div>
                        <div style="line-height: 14px; font-size: 12px; font-family: Arial, Helvetica, sans-serif;">
                            <span style="font-size: 16px;">
                            <span style="font-family: tahoma, geneva, sans-serif;"><strong>сумма к оплате :
                                    <b>
                                        <asp:Label ID="lblTotalPrice0" runat="server"></asp:Label></b>
                                </strong></span></span>
                        </div>
                    </div> -->
&nbsp;<br />
                    <br />
                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_OrderItem %>"></asp:Label>
                    <br />
                    <br />
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="grid-main">
                        <tr class="GridView_HeaderStyle">
                            <td class="grid-top">
                                <b>
                                    <asp:Label ID="lblAdmin_ViewOrder_ItemName" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemName %>"></asp:Label></b>
                            </td>
                            <td class="grid-top">
                                <b>
                                    <asp:Label ID="lblAdmin_ViewOrder_CustomOptions" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_CustomOptions %>"></asp:Label></b>
                            </td>
                            <td class="grid-top" style="text-align: center;">
                                <b>
                                    <asp:Label ID="lblAdmin_ViewOrder_Price" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Price %>"></asp:Label></b>
                            </td>
                            <td class="grid-top" style="width: 240px; text-align: center;">
                                <b>
                                    <asp:Label ID="lblAdmin_ViewOrder_ItemAmount" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemAmount %>"></asp:Label></b>
                            </td>
                            <td class="grid-top" style="width: 200px">
                                <b>
                                    <asp:Label ID="lblAdmin_ViewOrder_ItemCost" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemCost %>"></asp:Label></b>
                            </td>
                        </tr>
                        <asp:Repeater ID="DataListOrderItems" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="grid-left">
                                        <%# (EnumItemType)Eval("ItemType") == EnumItemType.Product ? Eval("Name") : Resources.Resource.Client_GiftCertificate %>
                                    </td>
                                    <td class="grid-left">
                                        <%#RenderSelectedOptions((IList<EvaluatedCustomOptions>)Eval("SelectedOptions"))%>
                                    </td>
                                    <td class="grid-even" style="width: 200px; text-align: center;">
                                        <%#CatalogService.GetStringPrice((decimal)Eval("Price"), 1, ordCurrency.CurrencyCode, ordCurrency.CurrencyValue)%>
                                    </td>
                                    <td class="grid-even" style="width: 240px; text-align: center;">
                                        <%#Eval("Amount")%>
                                    </td>
                                    <td class="grid-even" style="width: 200px">
                                        <%#CatalogService.GetStringPrice((decimal)Eval("Price"), (int)Eval("Amount"), ordCurrency.CurrencyCode, ordCurrency.CurrencyValue)%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr>
                                    <td class="grid-left_alt">
                                        <%# (EnumItemType)Eval("ItemType") == EnumItemType.Product ? Eval("Name") : Resources.Resource.Client_GiftCertificate%>
                                    </td>
                                    <td class="grid-left_alt">
                                        <%#RenderSelectedOptions((IList<EvaluatedCustomOptions>) Eval("SelectedOptions"))%>
                                    </td>
                                    <td class="grid-even_alt" style="width: 200px; text-align: center;">
                                        <%#CatalogService.GetStringPrice((decimal)Eval("Price"), 1, ordCurrency.CurrencyCode, ordCurrency.CurrencyValue)%>
                                    </td>
                                    <td class="grid-even_alt" style="width: 240px; text-align: center;">
                                        <%#Eval("Amount")%>
                                    </td>
                                    <td class="grid-even_alt" style="width: 200px">
                                        <%#CatalogService.GetStringPrice((decimal)Eval("Price"), (int)Eval("Amount"), ordCurrency.CurrencyCode, ordCurrency.CurrencyValue)%>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </table>
                    <asp:Panel ID="pnlSummary" runat="server">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="background-color: #FFFFFF; text-align: right">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemCost2 %>"></asp:Label>&nbsp;
                                </td>
                                <td style="background-color: #FFFFFF; width: 150px">
                                    <asp:Label ID="lblProductPrice" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDiscount" runat="server" visible="false">
                                <td style="background-color: #FFFFFF; text-align: right">
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemDiscount %>"></asp:Label>&nbsp;
                                </td>
                                <td style="background-color: #FFFFFF; width: 150px">
                                    <asp:Label ID="lblOrderDiscount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trCertificate" runat="server" visible="false">
                                <td style="background-color: #FFFFFF; text-align: right">
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Certificate %>"></asp:Label>:&nbsp;
                                </td>
                                <td style="background-color: #FFFFFF; width: 150px">
                                    <asp:Label ID="lblCertificate" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trCoupon" runat="server" visible="false">
                                <td style="background-color: #FFFFFF; text-align: right">
                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Coupon%>"></asp:Label>:&nbsp;
                                </td>
                                <td style="background-color: #FFFFFF; width: 150px">
                                    <asp:Label ID="lblCoupon" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #FFFFFF; text-align: right">
                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ShippingPrice %>"></asp:Label>&nbsp;
                                </td>
                                <td style="background-color: #FFFFFF; width: 150px">
                                    <asp:Label ID="lblShippingPrice" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <asp:Literal ID="literalTaxCost" runat="server"></asp:Literal>
                            <tr>
                                <td style="background-color: #FFFFFF; text-align: right">
                                    <b>
                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_TotalPrice %>"></asp:Label>&nbsp;</b>
                                </td>
                                <td style="background-color: #FFFFFF; width: 150px">
                                    <b>
                                        <asp:Label ID="lblTotalPrice" runat="server"></asp:Label></b>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="lblUserComments" runat="server" Text="<%$ Resources:Resource, Client_PrintOrder_YourComment %>"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
