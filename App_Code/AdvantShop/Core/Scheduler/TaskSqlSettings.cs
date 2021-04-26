//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Configuration;
using Newtonsoft.Json;

namespace AdvantShop.Core.Scheduler
{
    public enum TimeIntervalType
    {
        None,
        Days,
        Hours,
        Minutes
    }

    public class TaskSqlSettings : List<TaskSqlSetting>
    {
        private static TaskSqlSettings GetDefault()
        {
            return new TaskSqlSettings
                               {
                                   new TaskSqlSetting
                                       {
                                           Enabled = false,
                                           JobType = typeof (GenerateHtmlMapJob).ToString(),
                                           TimeInterval = 0,
                                           TimeHours =0,
                                           TimeMinutes =0,
                                           TimeType = TimeIntervalType.None
                                       },
                                   new TaskSqlSetting
                                       {
                                           Enabled = false,
                                           JobType = typeof (GenerateXmlMapJob).ToString(),
                                           TimeInterval = 0,
                                           TimeHours =0,
                                           TimeMinutes =0,
                                           TimeType = TimeIntervalType.None
                                       },
                                   new TaskSqlSetting
                                       {
                                           Enabled = false,
                                           JobType = typeof (GenerateYandexMarketJob).ToString(),
                                           TimeInterval = 0,
                                           TimeHours =0,
                                           TimeMinutes =0,
                                           TimeType = TimeIntervalType.None
                                       },
                                       //Added by Evgeni to insert update job
                                    new TaskSqlSetting
                                       {
                                           Enabled = false,
                                           JobType = typeof (GenerateNesProductAmountUpdateJob).ToString(),
                                           TimeInterval = 0,
                                           TimeHours =0,
                                           TimeMinutes =0,
                                           TimeType = TimeIntervalType.None
                                       }
                               };
        }

        public static TaskSqlSettings TaskSettings
        {
            get
            {
                var fromDB = SettingProvider.Items["TaskSqlSettings"];
                if (fromDB == null)
                    return GetDefault();
                var temp = JsonConvert.DeserializeObject<TaskSqlSettings>(Convert.ToString(fromDB));
                if (temp == null)
                    return GetDefault();
                return temp;
            }
            set
            {
                SettingProvider.Items["TaskSqlSettings"] = JsonConvert.SerializeObject(value);
            }
        }
    }

    public class TaskSqlSetting
    {
        public string JobType { get; set; }
        public bool Enabled { get; set; }
        public int TimeInterval { get; set; }
        public TimeIntervalType TimeType { get; set; }
        public int TimeHours { get; set; }
        public int TimeMinutes { get; set; }
    }
}