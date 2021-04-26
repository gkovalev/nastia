<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductProperties.ascx.cs"
    Inherits="Admin_UserControls_ProductProperties" %>
<script type="text/javascript">
    function ClickCboPropertyName() {
        $("#<%= txtNewPropertyName.ClientID %>").attr("class", "disabled");
        $("#<%= cboPropertyName.ClientID %>").removeAttr("class");
        $("#<%= cboPropertyName.ClientID %> option").attr("class", ".option");
        //Enable checkbox
        $("#<%= RadioButtonExistPropertyValue.ClientID %>").removeAttr("disabled");
        //Enable dropdown
        $("#<%= cboPropertyValue.ClientID %>").removeAttr("disabled");
    }

    function ClickTdExistPropertyName() {
        document.getElementById('<%=RadioButtonExistProperty.ClientID%>').click();
    }

    function ClickCboPropertyValue() {
        $("#<%= cboPropertyValue.ClientID %>").removeAttr("class");
        $("#<%= cboPropertyValue.ClientID %> option").attr("class", ".option");
        $("#<%= txtNewPropertyValue.ClientID %>").attr("class", "disabled");
    }

    function ClickTdExistPropertyValue() {
        document.getElementById('<%=RadioButtonExistPropertyValue.ClientID%>').click();
    }

    function ClickTxtNewPropertyName() {
        $("#<%= txtNewPropertyName.ClientID %>").removeAttr("class");
        $("#<%= cboPropertyName.ClientID %>").attr("class", "disabled");
        $("#<%= cboPropertyName.ClientID %> option").removeAttr("class");
        $("#<%= RadioButtonNewPropertyValue.ClientID %>").click();
        //Disable checkbox
        $("#<%= RadioButtonExistPropertyValue.ClientID %>").attr("disabled", "disabled");
        //Disable dropdown
        $("#<%= cboPropertyValue.ClientID %>").attr("disabled", "disabled");
    }

    function ClickTdNewPropertyName() {
        document.getElementById('<%=RadioButtonAddNewProperty.ClientID%>').click();
    }

    function ClickTxtNewPropertyValue() {
        $("#<%= txtNewPropertyValue.ClientID %>").removeAttr("class");
        $("#<%= cboPropertyValue.ClientID %>").attr("class", "disabled");
        $("#<%= cboPropertyValue.ClientID %> option").removeAttr("class");
    }

    function ClickTdNewPropertyValue() {
        document.getElementById('<%=RadioButtonNewPropertyValue.ClientID%>').click();
    }
    function SwitchDvValue(sender) {
        $(sender).closest("table").find("input[type=text]").attr("class", "disabled");
        $(sender).closest("table").find("select").attr("class", "disabled");
        $(sender).closest("table").find("select option").attr("class", ".option");
        $(sender).closest("tr").find("select").removeAttr("class");
        $(sender).closest("tr").find("select option").removeAttr("class");
        $(sender).closest("tr").find("input[type=text]").removeAttr("class");
    }
