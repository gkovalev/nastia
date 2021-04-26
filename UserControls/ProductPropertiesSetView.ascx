<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductPropertiesSetView.ascx.cs"
    Inherits="UserControls_ProductPropertiesSetView" %>
        <div style="display: none">
            <input id="hiddenProductID" runat="server" value="" type="hidden" />
          
        </div>   
        <asp:Label ID="lblHeader" runat="server" Font-Bold="True" 
    Font-Italic="True" Font-Size="Medium"></asp:Label>
        <hr />
 
        <table >
     
            <tr>
               <th>
                  <%=Resources.Resource.Client_Details_SKU%> 
                    <br /><br />
                </th>
                <%foreach (var item in ProductItems)
                  {%>
                <th >
                    <%=(item.ArtNo) %>
                    <br /><br />
                </th>
                <%}%>
               
            </tr>

            <%foreach (var propertyName in SetPropertyNames.Distinct())
              {%>
            <tr >
                <td >
               <span class="param-name">      <%=propertyName.Value%> 
                           </span><br/><br/>

                </td>
                <%foreach (var item in ProductItems)
                  {%>
                <td >
                   <span class="param-name">  
                    <%=item.Properties.Where(t => t.Value == propertyName.Key).Count() > 0 ? "<img src=" + @"images/icons/ico_yes.gif" + " />" : "<img src=" + @"images/icons/ico_no.gif" + " />"%>
                    </span><br/><br/>
                </td>
                <%}%>
            </tr>
            <%}%>

               <tr >
                <th >
                    <%=Resources.Resource.Client_CompareProducts_Price %>
                </th>
                <%foreach (var item in ProductItems)
                  {%>
                <th>
                    <%=AdvantShop.Catalog.CatalogService.GetStringPrice(item.Price) %>
                </th>
                <%}%>
            </tr>
        </table>

