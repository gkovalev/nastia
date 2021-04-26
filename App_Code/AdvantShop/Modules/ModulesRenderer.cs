//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text;
using AdvantShop.Orders;
using System.Drawing;

namespace AdvantShop.Modules
{
    public class ModulesRenderer
    {
        #region PictureModules
        public static void ProcessPhoto(Image image)
        {
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.ProcessPhoto))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("DoProcessPhoto");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    method.Invoke(classInstance, new object[] { image });
                }
            }
        }
        #endregion

        #region OrderModules
        public static void OrderAdded(int orderId)
        {
            IOrder order = null;

            var modules = AttachedModules.GetModules(AttachedModules.EModuleType.OrderChanged);

            if (modules.Count > 0)
                order = OrderService.GetOrder(orderId);

            foreach (var cls in modules)
            {
                System.Reflection.MethodInfo method = cls.GetMethod("DoOrderAdded");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    method.Invoke(classInstance, new object[] { order });
                }
            }
        }

        public static void OrderChangeStatus(int orderId)
        {
            IOrder order = null;

            var modules = AttachedModules.GetModules(AttachedModules.EModuleType.OrderChanged);

            if (modules.Count > 0)
                order = OrderService.GetOrder(orderId);

            foreach (var cls in modules)
            {
                System.Reflection.MethodInfo method = cls.GetMethod("DoOrderChangeStatus");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    method.Invoke(classInstance, new object[] { order });
                }
            }
        }

        public static void OrderUpdated(int orderId)
        {
            IOrder order = null;

            var modules = AttachedModules.GetModules(AttachedModules.EModuleType.OrderChanged);

            if (modules.Count > 0)
                order = OrderService.GetOrder(orderId);

            foreach (var cls in modules)
            {
                System.Reflection.MethodInfo method = cls.GetMethod("DoOrderUpdated");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    method.Invoke(classInstance, new object[] { order });
                }
            }
        }

        public static void OrderDeleted(int orderId)
        {
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.OrderChanged))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("DoOrderDeleted");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    method.Invoke(classInstance, new object[] { orderId });
                }
            }
        }
        #endregion

        #region DetailsModules
        public static string RenderDetailsModulesToRightColumn()
        {
            var result = string.Empty;
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.DetailsModule))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("RenderToRightColumn");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    result += method.Invoke(classInstance, null);
                }
            }
            return result;
        }

        public static void RenderDetailsModulesToProductInformation()
        {
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.DetailsModule))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("RenderToProductInformation");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    method.Invoke(classInstance, null);
                }
            }
        }

        public static void RenderDetailsModulesToBottom()
        {
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.DetailsModule))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("RenderToBottom");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    method.Invoke(classInstance, null);
                }
            }
        }

        #endregion

        #region NotificationModules
        public static void SendSms(string phoneNumber, string text)
        {
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.SMS))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("SendSms");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    method.Invoke(classInstance, new object[] { phoneNumber, text });
                }
            }
        }
        #endregion

        #region HtmlModules
        public static string RenderIntoHead()
        {
            var builder = new StringBuilder();
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.RenderIntoHtml))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("DoRenderIntoHead");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    builder.Append(method.Invoke(classInstance, null));
                }
            }
            return builder.ToString();
        }

        public static string RenderAfterBodyStart()
        {
            var builder = new StringBuilder();
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.RenderIntoHtml))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("DoRenderAfterBodyStart");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    builder.Append(method.Invoke(classInstance, null));
                }
            }
            return builder.ToString();
        }

        public static string RenderBeforeBodyEnd()
        {
            var builder = new StringBuilder();
            foreach (var cls in AttachedModules.GetModules(AttachedModules.EModuleType.RenderIntoHtml))
            {
                System.Reflection.MethodInfo method = cls.GetMethod("DoRenderBeforeBodyEnd");
                if (method != null)
                {
                    object classInstance = Activator.CreateInstance(cls, null);
                    builder.Append(method.Invoke(classInstance, null));
                }
            }
            return builder.ToString();
        }
        #endregion
    }
}