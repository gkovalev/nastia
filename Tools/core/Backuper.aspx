<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Backuper.aspx.cs" Inherits="Tools_core_Backuper"
    MasterPageFile="MasterPage.master" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools - Compare Code Masks</title>
    <script type="text/javascript" src="../../js/jq/jquery-1.7.1.min.js"></script>
</asp:Content>
<asp:Content runat="server" ID="cntMain" ContentPlaceHolderID="main">
    <div style="width: 100%; font-size: 16px;">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="border-bottom: 1px solid black; text-align: center;">
                        <h1>
                            Backuper</h1>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 50%; margin: auto; text-align: center; padding: 10px; margin-top: 0;">
                            <a href="updater.aspx" target="_blank">Updater</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="iscustom.aspx"
                                target="_blank">Check for changes</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="updaterfromfile.aspx"
                                    target="_blank">Update from file</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="">
                        <fieldset style="width: 50%; margin: auto; padding-top: 5px; padding-bottom: 10px;
                            line-height: 18px; margin-top: 20px;">
                            <legend>
                                <h2>
                                    Message</h2>
                            </legend>
                            <br />
                            Создание резервный копий базы данных (SQL скрипт с данными) и .zip архива исходного
                            кода интернет магазина.
                            <br />
                            <br />
                            Нажимая на кнопки создания резервных копий обнавляются старые.
                        </fieldset>
                        <div id="divLinks" runat="server" style="width: 50%; margin: auto; padding: 10px;
                            line-height: 18px;">
                            <asp:LinkButton ID="lnkFileSql" runat="server" OnClick="lnkFileSql_Click"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkFileCode" runat="server" OnClick="lnkFileCode_Click"></asp:LinkButton>
                        </div>
                        <div style="width: 50%; margin: auto; padding: 10px; line-height: 18px;">
                            <asp:Button ID="btn_BackupSql" runat="server" OnClick="btnBackupSql_Click" Text="Создать резервную копию базы" />
                            <asp:Button ID="btn_BackupCode" runat="server" OnClick="btnBackupCode_Click" Text="Создать резервную копию кода" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
