<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PopupTreeView.ascx.cs"
    Inherits="Admin_UserControls_PopupTreeView" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="tree" />
    </Triggers>
    <ContentTemplate>
        <ajaxToolkit:ModalPopupExtender ID="mpeTree" runat="server" PopupControlID="pTree"
            TargetControlID="hhl" BackgroundCssClass="blackopacitybackground" BehaviorID="ModalBehaviourTree">
        </ajaxToolkit:ModalPopupExtender>
        <asp:HyperLink ID="hhl" runat="server" Style="display: none;" />
        <asp:Panel runat="server" ID="pTree" CssClass="modal-admin">
            <table>
                <tbody>
                    <tr>
                        <td>
                            <div class="title">
                                <asp:Localize ID="lcl_Header" runat="server" Text="<%# HeaderText %>"></asp:Localize></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="height: 360px; width: 450px; overflow: scroll; background-color: White;
                                text-align: left">
                                <asp:TreeView ID="tree" ForeColor="Black" SelectedNodeStyle-BackColor="Blue" PopulateNodesFromClient="true"
                                    runat="server" ShowLines="True" ExpandImageUrl="images/loading.gif" BackColor="White"
                                    OnTreeNodePopulate="PopulateNode" OnTreeNodeCheckChanged="tree_TreeNodeCheckChanged"
                                    AutoPostBack="true">
                                    <SelectedNodeStyle BackColor="Yellow" />
                                </asp:TreeView>
                                ArtNo:<br />
                                <asp:TextBox ID="txtArtNo" runat="server" Height="80px" TextMode="MultiLine" 
                                    Width="145px"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50px; vertical-align: bottom; text-align: right">
                            <asp:Button runat="server" ID="btnOk" Text="<%$ Resources: Resource, Admin_Product_OK %>"
                                OnClick="tree_NodeSelected" CausesValidation="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources: Resource, Admin_Cancel %>"
                                OnClick="btnCancel_Click" Width="67" CausesValidation="false" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<% if (CheckChildrenNodes)
   { %>
<script type="text/javascript">
    function TreeViewCheckBoxClicked(Check_Event) {
        var objElement;
        try {
            // Get the element which fired the event.
            objElement = window.event.srcElement;
        }
        catch (Error) {
            //srcElement is failing, objElement is null.
        }
        if (objElement != null) {
            // If the element is a checkbox do postback.
            if (objElement.tagName == "INPUT" && objElement.type == "checkbox") {
                __doPostBack("", "");
            }
        }
        else {
            //    If the srcElement is failing due to browser incompatibility determine
            // whether the element is and HTML input element and do postback.
            if (Check_Event != null) {
                if (Check_Event.target.toString() == "[object HTMLInputElement]") {
                    __doPostBack("", "");
                }
            }
        }
    }
</script>
<% } %>

<script type="text/javascript">
    function ATreeView_Select(sender, arg) {
        $("a.selectedtreenode").removeClass("selectedtreenode");
        $(sender).addClass("selectedtreenode");
        $("#TreeView_SelectedValue").val(arg);
        $("#TreeView_SelectedNodeText").val(sender.innerHTML);
        return false;
    }
</script>
<input type="hidden" id="TreeView_SelectedValue" name="TreeView_SelectedValue" />
<input type="hidden" id="TreeView_SelectedNodeText" name="TreeView_SelectedNodeText" />