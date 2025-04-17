using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<AddressEntity> Addresses { get; set; } = null!;
    public DbSet<ClientEntity> Clients { get; set; } = null!;
    public DbSet<ProjectEntity> Projects { get; set; } = null!;
    public DbSet<StatusEntity> Statuses { get; set; } = null!;
    public DbSet<ProjectUserEntity> ProjectUsers { get; set; } = null!;
    public DbSet<NotificationEntity> Notification { get; set; } = null!;
    public DbSet<NotificationTargetGroupEntity> NotificationTargetGroups { get; set; } = null!;
    public DbSet<NotificationTypeEntity> NotificationTypes { get; set; } = null!;
    public DbSet<NotificationDisMissedEntity> DisMissedNotifications { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<ProjectUserEntity>()
            .HasKey(pu => new { pu.ProjectId, pu.UserId });

        modelBuilder.Entity<ProjectUserEntity>()
            .HasOne(x => x.Project)
            .WithMany(x => x.ProjectUsers)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectUserEntity>()
            .HasOne(x => x.User)
            .WithMany(x => x.ProjectUsers)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        // configure projectuserentity
        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.Client)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.Status)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<UserEntity>()
            .HasOne(x => x.Address)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<StatusEntity>().HasData(
            new StatusEntity { StatusId = 1, StatusName = "Not started" },
            new StatusEntity { StatusId = 2, StatusName = "Started" },
            new StatusEntity { StatusId = 3, StatusName = "Completed" }
        );

        modelBuilder.Entity<NotificationTypeEntity>().HasData(
            new NotificationTypeEntity { NotificationTypeId = 1, NotificationType = "User" },
            new NotificationTypeEntity { NotificationTypeId = 2, NotificationType = "Project" },
            new NotificationTypeEntity { NotificationTypeId = 3, NotificationType = "Client" }
        );

        modelBuilder.Entity<NotificationTargetGroupEntity>().HasData(
            new NotificationTargetGroupEntity { NotificationTargetGroupId = 1, TargetGroup = "AllUsers" },
            new NotificationTargetGroupEntity { NotificationTargetGroupId = 2, TargetGroup = "Admins" }
        );

        modelBuilder.Entity<NotificationEntity>()
            .Property(n => n.TargetGroupId)
            .HasDefaultValue( 1 );
    }
}
