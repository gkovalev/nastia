<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductPropertiesSetView.ascx.cs"
    Inherits="UserControls_ProductPropertiesSetView" %>
    <%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
        <div style="display: none">
            <input id="hiddenProductID" runat="server" value="" type="hidden" />
          
        </div>   
        <asp:Label ID="lblHeader" runat="server" Font-Bold="True" 
    Font-Italic="True" Font-Size="Medium"></asp:Label>
        <hr />
 
         <table class="pv-table" border="1">    <tr class="head">
                            <th class="icon">
                            </th>                                                                  
                                  <th >
                                   <%=Resources.Resource.ProductFields_Sku%> 
                            </th>
 
                <%foreach (var item in ProductItems)
                  {%>
                <th >
                    <%=(item.ArtNo) %>
                   
                </th>
                <%}%>
               
     </tr>

            <%foreach (var propertyName in SetPropertyNames.Distinct())
              {%>
           <tr class="pv-item">                         <td class="icon" abbr="<%=propertyName.PhotoLink%> ">
  <%=propertyName.PhotoLink != null ? " <div class=photo></div>" : "" %>
                          <!--  <div class="photo"></div>-->
                     </td>      
        
                <td  >
               <span class="param-name">      <%=propertyName.PropertyLink%> 
                           </span>
               
                               
               <br/><br/>

                </td>
                <%foreach (var item in ProductItems)
                  {%>
                <td >
                   <span class="param-name">  
                    <%=item.Properties.Where(t => t.Value == propertyName.PropertyName).Count() > 0 ? "<img src=" + @"images/icons/ico_yes.gif" + " />" : "<img src=" + @"images/icons/ico_no.gif" + " />"%>
                    </span><br/><br/>
                </td>
                <%}%>
            </tr>
            <%}%>

               <tr >
                <th >
                    
                </th>
               
                      <th >  <%=Resources.Resource.ProductFields_Price%> 
                            </th>
                 
                <%foreach (var item in ProductItems)
                  {%>
                <th>
                    <%=AdvantShop.Catalog.CatalogService.GetStringPrice(item.Price,"-") %>
                </th>
                <%}%>
            </tr>
        </table>

