//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdvantShop.Helpers
{
    public static class QueryHelper
    {

        public static string RemoveQueryParam(string query, string name)
        {
            query = query.Trim('?');

            if (query.IsNullOrEmpty())
                return string.Empty;

            var list = new List<string>();
            list.AddRange(query.Substring(0, query.Length - 1).Split('&'));

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].ToLower().Contains(name.ToLower() + "="))
                {
                    list.RemoveAt(i);
                }
            }
            return list.Any() ? "?" + string.Join("&", list.ToArray()) : string.Empty;
        }

        public static string ChangeQueryParam(string query, string name, string value)
        {

            //todo Vladimir сделать как RemoveQueryParam, на списке
            string[] @params;
            if (!string.IsNullOrEmpty(query))
            {
                @params = query.Substring(1, query.Length - 1).Split('&');
            }
            else
            {
                return string.IsNullOrEmpty(value) ? "" : "?" + name + "=" + value;
            }

            bool isExistPageParam = false;

            for (var i = 0; i <= @params.Length - 1; i++)
            {
                if (string.Compare(@params[i], 0, name, 0, name.Length, true) != 0) continue;
                if (string.IsNullOrEmpty(value))
                {
                    @params[i] = string.Empty;
                }
                else
                {
                    @params[i] = name + "=" + value;
                    isExistPageParam = true;
                }
                break;
            }


            if (!isExistPageParam)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return "?" + string.Join("&", @params);
                }
                return "?" + string.Join("&", @params) + "&" + name + "=" + value;
            }
            return "?" + string.Join("&", @params);
        }

        public static string CreateQueryString(this IEnumerable<KeyValuePair<string, string>> pars)
        {
            return pars.Aggregate(new StringBuilder(), (sb, par) => sb.AppendFormat("{0}={1}&", par.Key, par.Value),
                                  sb => sb.ToString().TrimEnd('&'));
        }
    }
}