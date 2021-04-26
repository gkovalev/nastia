<%@ Page Language="C#" MasterPageFile="MasterPageSocial.master" AutoEventWireup="true"
    CodeFile="StaticPageViewSocial.aspx.cs" Inherits="StaticPageViewSocial" %>

<%@ MasterType VirtualPath="MasterPageSocial.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="content-owner">
            <h1>
                <%= page.PageName %>
            </h1>
            <div class="new-descr">
                <%= page.PageText %>
            </div>
        </div>
    </div>
</asp:Content>
