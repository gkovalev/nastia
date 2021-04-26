//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Modules;
using AdvantShop.Modules.Interfaces;
using AdvantShop.Security;

namespace AdvantShop.Controls
{
    public class AdvMenuAdmin : Menu
    {
        #region  Properties

        public Customer CurrentCustomer { get; set; }

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            var modules = Core.AdvantshopConfigService.GetActivityModules();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "myslidemenu");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "jqueryslidemenu");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            int i = 1;

            foreach (MenuItem parent in this.Items)
            {
                try //Added by Evgeni to catch module exception
                {
                    if (parent.Value == @"modules")
                    {
                        var addingModules = new MenuItem { Text = Resources.Resource.Admin_ModuleManager_Header, NavigateUrl = "modulesmanager.aspx" };
                        parent.ChildItems.Add(addingModules);
                        var mdls = (from m in AttachedModules.GetModules(AttachedModules.EModuleType.All)
                                    orderby ((IModule)Activator.CreateInstance(m, null)).ModuleName
                                    select m);
                        foreach (var type in mdls)
                        {
                            var addingModule = ((IModule)Activator.CreateInstance(type, null));
                            if (addingModule.CheckAlive())
                            {
                                addingModules.ChildItems.Add(new MenuItem
                                    {
                                        Text = addingModule.ModuleName,
                                        NavigateUrl = "module.aspx?module=" + addingModule.ModuleStringId
                                    });
                            }
                        }
                    }
                }
                catch { }
                bool visible = RoleAccess.Check(CurrentCustomer, parent.NavigateUrl.ToLower()) && (!modules.ContainsKey(parent.Value) || modules[parent.Value]);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "m-main-item MenuHorizontalItem" + i + (!visible ? " m-hide" : "m-item"));
                //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "location='" + RouteService.GetAdminAbsoluteLink(parent.NavigateUrl) + "'");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);


                writer.AddAttribute(HtmlTextWriterAttribute.Href, UrlService.GetAdminAbsoluteLink(parent.NavigateUrl));
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "LinkMNImg");
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "MenuHorizontalItem" + i + "Img");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, parent.ImageUrl);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);

                writer.RenderEndTag(); // a - parent menuitem 
                writer.RenderEndTag(); // a

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "LinkMenuHorizontal LinkMH" + i);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, (visible ? UrlService.GetAdminAbsoluteLink(parent.NavigateUrl) : "#"));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.WriteLine(parent.Text);
                writer.RenderEndTag(); // a

                if (parent.ChildItems.Count > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "SubMenu" + i);
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                    foreach (MenuItem item in parent.ChildItems)
                    {
                        visible = RoleAccess.Check(CurrentCustomer, item.NavigateUrl.ToLower()) && (!modules.ContainsKey(item.Value) || modules[item.Value]);

                        writer.AddAttribute(HtmlTextWriterAttribute.Class, (!visible ? "m-hide" : "m-item"));
                        writer.RenderBeginTag(HtmlTextWriterTag.Li);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, (visible ? UrlService.GetAdminAbsoluteLink(item.NavigateUrl) : "#"));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.WriteLine(item.Text);
                        writer.RenderEndTag(); //a

                        if (item.ChildItems.Count > 0)
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                            foreach (MenuItem subItem in item.ChildItems)
                            {

                                visible = RoleAccess.Check(CurrentCustomer, subItem.NavigateUrl.ToLower()) && (!modules.ContainsKey(subItem.Value) || modules[subItem.Value]);

                                writer.AddAttribute(HtmlTextWriterAttribute.Class, (!visible ? "m-hide" : "m-item"));
                                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                                writer.AddAttribute(HtmlTextWriterAttribute.Href, (visible ? UrlService.GetAdminAbsoluteLink(subItem.NavigateUrl) : "#"));
                                writer.RenderBeginTag(HtmlTextWriterTag.A);
                                writer.WriteLine(subItem.Text);
                                writer.RenderEndTag(); //a

                                writer.RenderEndTag(); //li
                            }

                            writer.RenderEndTag(); //ul
                        }
                        writer.RenderEndTag(); //li
                    }
                    writer.RenderEndTag(); //ul
                }

                writer.RenderEndTag(); // li
                i++;
            }

            writer.RenderEndTag(); //ul

            writer.RenderEndTag(); //div

        }
    }
}