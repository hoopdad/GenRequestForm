using Microsoft.EntityFrameworkCore;
using GenReq.Models;
using System;

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
}