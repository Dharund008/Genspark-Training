using Microsoft.Extensions.Hosting;
using Online.Contexts;
using Microsoft.EntityFrameworkCore;
using Online.Models;

namespace Online.Services
{
    public class ProductExpiryService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductExpiryService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MigrationContext>();
                    var now = DateTime.UtcNow;

                    var expiredProducts = await context.Products
                        .Where(p => !p.IsSaleEnded && p.SellEndDate < now)
                        .ToListAsync();

                    foreach (var product in expiredProducts)
                    {
                        product.IsSaleEnded = true;
                    }

                    if (expiredProducts.Any())
                    {
                        await context.SaveChangesAsync();
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // runs every 10 minutes
            }
        }
    }

}