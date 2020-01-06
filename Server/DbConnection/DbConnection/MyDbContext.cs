using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnection
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql("Server=192.168.99.100;Port=5432;Database=jokesdb;User id=marcinbedcyc;Password=password");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.UseSerialColumns();
            modelBuilder.Entity<User>().HasIndex(u => u.Nickname).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Joke>().HasIndex(j => j.Content).IsUnique();

            modelBuilder.Entity<Joke>()
           .HasOne(j => j.Author)
           .WithMany(a => a.Jokes)
           .HasForeignKey(j => j.AuthorFK)
           .IsRequired()
           .OnDelete(DeleteBehavior.Cascade);
        

            modelBuilder.Entity<Comment>()
           .HasOne(c => c.Author)
           .WithMany(a => a.Comments)
           .HasForeignKey(c => c.AuthorFK)
           .IsRequired()
           .OnDelete(DeleteBehavior.Cascade);
        

            modelBuilder.Entity<Comment>()
           .HasOne(c => c.Joke)
           .WithMany(j => j.Comments)
           .HasForeignKey(c => c.JokeFK)
           .IsRequired()
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
