using Email.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;



namespace Email.DataContext
{
    class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=104.211.223.222;Initial Catalog=MISReportingDb;User ID=mis;Password=mis;Encrypt=false;TrustServerCertificate=yes");
        }

        public DbSet<MIS_FieldData> MIS_FieldData { get; set; }
        public DbSet<MIS_Users> MIS_Users { get; set; }

        public DbSet<MIS_MISReport> MIS_MISReport { get; set; }
        public DbSet<MIS_VerticalMaster> MIS_VerticalMaster { get; set; }
        public DbSet<MIS_ProjectMaster> MIS_ProjectMaster { get; set; }



    }
}
