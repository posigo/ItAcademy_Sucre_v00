using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
               
        }
                
        //table application roles
        public DbSet<AppRole> AppRoles { get; set; }
        //Alarm and fire automatic protection table
        public DbSet<AsPaz> AsPazs { get; set; }
        //table application users
        public DbSet<AppUser> AppUsers { get; set; }
        //measurement channel table
        public DbSet<Canal> Canals { get; set; }
        //table of metering point locations
        public DbSet<Cex> Cexs { get; set; }
        //table of energy types
        public DbSet<Energy> Energies { get; set; }
        //user group table
        public DbSet<GroupUser> GroupUsers { get; set; }
        //table of parameter types
        public DbSet<ParameterType> ParameterTypes { get; set; }
        //table of metering points
        public DbSet<Point> Points { get; set; }
        //table RefreshTokens
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        //table reports
        public DbSet<Report> Reports { get; set; }
        //table report details
        //public DbSet<ReportDetail> ReportDetails { get; set; }        
        //daily values table
        public DbSet<ValueDay> ValuesDay { get; set; }
        //hourly value table
        public DbSet<ValueHour> ValuesHour { get; set; }
        //table of values by month
        public DbSet<ValueMounth> ValuesMounth { get; set; }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceTag> DeviceTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ValueDay>().HasKey(key => new { key.Id, key.Date });
            modelBuilder.Entity<ValueHour>().HasKey(key => new { key.Id, key.Date, key.Hour });
            modelBuilder.Entity<ValueMounth>().HasKey(key => new { key.Id, key.Date });
            //modelBuilder.Entity<ReportDetail>().HasNoKey();
            //modelBuilder.Entity<Canal>().Ignore(field => field.ReportDetails);
            //modelBuilder.Entity<Point>().Ignore(field => field.ReportDetails);
            //modelBuilder.Entity<Report>().Ignore(field => field.ReportDetails);

            //modelBuilder.Entity<ParameterType>().HasKey(x => x.Id);
            //modelBuilder.Entity<ParameterType>().Property(x => x.Id).ValueGeneratedOnAdd();

        }

    }
}
