using Microsoft.EntityFrameworkCore;
using GenReq.Models;
using System;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using System.Drawing.Printing;

namespace GenReq.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GenRequest> GenRequest { get; set; }
        public DbSet<UserRegistration> UserRegistration { get; set; }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("in IDesignTimeDbContextFactory");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string SQLConnectSTR = "Data Source=trwiz-dev.database.windows.net;Initial Catalog=trywiz;Authentication=Active Directory Default;Encrypt=True;connect timeout=10000;";
            optionsBuilder.UseSqlServer(SQLConnectSTR);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

