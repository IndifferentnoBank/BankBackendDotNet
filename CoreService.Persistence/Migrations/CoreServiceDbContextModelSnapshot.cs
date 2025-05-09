﻿// <auto-generated />
using System;
using CoreService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoreService.Persistence.Migrations
{
    [DbContext(typeof(CoreServiceDbContext))]
    partial class CoreServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CoreService.Domain.Entities.BankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("isClosed")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AccountNumber")
                        .IsUnique();

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("CoreService.Domain.Entities.ExpiredToken", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.ToTable("ExpiredTokens");
                });

            modelBuilder.Entity("CoreService.Domain.Entities.FireBaseToken", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<int>("Service")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Token");

                    b.ToTable("FireBaseTokens");
                });

            modelBuilder.Entity("CoreService.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<Guid>("BankAccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("RelatedLoanId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RelatedTransactionId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TransferBankAccountId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("CoreService.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("CoreService.Domain.Entities.BankAccount", "BankAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("CoreService.Domain.Entities.BankAccount", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
