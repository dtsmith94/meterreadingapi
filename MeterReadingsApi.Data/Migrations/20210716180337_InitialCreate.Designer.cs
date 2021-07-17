﻿// <auto-generated />
using System;
using MeterReadingsApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MeterReadingsApi.Data.Migrations
{
    [DbContext(typeof(MeterReadingsContext))]
    [Migration("20210716180337_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("MeterReadingsApi.Model.Data.CustomerAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CustomerAccounts");
                });

            modelBuilder.Entity("MeterReadingsApi.Model.Data.MeterReading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CustomerAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CustomerAccountId");

                    b.ToTable("MeterReadings");
                });

            modelBuilder.Entity("MeterReadingsApi.Model.Data.MeterReading", b =>
                {
                    b.HasOne("MeterReadingsApi.Model.Data.CustomerAccount", "CustomerAccount")
                        .WithMany("MeterReadings")
                        .HasForeignKey("CustomerAccountId");

                    b.Navigation("CustomerAccount");
                });

            modelBuilder.Entity("MeterReadingsApi.Model.Data.CustomerAccount", b =>
                {
                    b.Navigation("MeterReadings");
                });
#pragma warning restore 612, 618
        }
    }
}
