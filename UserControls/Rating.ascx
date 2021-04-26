<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Rating.ascx.cs" Inherits="UserControls_Rating" %>
<%@ Import Namespace="Resources" %>
<div class="rating">
    <div id="rating_<%=ProductId %>" class="<%= ReadOnly ? "rating-readonly" : string.Empty  %>">
    </div>
    <input type="hidden" value="<%= Rating %>" id="rating_hidden_<%=ProductId %>" />
</div>
