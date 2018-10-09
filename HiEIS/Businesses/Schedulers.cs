using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;
using Quartz.Impl;
using HiEIS.Entities;
using System.Diagnostics;
using System.Collections.Specialized;

namespace HiEIS.Businesses
{
    [DisallowConcurrentExecution]
    public class NumberInvoiceJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var invoiceBusiness = new InvoiceBusiness();
                invoiceBusiness.NumberOutOfDateInvoices();
                Debug.WriteLine("Number Invoice Job - DONE!");
            });
        }
    }

    public class JobScheduler
    {
        private static IScheduler scheduler = null;

        public static void StartAsync()
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            scheduler = factory.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start();

            AddNumberInvoiceTask();
        }

        public static void AddNumberInvoiceTask()
        {
            string jobName = string.Format("job_number_invoice_{0:yyyyMMdd}", DateTime.Now);

            IJobDetail job = JobBuilder.Create<NumberInvoiceJob>()
                .Build();

            var date = DateBuilder.TodayAt(6, 0, 0);
            if (DateTime.Now.Hour >= 18)
            {
                date = DateBuilder.TomorrowAt(6, 0, 0);
            }
            else if (DateTime.Now.Hour >= 6)
            {
                date = DateBuilder.TodayAt(18, 0, 0);
            }

            var trigger = TriggerBuilder.Create()
            .StartNow()
            .WithDailyTimeIntervalSchedule(x => x
                .WithIntervalInHours(12)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(6, 0))
                .InTimeZone(TimeZoneInfo.Local))
            .Build();

            scheduler.Clear();
            scheduler.ScheduleJob(job, trigger).GetAwaiter().GetResult();
        }
    }

}