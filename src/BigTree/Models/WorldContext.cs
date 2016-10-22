using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BigTree.Models
{
    public class WorldContext : DbContext
    {

        private IConfigurationRoot _config;

        public WorldContext(IConfigurationRoot config, DbContextOptions options) : base(options)
        {

            _config = config;
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //Specify the DB provider
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:WorldContextConnection"]);
        }



    }
}
