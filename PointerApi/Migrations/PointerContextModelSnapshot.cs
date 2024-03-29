﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PointerApi.Data;

namespace PointerApi.Migrations
{
    [DbContext(typeof(PointerContext))]
    partial class PointerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PointerApi.Models.ConsumingApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ApiKey")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ApplicationDescription")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ApplicationName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("DateEntered")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecretKey")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ConsumingApplication");
                });

            modelBuilder.Entity("PointerApi.Models.PointerModel", b =>
                {
                    b.Property<string>("Action")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Address_Status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Alt_Thorfare_Name1")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Archived_Date")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BLPU")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Building_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Building_Number")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Building_Status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Classification")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Commencement_Date")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("County")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Creation_Date")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Local_Council")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Locality")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Organisation_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Postcode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Posttown")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Primary_Thorfare")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Secondary_Thorfare")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Sub_Building_Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Temp_Coords")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Town")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Townland")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UDPRN")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UPRN")
                        .HasColumnType("int");

                    b.Property<int>("USRN")
                        .HasColumnType("int");

                    b.Property<int>("Unique_Building_ID")
                        .HasColumnType("int");

                    b.Property<int>("X_COR")
                        .HasColumnType("int");

                    b.Property<int>("Y_COR")
                        .HasColumnType("int");

                    b.ToTable("Pointer");
                });
#pragma warning restore 612, 618
        }
    }
}
