<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductViewChanger.ascx.cs"
    Inherits="UserControls_Catalog_ProductViewChanger" %>
<%@ Import Namespace="Resources" %>
<ul class="views">
    <!--tile-->
    <% if (IsSelectedView(0))
       {
    %>
    <li class="selected" title="<%= Resource.Client_Catalog_Tiles %>"><span class="vtiles">
    </span></li>
    <%
        }
       else
       {%>
    <li>
        <asp:LinkButton runat="server" ID="lbTiles" OnClick="lbTiles_Click" ToolTip='<%$Resources:Resource, Client_Catalog_Tiles%>' CssClass="vtiles"></asp:LinkButton></li>
    <%} %>
    <!--list-->
    <% if (IsSelectedView(1))
       {
    %>
    <li class="selected" title="<%= Resource.Client_Catalog_List %>"><span class="vlist">
    </span></li>
    <%
        }
       else
       {%>
    <li>
        <asp:LinkButton runat="server" ID="lbList" OnClick="lbList_Click" ToolTip='<%$Resources:Resource, Client_Catalog_List%>' CssClass="vlist"></asp:LinkButton></li>
    <%} %>
    <!--table-->
    <% if (IsSelectedView(2))
       {
    %>
    <li class="selected" title="<%= Resource.Client_Catalog_Table %>"><span class="vtable">
    </span></li>
    <%
        }
       else
       {%>
    <li>
        <asp:LinkButton runat="server" ID="lbTable" OnClick="lbTable_Click" ToolTip='<%$Resources:Resource, Client_Catalog_Table%>'  CssClass="vtable"></asp:LinkButton></li>
    <%} %>
</ul>
