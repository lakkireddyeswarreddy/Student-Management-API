
namespace StudentManagementAPI.BackgroundServices
{
    public class CurrentTimeService : BackgroundService
    {
        private readonly ILogger<CurrentTimeService> _logger;

        public CurrentTimeService(ILogger<CurrentTimeService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Current Time : {DateTime.Now : hh:mm:ss}");

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
