<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IsCustom.aspx.cs" Inherits="Tools_core_IsCustom"
    MasterPageFile="MasterPage.master" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools - Is custom project</title>
</asp:Content>
<asp:Content runat="server" ID="cntMain" ContentPlaceHolderID="main">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr>
                <td style="border-bottom: 1px solid black; text-align: center;">
                    <h1>
                        Check for changes</h1>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 50%; margin: auto; text-align: center; padding-top: 5px; padding-bottom: 10px;">
                        <a href="backuper.aspx" target="_blank">Create backups</a>&nbsp;&nbsp;&nbsp;&nbsp;<a
                            href="updater.aspx" target="_blank">Updater</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="updaterfromfile.aspx"
                                target="_blank">Update from file</a>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 50%; margin: auto; padding: 10px;">
                        <asp:Button ID="btnCompareCode" runat="server" OnClick="btnCompareCode_OnClick" Text="Check source" />&nbsp;&nbsp;&nbsp;<asp:Button
                            ID="btnCompareBase" runat="server" OnClick="btnCompareBase_OnClick" Text="Check base" /><br />
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                    </div>
                    <fieldset style="width: 50%; margin: auto; padding: 10px;">
                        <legend>
                            <h2>
                                Report</h2>
                        </legend>
                        <asp:Literal ID="ltrlReport" runat="server">No reports</asp:Literal>
                    </fieldset>
                    <div id="divLinks" runat="server" style="width: 50%; margin: auto; padding: 10px;
                        line-height: 18px;">
                        <asp:LinkButton ID="lnkFileSql" runat="server" OnClick="lnkFileSql_Click"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkFileCode" runat="server" OnClick="lnkFileCode_Click"></asp:LinkButton>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
