<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_News.aspx.cs" Inherits="Admin_m_News"
    MasterPageFile="m_MasterPage.master" ValidateRequest="false" %>


<asp:Content ID="contentCenter" ContentPlaceHolderID="cphCenter" runat="server">
    <div>
        <div style="text-align: center;">
            <asp:Label ID="lblCustomer" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_m_News_SubHeader %>"></asp:Label>
        </div>
        <asp:Panel ID="pnlAdd" runat="server" Height="84px" Width="100%">
            <div style="text-align: center;">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>&nbsp;
            </div>
            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right">
                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Category %>" />&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 27px;">
                        <asp:DropDownList ID="dboNewsCategory" runat="server" DataSourceID="SqlDataSource1"
                            DataTextField="Name" DataValueField="NewsCategoryID" Width="224px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 27px; text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Data %>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 27px;">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtDate" runat="server" Width="100px" />
                                        <asp:TextBox ID="txtTime" runat="server" Width="50px"></asp:TextBox><span id="validTime"
                                            style="color: red; display: none;">*</span>
                                        <ajaxToolkit:MaskedEditExtender ID="meeTime" runat="server" TargetControlID="txtTime"
                                            Mask="99:99" AutoComplete="False" MaskType="Time">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td>
                                        <asp:Image ID="popupDateTo" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                        <ajaxToolkit:CalendarExtender TargetControlID="txtDate" PopupButtonID="popupDateTo"
                                            CssClass="popupCalendar" BehaviorID="calendar" ID="CalendarExtender1" runat="server"
                                            PopupPosition="Right">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:Label ID="Label2" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_m_News_Required %>"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDate"
                                            ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; height: 27px; text-align: right; vertical-align: middle;">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Title %>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 27px;">
                        <asp:TextBox ID="txtTitle" runat="server" Width="300px"></asp:TextBox>
                        <asp:Label ID="Label12" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_m_News_Required %>"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTitle"
                            ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 12px; text-align: right">
                        <asp:Label ID="lblStringID" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Url%>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 12px">
                        <asp:TextBox ID="txtStringID" runat="server" Width="224px"></asp:TextBox>
                        <asp:Label ID="Label6" runat="server" Font-Italic="True" ForeColor="DarkGray" Text="<%$ Resources:Resource, Admin_m_News_Required %>"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtStringID"
                            ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                        <br />
                        <%--<asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Size="Smaller" ForeColor="Gray" Text="<%$ Resources:Resource, Admin_m_News_UrlFormat %>"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 45%; text-align: right; height: 29px;">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Picture %>"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:Panel ID="pnlImage" runat="server" Width="100%">
                            &nbsp;<asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_m_News_CurrentImage %>"></asp:Label>
                            <br />
                            <asp:Image ID="Image1" runat="server" />
                            <br />
                            <asp:Button ID="btnDeleteImage" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>"
                                OnClick="btnDeleteImage_Click" />
                            <br />
                        </asp:Panel>
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="308px" Height="20px" />
                        <asp:Label ID="lblImage" runat="server" Text="Label" Visible="False"></asp:Label>
                        <br />
                        <asp:Label ID="lblImageInfo" runat="server" Font-Bold="False" Font-Size="Smaller"
                            ForeColor="Gray"></asp:Label>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 45%; height: 37px; text-align: right">
                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Resource, Admin_m_News_ShowOnMainPage %>"></asp:Label>&nbsp;
                    </td>
                    <td style="height: 37px">
                        <asp:CheckBox ID="chkOnMainPage" runat="server" Checked="True" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="2" cellspacing="0" width="100%" id="tableMailChimpNotActive"
                runat="server" visible="False">
                <tr>
                    <td style="width: 50%; height: 20px; text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_m_News_SendNews %>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 20px;">
                        <span style="color: red;">
                            <%= Resources.Resource.Admin_m_News_MailChimpOff %></span>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="2" cellspacing="0" width="100%" id="tableSendMailTypes"
                runat="server">
                <tr>
                    <td style="width: 45%; height: 20px; text-align: right">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_m_News_SendNews %>"></asp:Label>&nbsp;
                    </td>
                    <td style="vertical-align: middle; height: 20px;">
                        <asp:RadioButton ID="rbNo" runat="server" GroupName="Type" Text="<%$ Resources:Resource, Admin_m_News_NotSend %>"
                            Checked="True" />
                    </td>
                </tr>
                <tr style="background-color: #eff0f1">
                    <td style="height: 22px; text-align: right">
                        &nbsp;
                    </td>
                    <td style="vertical-align: top; height: 22px;">
                        <asp:RadioButton ID="rbToAll" runat="server" GroupName="Type" Text="<%$ Resources:Resource, Admin_m_News_ToAll %>" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 20px; text-align: right">
                    </td>
                    <td style="vertical-align: middle; height: 20px;">
                        <asp:RadioButton ID="rbToREG" runat="server" GroupName="Type" Text="<%$ Resources:Resource, Admin_m_News_SendRegUsers %>" />
                    </td>
                </tr>
                <tr style="background-color: #eff0f1">
                    <td style="height: 22px; text-align: right">
                    </td>
                    <td style="vertical-align: top; height: 22px;">
                        <asp:RadioButton ID="rbToUnReg" runat="server" GroupName="Type" Text="<%$ Resources:Resource, Admin_m_News_SendUnRegUsers %>" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                     <td style="height: 22px; text-align: right">
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr style="background-color: #eff0f1;">
                    <td style="width: 49%; height: 26px; text-align: right; vertical-align: middle;">
                        <asp:Localize ID="Localize_Admin_m_Product_HeadTitle" runat="server" Text="<%$ Resources: Resource, Admin_m_News_PageTitle %>"></asp:Localize>
                        <br />
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, Admin_m_News_UseGlobalVariables %>"></asp:Literal>
                    </td>
                    <td style="vertical-align: middle; height: 26px;">
                        <asp:TextBox ID="txtHeadTitle" runat="server" Width="354" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 49%; height: 29px; text-align: right">
                        <asp:Localize ID="Localize_Admin_m_Product_MetaKeywords" runat="server" Text="<%$ Resources: Resource, Admin_m_News_MetaKeywords %>"></asp:Localize>
                        <br />
                        <asp:Literal runat="server" Text="<%$ Resources: Resource, Admin_m_News_UseGlobalVariables %>"></asp:Literal>
                    </td>
                    <td style="vertical-align: middle; height: 29px;">
                        <asp:TextBox ID="txtMetaKeys" runat="server" TextMode="MultiLine" Width="354"></asp:TextBox>
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="width: 49%; height: 26px; text-align: right; vertical-align: middle;">
                        <asp:Localize ID="Localize_Admin_m_Product_MetaDescription" runat="server" Text="<%$ Resources: Resource, Admin_m_News_MetaDescription %>"></asp:Localize>
                        <br />
                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: Resource, Admin_m_News_UseGlobalVariables %>"></asp:Literal>
                    </td>
                    <td style="vertical-align: middle; height: 26px;">
                        <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" Width="354"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfMetaId" runat="server" />
            <!-- editor -->
            <tableid="table1" style="width: 100%; height: 206px; margin: 0px; padding: 0px; border:none;" >
                <tr>
                    <td align="center">
                        <br />
                        <asp:Panel ID="Panel4" runat="server" HorizontalAlign="Left" Width="700px" CssClass="all-mar">
                            <asp:Label ID="lblText" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Text %>"></asp:Label>
                            <asp:Label ID="lblRequiredText" runat="server" Font-Italic="True" ForeColor="DarkGray"
                                Text="<%$ Resources:Resource, Admin_m_News_Required %>"></asp:Label><br />
                            <CKEditor:CKEditorControl ID="FCKTextToPublication" BasePath="~/ckeditor/" runat="server" Height="500px" Width="700px" />
                        </asp:Panel>
                        &nbsp;&nbsp;
                        <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Left" Width="700px" CssClass="all-mar">
                            <asp:Label ID="lblAnnotation" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Annotation %>"></asp:Label>
                            <asp:Label ID="lblRequiredAnnotation" runat="server" Font-Italic="True" ForeColor="DarkGray"
                                Text="<%$ Resources:Resource, Admin_m_News_Required %>"></asp:Label><br />
                            <CKEditor:CKEditorControl ID="CKEditorControlAnnatation" BasePath="~/ckeditor/" SkinPath="skins/silver/"
                                ToolbarSet="Test" runat="server" Height="350px" Width="700px" />
                        </asp:Panel>
                        <br />
                      <%--  <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left" Width="700px" CssClass="all-mar">
                            <asp:Label ID="lblEmailText" runat="server" Text="<%$ Resources:Resource, Admin_m_News_EmailText %>"></asp:Label>
                            <asp:Label ID="lblOptional" runat="server" Font-Italic="True" ForeColor="DarkGray"
                                Text="<%$ Resources:Resource, Admin_m_News_Optional %>"></asp:Label><br />
                            <CKEditor:CKEditorControl ID="FCKTextToEmail" BasePath="~/ckeditor/" SkinPath="skins/silver/"
                                ToolbarSet="Test" runat="server" Height="350px" Width="700px" />
                        </asp:Panel>
                        &nbsp;--%>
                    </td>
                </tr>
            </tableid>
            <!-- editor -->
            <br />
            <div style="text-align: center;">
                <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:Resource, Admin_m_News_Ok %>"
                    Width="110px" OnClientClick="return Validate();" OnClick="btnOK_Click" />&nbsp;
            </div>
            <br />
        </asp:Panel>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT [Name], [NewsCategoryID] FROM [Settings].[NewsCategory] ORDER BY SortOrder"
            OnInit="SqlDataSource1_Init"></asp:SqlDataSource>
    </div>
</asp:Content>
<asp:Content ID="contentScript" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).click(function () {
            $find('calendar').hide();
        });

        function keyhook(ev) {
            ev = ev || window.event;
            var code = ev.keyCode;
            if (code == 27) {
                $find('calendar').hide();
            }
        }
        document.onkeydown = keyhook;

        function Validate() {
            var valid = true;
            var time = $("#<%= txtTime.ClientID %>").val().split(":");

            if (time[0] == "" || time[1] == "") {
                $("#validTime").show();
                valid = false;
                return valid;
            }
            else {
                $("#validTime").hide();
            }
            var hour = parseInt(time[0]);
            var minute = parseInt(time[1]);
            if (hour > 24 || hour < 0 || minute > 59 || minute < 0) {
                $("#validTime").show();
                valid = false;
            }
            else {
                $("#validTime").hide();
            }
            return valid;
        }

    </script>
</asp:Content>
