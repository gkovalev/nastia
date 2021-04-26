<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterPrice.ascx.cs" Inherits="UserControls_FilterPrice" %>
<%@ Import Namespace="AdvantShop.Repository.Currencies" %>
<!--noindex-->
<article class="block-uc-inside" data-plugin="expander">
    <h4 class="title" data-expander-control="#filter-price">
        <%= Resources.Resource.Client_Catalog_PriceFilter %>
        (
        <%= CurrencyService.CurrentCurrency.Symbol %>
        )</h4>
    <div class="content-price" id="filter-price">
        <div class="slider">
            <span class="min">
                <%= Min %></span> <span class="max">
                    <%= Max %></span>
        </div>
    </div>
</article>
<input type="hidden" id="sliderCurentMin" value="<%= CurValMin %>" />
<input type="hidden" id="sliderCurentMax" value="<%= CurValMax %>" />
<!--/noindex-->
