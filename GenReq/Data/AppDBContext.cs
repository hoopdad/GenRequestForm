using Microsoft.EntityFrameworkCore;
using GenReq.Models;
using System;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;

namespace GenReq.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GenRequest> Reminders { get; set; }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string SQLConnectSTR = "Server=tcp:trwiz-dev.database.windows.net,1433;Initial Catalog=trywiz;Persist Security Info=False;User ID=svc_webuser_dev@mikeolivieris.onmicrosoft.com;Password=Cabbo2007!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=\"Active Directory Password\";";//Configuration["ConnectionString:GenReqContext"];
            optionsBuilder.UseSqlServer(SQLConnectSTR);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

