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
    public class RemindToOrderCronJob : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILogger<RemindToOrderCronJob> _logger;
        IEmailService _emailService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RemindToOrderCronJob(ILogger<RemindToOrderCronJob> logger, IEmailService emailService, IServiceScopeFactory serviceScopeFactory)
        {
            _expression = CronExpression.Parse(@"0 13 * * *");
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            _serviceScopeFactory = serviceScopeFactory;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
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
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var recipients = userService.GetEmailsFromAllThatDidNotOrder(DateTime.Now.AddDays(1).Date);
                
                if (recipients != null && recipients.Count() > 0)
                {
                    _logger.LogInformation(recipients.ToString());
                    await SendEmailToRemind(recipients);
                }
            }
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

        #region helpers 
        private async Task SendEmailToRemind(List<string> recipients)
        {
            var emailBody = $"Poštovani,<br><br>" +
                $"Niste naručili hranu za sutra. To možete učiniti najkasnije danas do 14h.<br><br>" +
                $"Srdačan pozdrav i prijatno.<br> ";
            await _emailService.SendEmailToRecipientsAsinc(recipients, "Podsjetnik za nardžbu hrane", emailBody);

        }

        #endregion
    }
}
