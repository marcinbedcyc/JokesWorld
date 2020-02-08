using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DbConnection
{
    /// <summary>
    /// Database's context.
    /// </summary>
    public class MyDbContext : DbContext
    {
        /// <summary>
        /// Users' set from db.
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Jokes' set from db.
        /// </summary>
        public DbSet<Joke> Jokes { get; set; }
        /// <summary>
        /// Comments' set from db.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Configure connection to db. In this case establish connection with file db 'mydb.db' (using Sqlite).
        /// </summary>
        /// <param name="options">Addtional option to configuration.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=mydb.db"); // Debug mode
        //=> options.UseSqlite("Data Source=" + Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName).ToString().Replace(@"\", @"\\") + "\\mydb.db");  // When run as windows service

        /// <summary>
        /// Set unique for some attributes in entities. Add relation between entitites, set action on delete. 
        /// </summary>
        /// <param name="modelBuilder"></param>
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
