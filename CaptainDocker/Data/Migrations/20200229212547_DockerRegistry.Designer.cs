﻿// <auto-generated />
using System;
using CaptainDocker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CaptainDocker.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200229212547_DockerRegistry")]
    partial class DockerRegistry
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CaptainDocker.Entities.DockerConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EngineApiUrl");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("DockerConnections");
                });

            modelBuilder.Entity("CaptainDocker.Entities.DockerRegistry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("DockerRegistries");
                });
#pragma warning restore 612, 618
        }
    }
}