</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblErrorAddProp" runat="server" CssClass="mProductLabelInfo" EnableViewState="False"
                Font-Names="Verdana" Font-Size="14px" ForeColor="Red" Visible="False" />
            <table class="table-p">
                <tr>
                    <td class="formheader" colspan="4">
                        <h2>
                            <%=Resources.Resource.Admin_Product_ProductProperties%></h2>
                    </td>
                </tr>
                <tr class="formheaderfooter">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%;">
                        <asp:RadioButton ID="RadioButtonExistProperty" runat="server" GroupName="PropName"
                            Checked="True" OnClick="ClickCboPropertyName()" />
                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_ExistProperties %>"
                            Font-Bold="False" AssociatedControlID="RadioButtonExistProperty" Style="margin-left: -3px;" EnableViewState="false"></asp:Label>
                        <span style="color: red;">&nbsp;*</span>
                    </td>
                    <td style="width: 40%;">
                        <asp:RadioButton ID="RadioButtonExistPropertyValue" runat="server" GroupName="PropValue"
                            Checked="True" OnClick="ClickCboPropertyValue()" />
                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Value %>"
                            AssociatedControlID="RadioButtonExistPropertyValue" Style="margin-left: -3px;" EnableViewState="false"></asp:Label>
                        <span style="color: red;">&nbsp;*</span>
                    </td>
                    <td style="width: 50%;">
                        <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_SortIndex %>"></asp:Label>
                        <span style="color: red;">&nbsp;*</span>
                    </td>
                    <td style="width: 45%;">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%; height: 19px;">
                        <asp:DropDownList ID="cboPropertyName" runat="server" Width="90%" AutoPostBack="True"
                            onfocus="ClickTdExistPropertyName();return false;" OnSelectedIndexChanged="cboPropertyName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 40%; height: 19px;">
                        <%--<asp:TextBox ID="txtPropertyValue" runat="server" Width="90%" />--%>
                        <asp:DropDownList ID="cboPropertyValue" runat="server" Width="90%" onfocus="ClickTdExistPropertyValue(); return false;">
                        </asp:DropDownList>
                        &nbsp;
                    </td>
                    <td style="width: 25%; height: 19px;">
                        <asp:TextBox ID="txtSortIndex0" runat="server" Width="90%"></asp:TextBox>
                    </td>
                    <td style="width: 50%; height: 19px;">
                        <asp:Button ID="btnAddNewExistProperty" runat="server" Text="<%$ Resources:Resource, Admin_Product_Add %>"
                            OnClick="btnAddNewExistProperty_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 28px; width: 40%; vertical-align: bottom;">
                        <asp:RadioButton ID="RadioButtonAddNewProperty" runat="server" GroupName="PropName"
                            OnClick="ClickTxtNewPropertyName()" />
                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_AddNewProperty %>"
                            Font-Bold="False" AssociatedControlID="RadioButtonAddNewProperty" Style="margin-left: -3px;" EnableViewState="false"></asp:Label>
                        <span style="color: red;">&nbsp;*</span>
                    </td>
                    <td style="height: 28px; vertical-align: bottom; width: 40%;">
                        <asp:RadioButton ID="RadioButtonNewPropertyValue" runat="server" GroupName="PropValue"
                            OnClick="ClickTxtNewPropertyValue()" />
                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_NewValue %>"
                            AssociatedControlID="RadioButtonNewPropertyValue" Style="margin-left: -3px;" EnableViewState="false"></asp:Label>
                        <span style="color: red;">&nbsp;*</span>
                    </td>
                    <td style="width: 25%; height: 28px; vertical-align: bottom;">
                        <%--<asp:Label ID="Label42" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_SortIndex %>"
                                                                        Visible="True"></asp:Label>--%>
                    </td>
                    <td style="height: 28px; width: 50%;">
                        &nbsp;
                    </td>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="height: 19px; width: 40%;" onclick="ClickTdNewPropertyName()">
                        <asp:TextBox ID="txtNewPropertyName" runat="server" Width="90%" Style="border-width: 1px;"></asp:TextBox>
                    </td>
                    <td style="height: 19px; width: 40%;" onclick="ClickTdNewPropertyValue()">
                        <asp:TextBox ID="txtNewPropertyValue" runat="server" Width="90%" Style="border-width: 1px;"></asp:TextBox>
                    </td>
                    <td style="width: 35%; height: 19px;">
                        <%-- <asp:TextBox ID="txtSortIndexAdd" runat="server" Width="90%"></asp:TextBox>--%>
                    </td>
                    <%-- <td style="height: 19px; width: 50%;">
                                                        <asp:Button ID="btnAddNewProductPropertyWithValue" runat="server" Text="<%$ Resources:Resource, Admin_Product_Add %>" />&nbsp;
                                                    </td>--%>
                </tr>
                <tr style="background-color: #eff0f1;">
                    <td style="height: 13px; width: 40%;">
                        <asp:Label ID="lblValueRequired" runat="server" Visible="false" EnableViewState="false"
                            ForeColor="Red" Text="<%$Resources:Resource, Admin_m_Product_RequiredField %>" />
                    </td>
                    <td style="height: 13px; width: 30%;">
                        <%--   <asp:Label ID="lPropValueRequired1" runat="server" Visible="false" EnableViewState="false" 
                                                             ForeColor="Red" Text="<%$ Resources:Resource,Admin_m_Product_RequiredField %>" />--%>
                    </td>
                    <td style="width: 40%; height: 19px">
                        <asp:Label ID="lInvalidSortOrder1" runat="server" EnableViewState="false" ForeColor="Red" />
                    </td>
                    <td style="height: 13px; width: 50%;">
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_CurrentProperties %>" EnableViewState="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvProperties" runat="server" CellPadding="4" AutoGenerateColumns="False"
                            AllowSorting="false" CssClass="GridViewStyle" BorderWidth="1px" OnRowDataBound="gvProperties_DataBound"
                            OnRowEditing="gvProperties_Edit" Width="100%" OnRowDeleting="gvProperties_RowDeleting"
                            OnRowUpdating="gvProperties_Update" OnRowCancelingEdit="gvProperties_Cancel"
                            OnSelectedIndexChanged="gvProperties_Edit" BorderStyle="Solid">
                            <Columns>
        <%--                        <asp:BoundField DataField="Property.Name" ReadOnly="true" HeaderText="<%$ Resources:Resource, Admin_Product_Name %>"
                                    SortExpression="Property.Name">
                                    <ItemStyle Width="50%" />
                                    <HeaderStyle ForeColor="Black" />
                                </asp:BoundField>--%>
                                   <asp:TemplateField HeaderText="<%$ Resources:Resource, Admin_Product_Name %>"
                                    SortExpression="Property.Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Property.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                       <asp:Label ID="lblName" runat="server" Text='<%# Eval("Property.Name") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemStyle Width="50%" />
                                    <HeaderStyle ForeColor="Black" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, Admin_m_Product_Value %>"
                                    SortExpression="Value">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                        <asp:HiddenField ID="hfPropValId" runat="server" Value='<%# Eval("PropertyValueId") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfPropValId" runat="server" Value='<%# Eval("PropertyValueId") %>' />
                                        <!-- Вёрстку не трогать!!!! -->
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbExistValue" runat="server" Checked="true" GroupName="ValueType"
                                                        OnClick="SwitchDvValue(this)" />
                                                </td>
                                                <td onclick="if (!$(this).closest('tr').find('input[type=radio]').is(':checked')) $(this).closest('tr').find('input[type=radio]').click();">
                                                    <asp:DropDownList ID="ddlValue" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbNewValue" runat="server" GroupName="ValueType" OnClick="SwitchDvValue(this)" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="disabled" Text='<%# Eval("Value") %>'
                                                        OnClick="$(this).closest('tr').find('input[type=radio]').click()"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <ItemStyle Width="50%" />
                                    <HeaderStyle ForeColor="Black" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Resource, Admin_m_Product_SortIndex %>"
                                    SortExpression="SortOrder">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'></asp:TextBox>
                                        <%--<asp:HiddenField ID="hfPropValId" runat="server" Value='<%# Eval("SortOrder") %>' />--%>
                                        <!-- Вёрстку не трогать!!!! -->
                                    </EditItemTemplate>
                                    <ItemStyle Width="50%" />
                                    <HeaderStyle ForeColor="Black" />
                                </asp:TemplateField>
                                <asp:CommandField ShowDeleteButton="true" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="Link"
                                    DeleteText="<%$ Resources:Resource, Admin_Delete %>" ShowSelectButton="true"
                                    SelectText="<%$  Resources:Resource, Admin_Edit %>" ShowCancelButton="true" EditText=""
                                    CancelText="<%$  Resources:Resource, Admin_Cancel %>" ShowEditButton="true" UpdateText="<%$  Resources:Resource, Admin_Update %>"
                                    ShowHeader="false" ItemStyle-ForeColor="Black" />
                            </Columns>
                            <RowStyle CssClass="GridView_RowStyle" />
                            <FooterStyle CssClass="GridView_FooterStyle" Font-Bold="True" />
                            <EditRowStyle CssClass="GridView_EditRowStyle" />
                            <SelectedRowStyle CssClass="GridView_EditRowStyle" />
                            <HeaderStyle CssClass="GridView_HeaderStyle" />
                            <AlternatingRowStyle CssClass="GridView_AlternatingRowStyle" />
                            <PagerStyle CssClass="GridView_PagerStyle" ForeColor="Black" />
                        </asp:GridView>
                        <br />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
