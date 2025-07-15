using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Twitter.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
/*
Fluent-Api : flexible and more powerful than Data Annotations
--it is mostly used for complex validation rules, mapping to multiple tables, mapping relationships, keys, etc.

It has :
1) HasOne - used when current entity has one relationship with another entity.

2) HasMany - used when current entity has many relationships with another entity.

3) WithOne - used when related entity has one relationship with current entity.

4) WithMany - used when related entity has many relationships with current entity.

5) HasOneWithOne - used when current entity has one relationship with another entity and that entity also has one relationship with current entity.

6) HasManyWithMany - used when current entity has many relationships with another entity and that entity also has many relationships with current entity.

7) HasOneWithMany - used when current entity has one relationship with another entity and that entity has many relationships with current entity.

8) HasManyWithOne - used when current entity has many relationships with another entity and that entity has one relationship with current entity.

*/

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
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Like> Likes { get; set; }

        //make sure to have the property names assigned in class, with same spelling here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<UserProfile>()
                .HasKey(p => p.UserId);

            //post has : many to one user
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            //post - hashtag : a post can have many hashtags and a hashtag can be used in many posts
            modelBuilder.Entity<Post>()
                .HasMany(po => po.Hashtags)
                .WithMany(h => h.Posts);

            //user - like : a like can be made by one user and a user can have many likes.
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User) //each like belong to a user.
                .WithMany(u => u.Likes) //a user can have many likes.
                .HasForeignKey(l => l.UserId);

            //like - post : a like can be made on one post and a post can have many likes.
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId);

            //user - follower : a user can have many followers and a follower can follow many users.
            modelBuilder.Entity<Follower>()
                 .HasOne(f => f.FollowUser)
                 .WithMany()
                 .HasForeignKey(f => f.FollowerId)
                 .OnDelete(DeleteBehavior.Restrict);

            //oru user yethana peraiyu follow panalam..
            modelBuilder.Entity<Follower>()
                .HasOne(f => f.FollowingUser)
                .WithMany()
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}