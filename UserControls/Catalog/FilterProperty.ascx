<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterProperty.ascx.cs"
    Inherits="UserControls_FilterProperty" %>
<!--noindex-->
<div id="property-filter">
    <asp:ListView runat="server" ID="lvProperties">
        <ItemTemplate>
          <article class="block-uc-inside" data-plugin="expander">
               <h4 class="title" data-expander-control="#filter-prop-<%#Eval("PropertyId") %>">
                   <%# Eval("PropertyName")%></h4>
                <div class="content ex-content" id="filter-prop-<%#Eval("PropertyId") %>" <%# Convert.ToBoolean(Eval("Expanded")) ? "Style=\"display:block;\"" :string.Empty %>  hidden="true">
                    <div class="chb-list propList">
                        <asp:ListView runat="server" DataSource='<%# Eval("ValuesList") %>'>
                            <ItemTemplate>
                                <div>
                                    <input name="prop_<%#Eval("PropertyId") %>" type="checkbox" id="<%#"prop_" + Eval("PropertyValueID") %>" <%#  AvaliblePropertyIDs != null && !AvaliblePropertyIDs.Contains(Convert.ToInt32(Eval("PropertyValueID"))) ? "disabled=\"disabled\"" : String.Empty  %>
                                        <%# SelectedPropertyIDs.Contains(Convert.ToInt32(Eval("PropertyValueID"))) ? "checked=\"checked\"" : string.Empty %>
                                        value="<%# Eval("PropertyValueID")%>" />
                                    <label for="<%#"prop_" + Eval("PropertyValueID") %>">
                                        <%# Eval("Value") %></label>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </article>
        </ItemTemplate>
    </asp:ListView>
</div>
<!--/noindex-->
