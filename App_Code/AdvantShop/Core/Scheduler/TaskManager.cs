//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System.Configuration;

namespace AdvantShop.Core.Scheduler
{
    //pattern singleton 
    public class TaskManager
    {
        private static readonly TaskManager taskManager = new TaskManager();
        private readonly IScheduler _sched;
        public const string DataMap = "data";
        private const string TaskGroup = "TaskGroup";
        private const string WebConfigGrop = "WebConfigGrop";

        private TaskManager()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            // get a scheduler
            _sched = schedFact.GetScheduler();
        }

        public static TaskManager TaskManagerInstance()
        {
            return taskManager;
        }

        public void Init()
        {
            try
            {
                //Added By Evgeni
                var config = (List<XmlNode>)ConfigurationManager.GetSection("CustomGroup/TasksConfig");

                foreach (XmlNode nodeItem in config)
                {
                    //cheak for null
                    if (nodeItem.Attributes == null || string.IsNullOrEmpty(nodeItem.Attributes["name"].Value) ||
                        string.IsNullOrEmpty(nodeItem.Attributes["type"].Value) || string.IsNullOrEmpty(nodeItem.Attributes["cronExpression"].Value)) return;

                    string jobName = nodeItem.Attributes["name"].Value;
                    string jobType = nodeItem.Attributes["type"].Value;
                    string cronExpression = nodeItem.Attributes["cronExpression"].Value;
                    bool enabled = nodeItem.Attributes["enabled"].Value.TryParseBool();

                    if (!enabled) continue;

                    var taskType = Type.GetType(jobType);
                    if (taskType == null) continue;

                    // construct job info
                    var jobDetail = new JobDetailImpl(jobName, WebConfigGrop, taskType);

                    // каждый класс сам обработает хmlNode для себя
                    jobDetail.JobDataMap[DataMap] = nodeItem;
                    //http://www.quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger

                    var trigger = new CronTriggerImpl(jobName, WebConfigGrop, jobName, WebConfigGrop, cronExpression);
                    _sched.ScheduleJob(jobDetail, trigger);
                }

                var tType = Type.GetType("AdvantShop.Core.Scheduler.LicJob");
                var jDetail = new JobDetailImpl("LicJob", WebConfigGrop, tType);
                var trig = new CronTriggerImpl("LicJob", WebConfigGrop, "LicJob", WebConfigGrop, "0 59 0 1/1 * ?");
                _sched.ScheduleJob(jDetail, trig);
            }
            catch { }
        }

        public void ManagedTask(TaskSqlSettings settings)
        {
            //Added by Evgeni
            try
            {
                for (int i = 0; i < settings.Count; i++)
                {
                    _sched.DeleteJob(new JobKey(settings[i].JobType, TaskGroup));
                }

                for (int i = 0; i < settings.Count; i++)
                {
                    if (settings[i].Enabled)
                    {
                        if (string.IsNullOrEmpty(settings[i].JobType)) continue;

                        var taskType = Type.GetType(settings[i].JobType);
                        if (taskType == null) continue;
                        var jobDetail = new JobDetailImpl(settings[i].JobType, TaskGroup, taskType);
                        string cronExpression = GetCronString(settings[i]);
                        if (string.IsNullOrEmpty(cronExpression)) continue;
                        var trigger = new CronTriggerImpl(settings[i].JobType, TaskGroup, settings[i].JobType, TaskGroup, cronExpression);
                        _sched.ScheduleJob(jobDetail, trigger);
                    }
                }
            }
            catch { }
        }

        public string GetTasks()
        {
            var res = (from jobGroupName in _sched.GetJobGroupNames()
                       from jobkey in _sched.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(jobGroupName))
                       select _sched.GetJobDetail(jobkey)).ToList();

            string resT = string .Empty ;
            foreach (var item in res)
            {
                resT += ";" + item.JobType.ToString();
            }
            return resT;
        }

        private string GetCronString(TaskSqlSetting item)
        {
            switch (item.TimeType)
            {
                case TimeIntervalType.Days:
                    return string.Format("0 {2} {1} 1/{0} * ?", item.TimeInterval, item.TimeHours, item.TimeMinutes);
                case TimeIntervalType.Hours:
                    return string.Format("0 0 0/{0} * * ?", item.TimeInterval);
                case TimeIntervalType.Minutes:
                    return string.Format("0 0/{0} * * * ?", item.TimeInterval);
            }
            return string.Empty;
        }

        public void Start()
        {
            _sched.Start();
        }
    }
}