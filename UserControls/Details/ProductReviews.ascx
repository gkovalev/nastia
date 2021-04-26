<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductReviews.ascx.cs"
    Inherits="UserControls_ProductReviews" %>
<%@ Import Namespace="Resources" %>
<div class="reviews" data-plugin="reviews" data-reviews-options="{entityId:<%= EntityId %>, entityType: <%= (int)EntityType %>}"></div>