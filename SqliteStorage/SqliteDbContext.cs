using BeastHunterData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqliteStorage
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Enemy> Enemies { get; set; }

        public DbSet<EnemyItem> EnemyItems { get; set; }

        public DbSet<Reputation> Reputations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnemyItem>()
                .HasKey(table => new {
                    table.EnemyId,
                    table.ItemId
                });
        }
    }
}
