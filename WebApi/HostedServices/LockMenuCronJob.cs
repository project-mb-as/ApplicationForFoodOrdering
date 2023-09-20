using Cronos;
using Domain.Data;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Services;

namespace WebApi.HostedServices
{
    public class LockMenuCronJob : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILogger<RemindToOrderCronJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LockMenuCronJob(ILogger<RemindToOrderCronJob> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _expression = CronExpression.Parse(@"0 14 * * *");
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            LockMenues();
            await ScheduleJob(cancellationToken);
        }

        protected async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    var menuService = scope.ServiceProvider.GetRequiredService<IMeniService>();

            //    var menu = menuService.GetByDate(DateTime.Now.Date);
            //    if (menu != null)
            //    {
            //        menu.Locked = true;
            //        menuService.CreateOrUpdate(menu);
            //    }
            //}
            LockMenues();
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }

        private void LockMenues()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var menuService = scope.ServiceProvider.GetRequiredService<IMeniService>();
                var menues = menuService.GetAll().ToList();
                foreach (var menu in menues)
                {
                    if (!menu.Locked && (menu.Datum.Subtract(DateTime.Now).TotalHours < 10))
                    {
                        menuService.Lock(menu);
                    }
                }
            }
        }

    }
}
