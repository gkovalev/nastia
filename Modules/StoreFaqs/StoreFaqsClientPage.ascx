<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StoreFaqsClientPage.ascx.cs"
    Inherits="Advantshop.UserControls.Modules.StoreFaqs.StoreFaqsClientPage" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Modules" %>
<link rel="stylesheet" href="modules/storeFaqs/module-css/storeFaqs.css" />
<link rel="stylesheet" href="modules/storeFaqs/module-js/module-validate/validate.css" />
<h1>
    <asp:Label runat="server" ID="ltHeader" Text='<%$ Resources: StoreFaqs_StoreFaqs%>'></asp:Label>
</h1>
<div class="shop-Faqs">
    <asp:ListView ID="lvStoreFaqs" runat="server" ItemPlaceholderID="itemPlaceHolderId"
        OnPagePropertiesChanging="lvStoreFaqs_PagePropertiesChanging" OnPreRender="lvStoreFaqs_PreRender">
        <LayoutTemplate>
            <div runat="server" id="itemPlaceHolderId">
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div class="shop-Faqs-wrap pie" data-sr-item="<%# Eval("Id") %>" data-sr-parentid="<%# Eval("ParentId") %>">
                <asp:Panel CssClass="shop-Faqs-row" runat="server" Visible='<%# Convert.ToDouble(Eval("Rate").ToString()) > 0 %>'>
                    <div class="shop-Faqs-rating" data-sr-rating="<%# Convert.ToDouble(Eval("Rate")) %>">
                    </div>
                </asp:Panel>
                <div class="shop-Faqs-row">
                    <asp:Label CssClass="shop-Faqs-name" ID="lblEmail" runat="server" Text='<%# Eval("FaqerName") %>'></asp:Label>
                    <asp:Label CssClass="shop-Faqs-date" ID="lblDate" runat="server" Text='<%# ((DateTime)Eval("DateAdded")).ToString("dd MMMM yyyy, HH:mm") %>'></asp:Label>
                </div>
                <div class="shop-Faqs-row shop-Faqs-text">
                    <asp:Label ID="ltFaq" runat="server" Text='<%# Eval("Faq") %>'></asp:Label>
                </div>
                <div data-sr-form-btn="true">
                    <a href="javascript:void(0);" data-sr-reply="true">
                        <%= GetLocalResourceObject("StoreFaqs_Answer") %></a>
                </div>
                <%#  (bool)Eval("HasChild") ? RenderChild((List<StoreFaq>)Eval("ChildrenFaqs")) : ""%>
            </div>
        </ItemTemplate>
    </asp:ListView>
    <div class="shop-Faqs-paging" data-sr-paging="true">
        <asp:DataPager ID="dp" runat="server" PagedControlID="lvStoreFaqs" PageSize="2"
            ViewStateMode="Enabled" OnPagePropertiesChanging="dp_PagePropertiesChanging">
            <Fields>
                <asp:TemplatePagerField>
                    <PagerTemplate>
                        <span class="shop-Faqs-key">Ctrl + &larr; </span>
                    </PagerTemplate>
                </asp:TemplatePagerField>
                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowNextPageButton="False"
                    ShowLastPageButton="False" ShowPreviousPageButton="True" />
                <asp:NumericPagerField CurrentPageLabelCssClass="shop-Faqs-selected" NumericButtonCssClass="shop-Faqs-page" />
                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowNextPageButton="True"
                    ShowLastPageButton="False" ShowPreviousPageButton="False" />
                <asp:TemplatePagerField>
                    <PagerTemplate>
                        <span class="shop-Faqs-key">Ctrl + &rarr;</span>
                    </PagerTemplate>
                </asp:TemplatePagerField>
            </Fields>
        </asp:DataPager>
    </div>
</div>
<asp:Panel ID="adminPAnel" runat="server" Visible="false">

<div class="shop-Faqs-form-wrap">
    <div class="shop-Faqs-form-title">
        <%= GetLocalResourceObject("StoreFaqs_FormTitle")%></div>
    <ul class="shop-Faqs-form">
        <% if (ShowRatio)
           { %>
        <li class="shop-Faqs-form-row">
            <div class="shop-Faqs-form-rate">
            </div>
            <div id="hint">
            </div>
            <input type="hidden" runat="server" id="hfScope" name="hfScope" value="0" />
        </li>
        <% } %>
        <li class="shop-Faqs-form-row">
            <label for="txtFaqerName">
                <%= GetLocalResourceObject("StoreFaqs_FormName")%></label>
            <div class="shop-Faqs-form-input">
                <div class="input-wrap">
                    <asp:TextBox ID="txtFaqerName" data-plugin="validelem" data-validelem-group="StoreFaqs"
                        data-validelem-methods="['required']" runat="server" /></div>
            </div>
        </li>
        <li class="shop-Faqs-form-row">
            <label for="txtEmail">
                <%= GetLocalResourceObject("StoreFaqs_FaqerEmail") %></label>
            <div class="shop-Faqs-form-input">
                <div class="input-wrap">
                    <asp:TextBox ID="txtEmail" runat="server" data-plugin="validelem" data-validelem-group="StoreFaqs"
                        data-validelem-methods="['required', 'email']"></asp:TextBox></div>
            </div>
        </li>
        <li class="shop-Faqs-form-row">
            <label for="txtFaq">
                <%= GetLocalResourceObject("StoreFaqs_Faq")%></label>
            <div class="shop-Faqs-form-input">
                <div class="textarea-wrap">
                    <asp:TextBox ID="txtFaq" runat="server" TextMode="MultiLine" data-validelem-group="StoreFaqs"
                        data-plugin="validelem" data-validelem-methods="['required']"></asp:TextBox></div>
            </div>
        </li>
        <li runat="server" id="liError" class="shop-Faqs-li-error"></li>
        <li class="shop-Faqs-form-row">
            <asp:LinkButton CssClass="btn btn-submit btn-middle" ID="btn" runat="server" OnClick="btnClick"
                Text='<%$Resources: StoreFaqs_Send  %>' data-validelem-btn="StoreFaqs"></asp:LinkButton>
        </li>
    </ul>
</div>
</asp:Panel>
<script src="modules/StoreFaqs/module-js/module-localization/<%=SettingsMain.Language %>.js"></script>
<script src="modules/StoreFaqs/module-js/module-rate/jquery.ratyStoreFaqs.js"></script>
<script src="modules/StoreFaqs/module-js/module-validate/validate.js"></script>
<script src="modules/StoreFaqs/module-js/StoreFaqs.js"></script>
