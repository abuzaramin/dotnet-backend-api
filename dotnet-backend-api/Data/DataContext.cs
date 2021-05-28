using System;
using dotnet_backend_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace dotnet_backend_api.Data
{
    public class DataContext : DbContext
    {
        IConfiguration _configuration;

        //public DataContext()
        //{
          
        //}

        //public DataContext(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    if (!options.IsConfigured)
        //    {
        //        options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        //    }
        //}

        public DbSet<Marvel> Marvels { get; set; }
    }
}
