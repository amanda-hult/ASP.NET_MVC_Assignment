using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<AddressEntity> Addresses { get; set; } = null!;
    //public DbSet<ClientEntity> Clients { get; set; } = null!;
    //public DbSet<ProjectEntity> Projects { get; set; } = null!;
    //public DbSet<StatusEntity> Statuses { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // configure projectuserentity

        modelBuilder.Entity<StatusEntity>().HasData(
            new StatusEntity { StatusId = 1, StatusName = "Not started" },
            new StatusEntity { StatusId = 2, StatusName = "Started" },
            new StatusEntity { StatusId = 3, StatusName = "Completed" }
        );
    }
}
