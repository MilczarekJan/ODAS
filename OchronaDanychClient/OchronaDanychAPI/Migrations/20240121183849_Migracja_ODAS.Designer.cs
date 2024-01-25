﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OchronaDanychAPI.Models;

#nullable disable

namespace OchronaDanychAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240121183849_Migracja_ODAS")]
    partial class Migracja_ODAS
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OchronaDanychShared.Auth.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("LettersHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("LettersSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OchronaDanychShared.Models.BankTransfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasPrecision(2)
                        .HasColumnType("float(2)");

                    b.Property<string>("Recipient_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Recipient_Id")
                        .HasColumnType("int");

                    b.Property<string>("Recipient_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sender_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sender_Id")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("BankTransfers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 427.0,
                            Recipient_Email = "Briana88@gmail.com",
                            Recipient_Id = 0,
                            Recipient_Name = "Gia Emmerich",
                            Sender_Email = "Flossie.Braun@hotmail.com",
                            Sender_Id = 0,
                            Title = "Licensed Granite Bacon hacking port Denmark"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 3567.0,
                            Recipient_Email = "Olaf.Abernathy@gmail.com",
                            Recipient_Id = 1,
                            Recipient_Name = "Joseph Kuhic",
                            Sender_Email = "Merle28@yahoo.com",
                            Sender_Id = 1,
                            Title = "Plains monitor ROI Djibouti Franc"
                        },
                        new
                        {
                            Id = 3,
                            Amount = 3213.0,
                            Recipient_Email = "Merle.Bergstrom9@yahoo.com",
                            Recipient_Id = 2,
                            Recipient_Name = "Jerrod Bartell",
                            Sender_Email = "Eusebio.Armstrong86@yahoo.com",
                            Sender_Id = 2,
                            Title = "framework hacking quantifying Investment Account"
                        },
                        new
                        {
                            Id = 4,
                            Amount = 3680.0,
                            Recipient_Email = "Edgardo_Fritsch54@gmail.com",
                            Recipient_Id = 3,
                            Recipient_Name = "Destiny Blanda",
                            Sender_Email = "Antone45@yahoo.com",
                            Sender_Id = 3,
                            Title = "navigate monitor Isle payment"
                        },
                        new
                        {
                            Id = 5,
                            Amount = 3607.0,
                            Recipient_Email = "Randall.Bruen6@gmail.com",
                            Recipient_Id = 4,
                            Recipient_Name = "Hermann Nikolaus",
                            Sender_Email = "Gay_Gottlieb@hotmail.com",
                            Sender_Id = 4,
                            Title = "Unbranded Rubber Ball empower Concrete Plains"
                        },
                        new
                        {
                            Id = 6,
                            Amount = 3260.0,
                            Recipient_Email = "Providenci98@gmail.com",
                            Recipient_Id = 5,
                            Recipient_Name = "Dortha Bechtelar",
                            Sender_Email = "Kane.Abshire@yahoo.com",
                            Sender_Id = 5,
                            Title = "Dynamic synthesizing deliverables static"
                        },
                        new
                        {
                            Id = 7,
                            Amount = 4317.0,
                            Recipient_Email = "Tristian.Upton@yahoo.com",
                            Recipient_Id = 6,
                            Recipient_Name = "Ariane Kessler",
                            Sender_Email = "Emerson.Spencer57@yahoo.com",
                            Sender_Id = 6,
                            Title = "Auto Loan Account index invoice Colombian Peso"
                        },
                        new
                        {
                            Id = 8,
                            Amount = 1882.0,
                            Recipient_Email = "Nayeli_Steuber@yahoo.com",
                            Recipient_Id = 7,
                            Recipient_Name = "Heber Bogan",
                            Sender_Email = "Lester.Thompson82@gmail.com",
                            Sender_Id = 7,
                            Title = "Home & Jewelery Health compressing Jewelery & Home"
                        },
                        new
                        {
                            Id = 9,
                            Amount = 4051.0,
                            Recipient_Email = "Mariam40@yahoo.com",
                            Recipient_Id = 8,
                            Recipient_Name = "Simone Torp",
                            Sender_Email = "Bradford.Bartell94@yahoo.com",
                            Sender_Id = 8,
                            Title = "Dynamic Unbranded Wooden Ball Right-sized bandwidth-monitored"
                        },
                        new
                        {
                            Id = 10,
                            Amount = 563.0,
                            Recipient_Email = "Leonor44@yahoo.com",
                            Recipient_Id = 9,
                            Recipient_Name = "Christopher Gottlieb",
                            Sender_Email = "Anjali.OKon85@yahoo.com",
                            Sender_Id = 9,
                            Title = "SAS Corporate systems Mountain"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
