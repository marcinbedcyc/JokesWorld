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
        }
    }
}
