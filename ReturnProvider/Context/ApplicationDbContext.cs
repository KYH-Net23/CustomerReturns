using Microsoft.EntityFrameworkCore;
using ReturnProvider.Models.Entities;

namespace ReturnProvider;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ReturnEntity> Returns { get; set; }
}
