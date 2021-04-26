//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Statistic
{
    public class ClientInfoService
    {
        public static string GetOSName(string userAgent)
        {

            if (userAgent == null)
                return string.Empty;

            string osName = string.Empty;
            var listOs = new Dictionary<string, string>
                                                    {
                                                        {"Windows 3.11", "Win16"},
                                                        {"Windows 95", "Windows 95|Win95|Windows_95"},
                                                        {"Windows 98", "Windows 98|Win98"},
                                                        {"Windows 2000", "Windows NT 5.0|Windows 2000"},
                                                        {"Windows XP", "Windows NT 5.1|Windows XP"},
                                                        {"Windows Server 2003", "Windows NT 5.2"},
                                                        {"Windows Vista", "Windows NT 6.0"},
                                                        {"Windows 7", "Windows NT 6.1"},
                                                        {"Windows ME", "Windows ME"},
                                                        {"OpenBSD", "OpenBSD"},
                                                        {"SunOS", "SunOS"},
                                                        {"Linux", "Linux|(X11"},
                                                        {"MacOS", "Mac_PowerPC|Macintosh"},
                                                        {"QNX", "QNX"},
                                                        {"BeOS", "BeOS"},
                                                        {"OS/2", "OS/2"}
                                                    };

            foreach (KeyValuePair<string, string> searchstr in listOs)
            {
                string[] strs = searchstr.Value.Split('|');
                foreach (string t in strs)
                {
                    if (userAgent.Contains(t))
                    {
                        osName = searchstr.Key;
                    }
                }
            }
            if (string.IsNullOrEmpty(osName))
            {
                osName = "Unknown";
            }
            return osName;
        }

        public static string GetBrowser(string userAgent)
        {

            if (userAgent == null)
                return string.Empty;

            string browserName = string.Empty;
            userAgent = userAgent.Replace(" ", "");
            string[] userAgents = userAgent.Split('/');

            for (int i = 0; i < userAgents.Length - 1; i++)
            {

                // Chrome
                if (userAgents[i].Contains("Chrome"))
                {
                    browserName = "Chrome" + @" " + userAgents[i + 1].Replace("Safari", "");
                    break;
                }

                // Safari
                if (userAgents[i].Contains("Safari"))
                {
                    for (int j = 0; j < userAgents.Length - 1; j++)
                    {
                        if (userAgents[j].Contains("Version"))
                        {
                            browserName = "Safari" + @" " + userAgents[j + 1].Replace("Safari", "");
                            break;
                        }
                    }
                    break;
                }

                // Firefox
                if (userAgents[i].Contains("Firefox"))
                {
                    browserName = "Firefox" + @" " + userAgents[i + 1];
                    break;
                }

                // Opera
                if (userAgents[i].Contains("Opera"))
                {
                    browserName = "Opera" + @" " + userAgents[i + 1].Substring(0, userAgents[i + 1].IndexOf("("));
                    break;
                }

                // MS IE
                if (userAgents[i].Contains("MSIE"))
                {
                    browserName = "Internet Explorer" + @" " + userAgents[i].Substring(userAgents[i].IndexOf("MSIE") + 4, 3);
                    break;
                }
            }
            if (string.IsNullOrEmpty(browserName))
            {
                browserName = "Unknown";
            }
            return browserName;
        }

        public static void CreateClient(ClientOnlineInfo cInf)
        {
            SQLDataAccess.ExecuteNonQuery(
                IsExist(cInf.Address)
                    ? "update [Customers].[ClientInfo] Set [sessionID]=@sessionID, [UserAgentBrowser]=@UserAgentBrowser, [UserAgentOS]=@UserAgentOS, [CountryByGeoIp]=@CountryByGeoIp, [LastAccessedPath]=@LastAccessedPath, [Started]=@Started, [Ended]=@Ended Where Address=@Address"
                    : "INSERT INTO [Customers].[ClientInfo] ([sessionID], [Address], [UserAgentBrowser], [UserAgentOS], [CountryByGeoIp], [LastAccessedPath] ,[Started], [Ended]) VALUES (@sessionID, @Address, @UserAgentBrowser, @UserAgentOS, @CountryByGeoIp, @LastAccessedPath, @Started, @Ended)",
                CommandType.Text,
                new SqlParameter("@sessionID", cInf.SessionId),
                new SqlParameter("@Address", cInf.Address),
                new SqlParameter("@UserAgentBrowser", cInf.UserAgentBrowser),
                new SqlParameter("@UserAgentOS", cInf.UserAgentOS),
                new SqlParameter("@CountryByGeoIp", cInf.CountryByGeoIp),
                new SqlParameter("@LastAccessedPath", cInf.LastAccessedPath),
                new SqlParameter("@Started", cInf.Started),
                new SqlParameter("@Ended", cInf.Ended)
                );
        }

        private static bool IsExist(string address)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(Address) from  [Customers].[ClientInfo]  where Address=@address", CommandType.Text, new SqlParameter("@address", address)) > 0;
        }

        public static IList<ClientOnlineInfo> GetAllInfo()
        {
            List<ClientOnlineInfo> lInfo = SQLDataAccess.ExecuteReadList<ClientOnlineInfo>("SELECT * FROM [Customers].[ClientInfo]", CommandType.Text,
                                                                         reader => new ClientOnlineInfo
                                                                                       {
                                                                                           SessionId = SQLDataHelper.GetString(reader, "sessionID"),
                                                                                           Address = SQLDataHelper.GetString(reader, "Address"),
                                                                                           UserAgentBrowser = SQLDataHelper.GetString(reader, "UserAgentBrowser"),
                                                                                           UserAgentOS = SQLDataHelper.GetString(reader, "UserAgentOS"),
                                                                                           CountryByGeoIp = SQLDataHelper.GetString(reader, "CountryByGeoIp"),
                                                                                           LastAccessedPath = SQLDataHelper.GetString(reader, "LastAccessedPath"),
                                                                                           Started = SQLDataHelper.GetDateTime(reader, "Started"),
                                                                                           Ended = SQLDataHelper.GetDateTime(reader, "Ended")
                                                                                       });
            return lInfo;
        }

        public static int GetCountInfo()
        {
            var count = SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(*) FROM [Customers].[ClientInfo]", CommandType.Text);
            return count;
        }

        public static void UpdateClientInfo(string sessionId, string lastAccessedPath)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Customers].[ClientInfo] SET [LastAccessedPath] =@LastAccessedPath, [Ended] = @Ended where sessionID=@sessionID",
                                            CommandType.Text,
                                            new SqlParameter("@LastAccessedPath", lastAccessedPath),
                                            new SqlParameter("@Ended", DateTime.Now.AddMinutes(SessionServices.GetTimeoutSession())),
                                            new SqlParameter("@sessionID", sessionId)
                                            );
        }

        public static void Clear(string sessionId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Customers].[ClientInfo] WHERE [sessionID]=@sessionID", CommandType.Text, new SqlParameter("@sessionID", sessionId));
        }

        public static void ClearIfNotInDbSession()
        {
            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_DeleteOldSessions]", CommandType.StoredProcedure, new SqlParameter("@CurrentTime", DateTime.Now));
        }
    }
}