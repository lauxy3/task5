﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace TaskProcessor.Models
{
    public class TaskProDBContext : DbContext
    {
        // public ILogger _logger { get; }
        private readonly IConfiguration _env;
        public DbSet<TaskProcessorItem> TaskProcessItems { get; set; }
        public TaskProDBContext(DbContextOptions<TaskProDBContext> options, IConfiguration env)
            : base(options)
        {
            // _logger = logger;
            _env = env;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var server = _env["MSSQL_HOST"] ?? "ms-sql-server";
            var port = _env["MSSQL_PORT"] ?? "1433"; // Default SQL Server port
            var user = _env["MSSQL_USER"] ?? "SA"; // Warning do not use the SA account
            var password = _env["MSSQL_PASSWORD"] ?? "default";
            var database = _env["MSSQL_DATABASE"] ?? "default";
            // _logger.LogInformation("Connection string:" + $"Server={server},{port};Database={database};User={user};Password={password}"
            //    );
            optionsBuilder.UseSqlServer(
                //_env.GetConnectionString("DefaultURL")
                //_env.GetSection("cs:DefaultURL").Value);
                $"Server={server},{port};Database={database};User={user};Password={password}"
                );

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskProcessorItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
            });
        }
    }
}
