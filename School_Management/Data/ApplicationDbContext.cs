﻿using Microsoft.EntityFrameworkCore;
using School_Management.Models;

namespace School_Management.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Seed initial roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "Teacher" },
                new Role { Id = 3, RoleName = "Student" }
            );
        }
    }
}