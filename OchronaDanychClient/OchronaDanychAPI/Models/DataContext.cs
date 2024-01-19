using Microsoft.EntityFrameworkCore;
using OchronaDanychShared.Models;
using OchronaDanychShared.Auth;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OchronaDanychAPI.Models
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<BankTransfer> BankTransfers { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // fluent api 
            modelBuilder.Entity<Shoe>().HasKey(p => p.Id);

            modelBuilder.Entity<Shoe>()
                .Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Shoe>()
                .Property(p => p.ShoeSize)
                .IsRequired();

            modelBuilder.Entity<Shoe>()
             .Property(p => p.Description)
             .HasMaxLength(200);

            modelBuilder.Entity<Shoe>()
            .Property(p => p.Name)
            .HasMaxLength(100);


            // data seed 

            modelBuilder.Entity<Shoe>().HasData(ShoeSeeder.GenerateShoeData());
        }
    }
}
// instalacja dotnet ef 
//dotnet tool install --global dotnet-ef

// aktualizacja 
//dotnet tool update --global dotnet-ef

// dotnet ef migrations add [nazwa_migracji]
// dotnet ef database update 


// cofniecie migraji niezaplikowanych 
//dotnet ef migrations remove