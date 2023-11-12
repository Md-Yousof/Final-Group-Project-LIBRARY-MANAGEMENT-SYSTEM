using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.services
{
    public class ApprovalExpirationService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<ApprovalExpirationService> _logger;

        public ApprovalExpirationService(IServiceProvider provider, ILogger<ApprovalExpirationService> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for overdue approvals...");

                using (var scope = _provider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Calculate the time 24 hours ago
                    var twentyFourHoursAgo = DateTime.Now.AddHours(-24);

                    // Find requested books older than 24 hours
                    var overdueBooks = await dbContext.BorrowedBooks
                        .Where(b => b.Status == "Requested" && b.RequestTimestamp < twentyFourHoursAgo)
                        .ToListAsync();

                    foreach (var overdueBook in overdueBooks)
                    {
                        overdueBook.Status = "Cancelled";
                        overdueBook.Comment = "Automatic Cancel";

                        // Update the book copy availability
                        var bookCopy = await dbContext.Copies.FindAsync(overdueBook.BookCopyId);
                        if (bookCopy != null)
                        {
                            bookCopy.IsAvailable = true;
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Check every hour
            }
        }
    }

}
