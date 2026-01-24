using Microsoft.EntityFrameworkCore;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Locker> Lockers { get; set; }
}