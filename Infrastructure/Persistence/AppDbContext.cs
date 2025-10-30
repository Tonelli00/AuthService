using Application.UseCase.HashUseCase;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(25);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(25);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Password).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);

                entity.HasOne(e => e.Role)
                      .WithMany(e => e.Users)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasData(
                    new User
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Álvaro",
                        LastName = "Ramirez",
                        Email = "alvaro@gmail.com",
                        Password = new HashingService().encryptSHA256("123"),
                        Phone = "1234567890",
                        RoleId = 3
                    });

                modelBuilder.Entity<Role>(entity =>
                {
                    entity.HasKey(e => e.RoleId);
                    entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
                    entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
                    entity.HasIndex(e => e.Name).IsUnique();
                    entity.HasMany(e => e.Users)
                          .WithOne(e => e.Role)
                          .HasForeignKey(e => e.RoleId)
                          .OnDelete(DeleteBehavior.Cascade);

                    entity.HasData(
                        new Role { RoleId = 1, Name = "Current" },
                        new Role { RoleId = 2, Name = "Admin" },
                        new Role { RoleId = 3, Name = "SuperAdmin" }
                    );
                });
            });

        }
    }
}
