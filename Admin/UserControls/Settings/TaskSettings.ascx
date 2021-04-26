<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_TaskSettings" %>
<%@ Import Namespace="AdvantShop.Core.Scheduler" %>
<script type="text/javascript">
    var type = "<%=TimeIntervalType.Days.ToString() %>";

    function ChangeDL(obj, tr) {
        if ($(obj).val() == type) {
            $('#' + tr).show();
        }
        else {
            $('#' + tr).hide();
        }
    }

    $(function () {
        ChangeDL('#<% = ddlTypeHtml.ClientID  %>', 'trHtml');
        ChangeDL('#<% = ddlTypeXml.ClientID  %>', 'trXml');
        ChangeDL('#<% = ddlTypeYandex.ClientID  %>', 'trYandex');
        //Added by Evgeni
        ChangeDL('#<% = ddlTypeAmountUpdate.ClientID  %>', 'trAmountUpdate');
    });
</script>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Scheduling%>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SiteMapHtmlUpdating%>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Enabled%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:CheckBox runat="server" Checked="True" ID="chbEnabledHtml" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_StartInterval%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtTimeIntervalHtml">1</asp:TextBox>&nbsp;
            <asp:DropDownList runat="server" ID="ddlTypeHtml" onchange="javascript:ChangeDL(this,'trHtml')">
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trHtml">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_StartTime%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtHoursHtml" Text="0"/><%= Resources.Resource.Admin_CommonSettings_Hours%>
            &nbsp;
            <asp:TextBox runat="server" ID="txtMinutesHtml" Text="0"/><%= Resources.Resource.Admin_CommonSettings_Minutes%>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SiteMapXMLUpdating%>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Enabled%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:CheckBox runat="server" Checked="True" ID="chbEnabledXml" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_StartInterval%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtTimeIntervalXml" Text="1"/>&nbsp;
            <asp:DropDownList runat="server" ID="ddlTypeXml" onchange="javascript:ChangeDL(this,'trXml')">
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trXml">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_StartTime%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtHoursXml" Text="0" /><%= Resources.Resource. Admin_CommonSettings_Hours%>&nbsp;
            <asp:TextBox runat="server" ID="txtMinutesXml" Text="0"/><%= Resources.Resource. Admin_CommonSettings_Minutes%>
        </td>
    </tr>
    <% if (ShowYaMarket) { %>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_YMLyandexFileUpdating%>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Enabled%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:CheckBox runat="server" Checked="True" ID="chbEnabledYandex" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_StartInterval%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtTimeIntervalYandex" Text="1"/>&nbsp;
            <asp:DropDownList runat="server" ID="ddlTypeYandex" onchange="javascript:ChangeDL(this,'trYandex')">
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trYandex">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_StartTime%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtHoursYandex" Text="0" /><%= Resources.Resource.Admin_CommonSettings_Hours%>&nbsp;
            <asp:TextBox runat="server" ID="txtMinutesYandex" Text="0"/><%= Resources.Resource.Admin_CommonSettings_Minutes%>
        </td>
    </tr>
    </table>
    <!--Added By Evgeni to insert Amount Update-->
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize22" runat="server" 
                Text="Синхронизация остатков с 1С"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize23" runat="server" 
                Text="<%$ Resources:Resource, Admin_CommonSettings_Enabled%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:CheckBox runat="server" Checked="True" ID="chbEnableAmountUpdate" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize24" runat="server" 
                Text="<%$ Resources:Resource, Admin_CommonSettings_StartInterval%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtTimeIntervalAmountUpdate" Text="1"/>&nbsp;
            <asp:DropDownList runat="server" ID="ddlTypeAmountUpdate" 
                onchange="javascript:ChangeDL(this,'trAmountUpdate')">
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trAmountUpdate">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize25" runat="server" 
                Text="<%$ Resources:Resource, Admin_CommonSettings_StartTime%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtHoursAmountUpdate" Text="0" /><%= Resources.Resource.Admin_CommonSettings_Hours%>&nbsp;
            <asp:TextBox runat="server" ID="txtMinutesAmountUpdate" Text="0"/><%= Resources.Resource.Admin_CommonSettings_Minutes%>
        </td>
    </tr>
</table>
<table border="0" cellpadding="2" cellspacing="0">
    <% } %>
</table>
<div>
    <span style="color: gray;">
        <asp:Localize ID="Localize13" runat="server" Text="<%$Resources:Resource, Admin_TaskSettings_Warning %>"></asp:Localize>
    </span>
</div>
