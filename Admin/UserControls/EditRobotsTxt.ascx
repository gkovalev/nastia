<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditRobotsTxt.ascx.cs" Inherits="Admin_UserControls_EditRobotsTxt" %>
<span class="spanSettCategory"><asp:Localize ID="Localize_Admin_UserControl_EditRobotsTxt_EditTab" runat="server" 
    Text="<%$ Resources:Resource, Admin_UserControl_EditRobotsTxt_EditTab %>"></asp:Localize></span>
<hr color="#C2C2C4" size="1px" />
<asp:TextBox ID="txtRobots" runat="server" TextMode="MultiLine" CssClass="txtRobotsStyle" ></asp:TextBox>