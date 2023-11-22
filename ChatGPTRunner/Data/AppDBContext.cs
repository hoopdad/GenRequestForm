﻿using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using System.Drawing.Printing;
using ChatGPTRunner.Models;

namespace ChatGPTRunner.Data
{
    public class ApplicationDbContext : DbContext
    {
        public string DbPath { get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext()
        {
            DbPath = Environment.GetEnvironmentVariable("ConnectionString:GenReqContext");

            if (DbPath == null)
            {
                DbPath = "";
                Console.Error.WriteLine("Environment misconfigured. Need value for ConnectionString:GenReqContext");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(DbPath);


        public DbSet<GenRequest> GenRequest { get; set; }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("in IDesignTimeDbContextFactory");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string SQLConnectSTR = "Data Source=trwiz-dev.database.windows.net;Initial Catalog=trywiz;Authentication=Active Directory Default;Encrypt=True;connect timeout=10000;";
            SQLConnectSTR = "Server=tcp:trwiz-dev.database.windows.net,1433;Initial Catalog=trywiz;Persist Security Info=False;User ID=svc_webuser_dev@mikeolivieris.onmicrosoft.com;Password=Cabbo2007!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=\"Active Directory Password\";";//Configuration["ConnectionString:GenReqContext"];
            optionsBuilder.UseSqlServer(SQLConnectSTR);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
