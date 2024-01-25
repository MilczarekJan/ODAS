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
            modelBuilder.Entity<BankTransfer>().HasKey(p => p.Id);

            modelBuilder.Entity<BankTransfer>()
                .Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<BankTransfer>()
                .Property(p => p.Amount)
                .IsRequired()
                .HasPrecision(2);

            modelBuilder.Entity<BankTransfer>()
             .Property(p => p.Title)
             .IsRequired()
             .HasMaxLength(200);

            modelBuilder.Entity<BankTransfer>()
            .Property(p => p.Sender_Id)
            .IsRequired();

            modelBuilder.Entity<BankTransfer>()
            .Property(p => p.Sender_Email)
            .IsRequired();

            modelBuilder.Entity<BankTransfer>()
            .Property(p => p.Recipient_Id)
            .IsRequired();

            modelBuilder.Entity<BankTransfer>()
            .Property(p => p.Recipient_Name)
            .IsRequired();

            modelBuilder.Entity<BankTransfer>()
            .Property(p => p.Recipient_Email)
            .IsRequired();

            modelBuilder.Entity<BankTransfer>().HasData(BankTransferSeeder.GenerateTransferData());
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