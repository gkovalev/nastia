<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Import1C.aspx.cs" Inherits="Admin_Import1C" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainDiv" runat="server">
        <center>
            <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_Import1C_Catalog %>"></asp:Label><br />
            <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_Import1C_CatalogUpload %>"></asp:Label>
            <br />
        </center>
        <br />
        <br />
        <asp:Panel ID="pUploadXml" runat="server">
            <center>
                <table style="text-align: center;">
                    <%-- <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_Import1C_XMLPath %>" />
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                        <asp:Button ID="btnLoad" runat="server" Height="22px" Text="<%$ Resources:Resource, Admin_Import1C_Upload %>"
                            OnClick="btnLoad_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_Import1C_AddPhotos %>" />
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUpload2" runat="server" />
                        <asp:Button ID="AddPhoto" runat="server" Height="22px" Text="<%$ Resources:Resource, Admin_Import1C_AddPhoto %>"
                            OnClick="AddPhoto_Click" />
                    </td>
                </tr>--%>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="Label2" runat="server" Text="Пусть к зип файлу" />
                        </td>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            <asp:Button ID="btnLoad" runat="server" Height="22px" Text="<%$ Resources:Resource, Admin_Import1C_Upload %>"
                                OnClick="btnLoad_Click" />
                        </td>
                    </tr>
                </table>
                <div style="text-align: center; margin-top: 20px;">
                    <asp:CheckBox ID="chboxDisableProducts" runat="server" Text="<%$ Resources:Resource, Admin_Import1C_DeactiveProducts %>" />
                </div>
            </center>
        </asp:Panel>
        <center>
            <div style="text-align: left; font-family: Courier New; width: 400px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />
                    </Triggers>
                    <ContentTemplate>
                        <center>
                            <asp:Panel ID="progressBar" runat="server" Height="12px" Visible="false">
                                <asp:Label ID="lProgress" runat="server" Text="0%" Visible="false"></asp:Label>
                                <span>(</span><asp:Label ID="lDone" runat="server" Text="0" Visible="false"></asp:Label><span>/</span><asp:Label
                                    ID="lSummary" runat="server" Text="0" Visible="false"></asp:Label><span>)</span>
                            </asp:Panel>
                        </center>
                        <%--<div style="width: 100%; height: 17px;" class="progressInDiv" id="ctl00_cphMain_InDiv">
                        &nbsp;
                    </div>--%>
                        <br />
                        <asp:Panel ID="pSummary" runat="server" Visible="false">
                            <span>
                                <%= Resources.Resource.Admin_Import1C_AddProducts%></span><asp:Label ID="lAdded"
                                    runat="server" Text="0"></asp:Label><br />
                            <span>
                                <%= Resources.Resource.Admin_Import1C_UpdateProducts%></span><asp:Label ID="lUpdated"
                                    runat="server" Text="0"></asp:Label><br />
                            <span>
                                <%= Resources.Resource.Admin_Import1C_ProductsWithError %></span><asp:Label ID="lErrors"
                                    runat="server" Text="0"></asp:Label>
                        </asp:Panel>
                        <br />
                        <center>
                            <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" /><br />
                            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label><br />
                            <%--<asp:Repeater ID="rLog" runat="server" DataSourceID="sdsLog" OnItemDataBound="rLog_ItemDataBound">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("message")%>' CssClass='<%# Eval("mtype").ToString() %>' /><br />
                        </ItemTemplate>
                    </asp:Repeater>--%>
                            <asp:HyperLink ID="hlDownloadImportLog" runat="server" Visible="false" Text="<%$ Resources:Resource, Admin_Import1C_DownloadImportLog%>"
                                NavigateUrl="~/HttpHandlers/ImportLog.ashx" /><asp:Label ID="lImportLogSize" runat="server"
                                    Visible="false"></asp:Label>
                        </center>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </center>
        <asp:Timer ID="timer" runat="server" Interval="10" Enabled="false" OnTick="timer_Tick">
        </asp:Timer>
        <asp:SqlDataSource ID="sqlDataSourceLog" runat="server" SelectCommand="SELECT TOP(10) [id], [mtype], [message] FROM [Catalog].[ImportLog] ORDER BY [id] DESC"
            InsertCommand="INSERT INTO [Catalog].[ImportLog] (message, mtype) VALUES (@message, @mtype)"
            DeleteCommand="DELETE [Catalog].[ImportLog]" OnInit="sqlDataSourceLog_Init">
            <InsertParameters>
                <asp:Parameter Name="message" Type="String" />
                <asp:Parameter Name="mtype" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
    </div>
    <div id="notInTariff" runat="server" visible="false">
        <center>
            <h2>
                Данный функционал не доступен на вашем тарифном плане
            </h2>
        </center>
    </div>
</asp:Content>
