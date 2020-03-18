﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Terra.Data;

namespace Terra.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200314142355_UpdatedStatModel")]
    partial class UpdatedStatModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Terra.Models.Characters", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Age")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("Gender")
                        .IsRequired();

                    b.Property<string>("Height");

                    b.Property<string>("Job")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Origin")
                        .IsRequired();

                    b.Property<string>("Race")
                        .IsRequired();

                    b.Property<string>("Weight");

                    b.HasKey("Id");

                    b.ToTable("Character");
                });

            modelBuilder.Entity("Terra.Models.Game", b =>
                {
                    b.Property<Guid>("GameId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Picture");

                    b.Property<string>("Platform");

                    b.Property<string>("ReleaseDate");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("GameId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Terra.Models.Monster", b =>
                {
                    b.Property<Guid>("MonsterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddedBy");

                    b.Property<int>("Attack");

                    b.Property<int>("Defense");

                    b.Property<string>("Description");

                    b.Property<string>("ElementalAffinity");

                    b.Property<string>("ElementalWeakness");

                    b.Property<string>("Game");

                    b.Property<int>("HitPoints");

                    b.Property<string>("JapaneseName");

                    b.Property<int>("ManaPoints");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Picture");

                    b.HasKey("MonsterId");

                    b.ToTable("Monsters");
                });

            modelBuilder.Entity("Terra.Models.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CollectionId");

                    b.Property<int>("Primary");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("Terra.Models.Stat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Agility");

                    b.Property<int>("Attack");

                    b.Property<Guid>("CollectionId");

                    b.Property<int>("Defense");

                    b.Property<int>("HitPoints");

                    b.Property<int>("Magic");

                    b.Property<int>("MagicDefense");

                    b.Property<int>("ManaPoints");

                    b.Property<string>("Platform");

                    b.Property<int>("Spirit");

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("Terra.Models.Picture", b =>
                {
                    b.HasOne("Terra.Models.Characters", "Character")
                        .WithMany("Pictures")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Terra.Models.Stat", b =>
                {
                    b.HasOne("Terra.Models.Characters", "Character")
                        .WithMany("Stats")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
