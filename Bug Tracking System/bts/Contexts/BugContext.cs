using Microsoft.EntityFrameworkCore;
using Bts.Models;

namespace Bts.Contexts
{
    public class BugContext : DbContext
    {
        public BugContext(DbContextOptions<BugContext> options) : base(options)
        {
        }

        public DbSet<Bug> Bugs { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Developer> Developers { get; set; } = null!;
        public DbSet<Tester> Testers { get; set; } = null!;
        public DbSet<UploadedFileLog> UploadedFileLogs { get; set; } = null!;

        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; } = null!;

        public DbSet<BugLog> BugLogs { get; set; } = null!;
        public DbSet<PasswordReset> PasswordResets { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasKey(a => a.Id);
            modelBuilder.Entity<Developer>().HasKey(d => d.Id);
            modelBuilder.Entity<Tester>().HasKey(t => t.Id);
            modelBuilder.Entity<Bug>().HasKey(b => b.Id);
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<UploadedFileLog>().HasKey(up => up.Id);
            modelBuilder.Entity<PasswordReset>().HasKey(p => p.Id);

           modelBuilder.Entity<Developer>().HasQueryFilter(d => !d.IsDeleted);
           modelBuilder.Entity<Tester>().HasQueryFilter(t => !t.IsDeleted);



            // BUG
            modelBuilder.Entity<Bug>()
                .HasOne(b => b.CreatedByTester)
                .WithMany(t => t.Bugs)
                .HasForeignKey(b => b.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict) 
                .IsRequired(false);  
            modelBuilder.Entity<Bug>()
                .HasOne(b => b.AssignedToDeveloper)
                .WithMany(d => d.Bugs)
                .HasForeignKey(b => b.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict) 
                .IsRequired(false);  

            // USER (1-to-1s)
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.Id)
                .HasConstraintName("FK_User_Admin")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tester>()
                .HasOne(t => t.User)
                .WithOne(u => u.Tester)
                .HasForeignKey<Tester>(t => t.Id)
                .HasConstraintName("FK_User_Tester")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Developer>()
                .HasOne(d => d.User)
                .WithOne(u => u.Developer)
                .HasForeignKey<Developer>(d => d.Id)
                .HasConstraintName("FK_User_Developer")
                .OnDelete(DeleteBehavior.Restrict);

            // COMMENT
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .HasConstraintName("FK_User_Comments")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Bug)
                .WithMany()
                .HasForeignKey(c => c.BugId)
                .OnDelete(DeleteBehavior.Restrict);

            // ENUMS (optional, for PostgreSQL support)
            modelBuilder
                .Entity<Bug>()
                .Property(b => b.Priority)
                .HasConversion<string>();

            modelBuilder
                .Entity<Bug>()
                .Property(b => b.Status)
                .HasConversion<string>();
        }
    }
}
