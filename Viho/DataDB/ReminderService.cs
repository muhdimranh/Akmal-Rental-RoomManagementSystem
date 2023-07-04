using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Viho.web.DataDB
{


    public class ReminderService : BackgroundService
    {
        private readonly ILogger<ReminderService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ReminderService(ILogger<ReminderService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Run the task every 1 minute

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DbRentalContext>();

                    _logger.LogInformation("Reminder service running...");

                    // Get the tenants with overdue payments
                    var overdueTenants = dbContext.TbTenants
                        .Where(t => t.LastReminderDate <= DateTime.Now &&
                                    (t.TPaymentStatus == "Unpaid" || t.TPaymentStatus == "Paid") && !t.IsCheckedOut) 
                        .ToList();

                    _logger.LogInformation($"Number of overdue tenants: {overdueTenants.Count}");


                    foreach (var tenant in overdueTenants)
                    {
                        _logger.LogInformation($"Updating payment status for Tenant ID: {tenant.TId}");

                        // Send reminder to the tenant (e.g., using email or SMS)

                        // Update payment status and IsPaymentCollected
                        tenant.TPaymentStatus = "Unpaid";
                        tenant.IsPaymentCollected = false;

                        // Update the reminder date
                       // tenant.LastReminderDate = tenant.LastReminderDate.AddDays(28);
                    }

                    var currentDate = DateTime.Now.Day;
                    var pendingLocations = dbContext.TbLocations
                        .Where(l => l.IsPaymentMade && currentDate == l.LReminderDate)
                        .ToList();

                    _logger.LogInformation($"Number of pending landlords: {pendingLocations.Count}");

                    foreach (var location in pendingLocations)
                    {
                        _logger.LogInformation($"Updating payment status for Location ID: {location.LId}");

                        // Send reminder to the landlord (e.g., display on the dashboard)
                        // Check if it is the reminder day and IsPaymentMade is true
                        if (currentDate == location.LReminderDate && location.IsPaymentMade)
                        {
                            // Set IsPaymentMade to false
                            location.IsPaymentMade = false;

                            // Perform the reminder action (e.g., send the reminder to the landlord)
                            // ...
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
        }
    }





}
