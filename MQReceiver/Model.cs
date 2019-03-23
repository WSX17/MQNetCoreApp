using Microsoft.EntityFrameworkCore;
using System.Configuration;
namespace MQReceiver
{

    public class PersonContext : DbContext
    {
        public DbSet<PersonExt> PersonsDb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@ConfigurationManager.AppSettings["dbpath"]);
        }
    }

}