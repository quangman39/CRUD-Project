using Microsoft.EntityFrameworkCore;

namespace Enities
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Seed countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> seedCountry = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (var country in seedCountry)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            string personJson = System.IO.File.ReadAllText("people.json");

            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personJson);
            foreach (var person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }







        }
    }
}
