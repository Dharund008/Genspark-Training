using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Video.Models;

namespace Video.Contexts
{
    public class TrainingVideoContext : DbContext
    {

        public TrainingVideoContext(DbContextOptions<TrainingVideoContext> options) : base(options)
        {
        }
        public DbSet<TrainingVideo> TrainingVideos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingVideo>().HasKey(a => a.Id);
        }
    }

}