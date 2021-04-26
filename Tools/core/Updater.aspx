<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Updater.aspx.cs" Inherits="Tools_core_Updater"
    MasterPageFile="MasterPage.master" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools - Advantshop updater</title>
    <script type="text/javascript" src="../../js/jq/jquery-1.7.1.min.js"></script>
</asp:Content>
<asp:Content runat="server" ID="cntMain" ContentPlaceHolderID="main">
    <div style="width: 100%; font-size: 16px;">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="border-bottom: 1px solid black; text-align: center;">
                        <h1>
                            Updater</h1>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 50%; margin: auto; text-align: center; padding-top: 5px; padding-bottom: 10px;">
                            <a href="backuper.aspx" target="_blank">Create backups</a>&nbsp;&nbsp;&nbsp;&nbsp;<a
                                href="iscustom.aspx" target="_blank">Check for changes</a>&nbsp;&nbsp;&nbsp;&nbsp;<a
                                    href="updaterfromfile.aspx" target="_blank">Update from file</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="">
                        <div style="width: 50%; margin: auto; padding: 10px;">
                            <div style="margin-bottom: 30px;">
                                <span style="font-weight: bold;">Текущая версия магазина:</span>
                                <asp:Label runat="server" ID="lblCurrentVersion"><%= AdvantShop.Configuration.SettingsGeneral.SiteVersion%></asp:Label>
                            </div>
                            <div style="">
                                <span style="font-weight: bold;">Последняя версия магазина:</span>
                                <asp:Label runat="server" ID="lblLastVersion"></asp:Label>
                                <br />
                                <div id="divMoreInf" runat="server">
                                    <span id="spanMore" style="cursor: pointer; text-decoration: underline;" onclick="showHideInformation()">
                                        Подробнее о изменениях:</span>
                                    <fieldset id="divMore" style="display: none; margin: auto; padding: 10px;">
                                        <asp:Literal runat="server" ID="lblVersionInformation"></asp:Literal>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <fieldset style="width: 50%; margin: auto;">
                            <legend>
                                <h2>
                                    Attention</h2>
                            </legend>Просим сделать резервные копии исходного кода магазина и базы данных.
                            <br />
                            <br />
                            Только после завершения всех приготовлений нажмите кнопку "UPDATE".
                        </fieldset>
                        <div style="width: 50%; margin: auto; padding: 10px; line-height: 18px;">
                            <br />
                            <asp:CheckBox ID="ckbUpdate" runat="server" Text="Я нахожусь в трезвом уме и твердой памяти" />
                            <br />
                            <asp:CheckBox ID="ckbUpdate1" runat="server" Text="Я сделал все необходимые резервные копии" />
                            <br />
                            <br />
                            <asp:Button ID="btn_update" runat="server" OnClick="btnUpdate_Click" Text="UPDATE"
                                Style="display: none;" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <script type="text/javascript">
        function showHideInformation() {
            if ($("#divMore").is(":visible")) {
                $("#divMore").hide();
            } else {
                $("#divMore").show();
            }
        }

        $("#<%=ckbUpdate.ClientID%>").click(function () {
            if ($("#<%= ckbUpdate1.ClientID%>").is(":checked") && $("#<%= ckbUpdate.ClientID%>").is(":checked")) {
                $("#<%= btn_update.ClientID%>").show();
            } else {
                $("#<%= btn_update.ClientID%>").hide();
            }
        });

        $("#<%=ckbUpdate1.ClientID%>").click(function () {
            if ($("#<%= ckbUpdate1.ClientID%>").is(":checked") && $("#<%= ckbUpdate.ClientID%>").is(":checked")) {
                $("#<%= btn_update.ClientID%>").show();
            } else {
                $("#<%= btn_update.ClientID%>").hide();
            }
        });
        
    </script>
</asp:Content>
