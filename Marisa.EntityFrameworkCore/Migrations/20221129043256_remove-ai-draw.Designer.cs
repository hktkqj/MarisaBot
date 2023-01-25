﻿// <auto-generated />
using System;
using Marisa.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace QQBot.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(BotDbContext))]
    [Migration("20221129043256_remove-ai-draw")]
    partial class removeaidraw
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Marisa.EntityFrameworkCore.Entity.BlackList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("UId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UId");

                    b.ToTable("BlackList");
                });

            modelBuilder.Entity("Marisa.EntityFrameworkCore.Entity.Plugin.Arcaea.ArcaeaGuess", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimesCorrect")
                        .HasColumnType("int");

                    b.Property<int>("TimesStart")
                        .HasColumnType("int");

                    b.Property<int>("TimesWrong")
                        .HasColumnType("int");

                    b.Property<long>("UId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UId");

                    b.ToTable("Arcaea.Guess");
                });

            modelBuilder.Entity("Marisa.EntityFrameworkCore.Entity.Plugin.Chunithm.ChunithmGuess", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimesCorrect")
                        .HasColumnType("int");

                    b.Property<int>("TimesStart")
                        .HasColumnType("int");

                    b.Property<int>("TimesWrong")
                        .HasColumnType("int");

                    b.Property<long>("UId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UId");

                    b.ToTable("Chunithm.Guess");
                });

            modelBuilder.Entity("Marisa.EntityFrameworkCore.Entity.Plugin.MaiMaiDx.MaiMaiDxGuess", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimesCorrect")
                        .HasColumnType("int");

                    b.Property<int>("TimesStart")
                        .HasColumnType("int");

                    b.Property<int>("TimesWrong")
                        .HasColumnType("int");

                    b.Property<long>("UId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UId");

                    b.ToTable("MaiMaiDx.Guess");
                });

            modelBuilder.Entity("Marisa.EntityFrameworkCore.Entity.Plugin.Osu.OsuBind", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GameMode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OsuUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("OsuUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OsuUserId");

                    b.HasIndex("UserId");

                    b.ToTable("Osu.Bind");
                });
#pragma warning restore 612, 618
        }
    }
}
