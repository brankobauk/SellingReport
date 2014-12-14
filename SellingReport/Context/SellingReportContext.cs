using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SellingReport.Migrations;
using SellingReport.Models.Models;

namespace SellingReport.Context
{
    public class SellingReportContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<ProductSellingPlan> ProductSellingPlans { get; set; }
        public DbSet<ProductSellingReport> ProductSellingReports { get; set; }
        public DbSet<VariableHoliday> VariableHolidays { get; set; }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SellingReportContext, Configuration>());
        }
    }  
}