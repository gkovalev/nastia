<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" ValidateRequest="false"
    AutoEventWireup="true" CodeFile="VotingHistory.aspx.cs" Inherits="Admin_VotingHistory" %>

<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="ContentVoting" ContentPlaceHolderID="cphMain" runat="server">
    <div style="text-align: center;" >
        <span class="AdminHead">
            <%=Resources.Resource.Admin_VotingHistory_Voting%></span>
        <br/>
        <span class="AdminSubHead">
            <%=Resources.Resource.Admin_VotingHistory_VotingHistory%></span><br/>
        <br/>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 400px; height: 100%;">
            <tr>
                <td align="center">
                    <div class="PagesNavigateVoice">
                        <%=GetPagesIndex()%>
                    </div>                    
                    <%=GetHtmlTableVoiceThemes()%>    
                    <div class="PagesNavigateVoice">
                        <%=GetPagesIndex()%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
