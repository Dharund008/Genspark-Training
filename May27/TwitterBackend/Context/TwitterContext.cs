using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Twitter.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace Twitter.Contexts
{
    public class TwitterContext : DbContext
    {
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseNpgsql("User ID=postgres;Password=dharun23;Host=localhost;Port=5432;Database=SampleTwitter;");
        // }
        // public DbSet<User> user { get; set; }
        // public DbSet<Profile> Profile { get; set; } // this approach 


        public TwitterContext(DbContextOptions options) : base(options)
        {
            //constructor class
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Like> Likes { get; set; }

    }
}