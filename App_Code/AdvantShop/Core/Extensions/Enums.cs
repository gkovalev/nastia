//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Resources;
using AdvantShop.Configuration;

namespace AdvantShop
{
    /// <summary>
    /// Summary description for Enums
    /// </summary>
    public static class Enums
    {
        public static string GetLocalizedName(this Enum val)
        {
            return
                new ResourceManager(typeof (Resource)).GetString(string.Format("Enums_{0}_{1}", val.GetType().Name, val))
                    .Default(val.ToString());
        }

        public static IEnumerable<string> GetLocalizedNames(this Enum val)
        {
            var resourceManager = new ResourceManager(typeof(Resource));
            var type = val.GetType();
            return from object value in Enum.GetValues(type) select resourceManager.GetString(string.Format("Enums_{0}_{1}", type.Name, value));
        }

        public static IEnumerable<string> GetValues(this Enum val)
        {
            var type = val.GetType();
            return from object value in Enum.GetValues(type) select value.ToString();
        }


        
        public static TResult ToEnum<TResult>(this Enum val)
        {
            var type = typeof (TResult);
            if (type.BaseType != typeof(Enum))
                throw new ArgumentException("TResult must be an enumeration.");
            var valName = val.ToString();
            return Enum.GetValues(type).Cast<TResult>().Where(value => value.ToString() == valName).FirstOrDefault();
        }
    }
}