<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompareControl.ascx.cs"
    Inherits="UserControls_Catalog_CompareControl" %>
<%@ Import Namespace="Resources" %>
<input class="compare-checkbox" data-plugin="compare" data-compare-animation-obj=".compare-<%=ProductId%>"
    type="checkbox" id="<%= "chk_" + ProductId %>" <%= IsSelected ? "checked=checked" : "" %>
    value="<%=ProductId %>" data-compare-options='<%= GetOptions() %>' />
<label class="compare-label" for="<%= "chk_" + ProductId %>">
    <%= IsSelected ? Resource.Client_Catalog_AlreadyCompare + "(<a href=\"javascript:void(0);\">" + Resource.Client_Compare_View + "</a>)" : Resource.Client_Catalog_Compare%>
</label>
