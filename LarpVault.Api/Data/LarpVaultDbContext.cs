using Microsoft.EntityFrameworkCore;

namespace LarpVault.Data;

public class LarpVaultDbContext : DbContext
{
    public LarpVaultDbContext(DbContextOptions<LarpVaultDbContext> options) 
        : base(options) { }

    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
}

public class PurchaseOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerEmail { get; set; } = string.Empty;
    public string PackName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    public string? DownloadLink { get; set; }
    public string Status { get; set; } = "Pending";
}