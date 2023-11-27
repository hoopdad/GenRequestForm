using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using System.Drawing.Printing;
using ChatGPTRunner.Models;
using GenReq.Models;

namespace ChatGPTRunner.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public MyContext MyAppContext { get; set; }
        public ApplicationDbContext(MyContext cntxt)
        {
            MyAppContext = cntxt;
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(MyAppContext.DbPath);


        public DbSet<GenRequest> GenRequest { get; set; }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("in IDesignTimeDbContextFactory");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string SQLConnectSTR = "Data Source=trwiz-dev.database.windows.net;Initial Catalog=trywiz;Authentication=Active Directory Default;Encrypt=True;connect timeout=10000;";
            optionsBuilder.UseSqlServer(SQLConnectSTR);
            MyContext cntxt = new MyContext();
            cntxt.DbPath = SQLConnectSTR;
            ApplicationDbContext dbContxt = new ApplicationDbContext(optionsBuilder.Options);
            dbContxt.MyAppContext = cntxt;

            return dbContxt;
        }
    }
}

