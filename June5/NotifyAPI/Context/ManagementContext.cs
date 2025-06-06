using NotifyAPI.Models;
using NotifyAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace NotifyAPI.Contexts
{
    public class ManagementContext : DbContext
    {
        public ManagementContext(DbContextOptions<ManagementContext> options) : base(options)
        {
        }

        public DbSet<Register> Registers { get; set; } = null!;
        public DbSet<HRAdmin> HRAdmins { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<FileDocument> FileDocuments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOne(p => p.Register)
                                        .WithOne(u => u.User)
                                        .HasForeignKey<User>(p => p.Email)
                                        .HasConstraintName("FK_User_Register")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HRAdmin>().HasOne(p => p.Register)
                                        .WithOne(u => u.HRAdmin)
                                        .HasForeignKey<HRAdmin>(p => p.Email)
                                        .HasConstraintName("FK_HRAdmin_Register")
                                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}