<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdaterFromFile.aspx.cs"
    Inherits="Tools_core_UpdaterFromFile" MasterPageFile="MasterPage.master" %>

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
                            Update from file</h1>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 50%; margin: auto; text-align: center; padding-top: 5px; padding-bottom: 10px;">
                            <a href="backuper.aspx" target="_blank">Create backups</a>&nbsp;&nbsp;&nbsp;&nbsp;<a
                                href="iscustom.aspx" target="_blank">Check for changes</a>&nbsp;&nbsp;&nbsp;&nbsp;<a
                                    href="updater.aspx" target="_blank">Updater</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="">
                        <fieldset style="width: 50%; margin: auto; padding: 10px;">
                            <legend>
                                <h2>
                                    Attention</h2>
                            </legend>????? ????????????? ??????? ??????? ???? ?????? ????????????????? ? ??
                            ?????? ???????? ?????? ??????????<br />
                            ?????? ??????? ????????? ????? ????????? ???? ???????? ? ???? ??????.
                            <br />
                            -
                            <br />
                            ?????? ????? ?????????? ???? ????????????? ??????? ?????? "UPDATE".
                        </fieldset>
                        <fieldset style="width: 50%; margin: auto; padding: 10px; margin-top: 10px;">
                            ???????? ???? .zip. ???? ?????? ????????? ????????? ???? ? ?????????? ???????? ?????.
                            <br />
                            <br />
                            ??? ?? ? ????? ?????? ?????? ?????????? ???? "advantshop_sql.txt" ? SQL ?????????
                            ????????? ? ???? (???? ??????? ????? ?????)
                            <br />
                            <br />
                            <asp:Label ID="lblFileUploadNote" runat="server" Text="???? ?????? .zip "></asp:Label>&nbsp;<asp:FileUpload
                                runat="server" />
                        </fieldset>
                        <div style="width: 50%; margin: auto; padding: 10px; line-height: 18px;">
                            <br />
                            <asp:CheckBox ID="ckbUpdate" runat="server" Text="? ???????? ? ??????? ??? ? ??????? ??????" />
                            <br />
                            <asp:CheckBox ID="ckbUpdate1" runat="server" Text="? ?????? ??? ??????????? ????????? ?????" />
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
