<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductVideoView.ascx.cs" Inherits="UserControls_ProductVideoView" %>

<div id="videos"> 
</div>
 <asp:Label ID="lblScript" runat="server" Text="Label"></asp:Label>
  <asp:Label ID="lblVideoEmbedLabel" runat="server" Text=""></asp:Label>
<asp:Repeater ID="rprVideos" runat="server">
    <ItemTemplate>
         <div class="prod_video">
            <%# Eval("PlayerCode") %>
            <%# !String.IsNullOrEmpty(Eval("Description").ToString()) ? "<div class='prod_video_descr'>" + Eval("Description").ToString() + "</div>" : ""%>
        </div>
    </ItemTemplate>
</asp:Repeater>
