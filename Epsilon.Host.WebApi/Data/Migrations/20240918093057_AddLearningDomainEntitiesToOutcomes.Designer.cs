﻿// <auto-generated />
using System;
using Epsilon.Host.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Epsilon.Host.WebApi.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240918093057_AddLearningDomainEntitiesToOutcomes")]
    partial class AddLearningDomainEntitiesToOutcomes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Epsilon.Abstractions.LearningDomain", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<Guid?>("ColumnsSetId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("RowsSetId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ValuesSetId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ColumnsSetId");

                    b.HasIndex("RowsSetId");

                    b.HasIndex("ValuesSetId");

                    b.ToTable("LearningDomains");
                });

            modelBuilder.Entity("Epsilon.Abstractions.LearningDomainOutcome", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("TenantId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ColumnId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DomainId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RowId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ValueId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id", "TenantId");

                    b.HasIndex("ColumnId");

                    b.HasIndex("DomainId");

                    b.HasIndex("RowId");

                    b.HasIndex("ValueId");

                    b.ToTable("LearningDomainOutcomes");
                });

            modelBuilder.Entity("Epsilon.Abstractions.LearningDomainType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("HexColor")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("LearningDomainTypes");
                });

            modelBuilder.Entity("Epsilon.Abstractions.LearningDomainTypeSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("LearningDomainTypeSet");
                });

            modelBuilder.Entity("LearningDomainTypeLearningDomainTypeSet", b =>
                {
                    b.Property<Guid>("SetsId")
                        .HasColumnType("char(36)");

                    b.Property<string>("TypesId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("SetsId", "TypesId");

                    b.HasIndex("TypesId");

                    b.ToTable("LearningDomainTypeSetTypes", (string)null);
                });

            modelBuilder.Entity("Epsilon.Abstractions.LearningDomain", b =>
                {
                    b.HasOne("Epsilon.Abstractions.LearningDomainTypeSet", "ColumnsSet")
                        .WithMany()
                        .HasForeignKey("ColumnsSetId");

                    b.HasOne("Epsilon.Abstractions.LearningDomainTypeSet", "RowsSet")
                        .WithMany()
                        .HasForeignKey("RowsSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Epsilon.Abstractions.LearningDomainTypeSet", "ValuesSet")
                        .WithMany()
                        .HasForeignKey("ValuesSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ColumnsSet");

                    b.Navigation("RowsSet");

                    b.Navigation("ValuesSet");
                });

            modelBuilder.Entity("Epsilon.Abstractions.LearningDomainOutcome", b =>
                {
                    b.HasOne("Epsilon.Abstractions.LearningDomainType", "Column")
                        .WithMany()
                        .HasForeignKey("ColumnId");

                    b.HasOne("Epsilon.Abstractions.LearningDomain", "Domain")
                        .WithMany()
                        .HasForeignKey("DomainId");

                    b.HasOne("Epsilon.Abstractions.LearningDomainType", "Row")
                        .WithMany()
                        .HasForeignKey("RowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Epsilon.Abstractions.LearningDomainType", "Value")
                        .WithMany()
                        .HasForeignKey("ValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Column");

                    b.Navigation("Domain");

                    b.Navigation("Row");

                    b.Navigation("Value");
                });

            modelBuilder.Entity("LearningDomainTypeLearningDomainTypeSet", b =>
                {
                    b.HasOne("Epsilon.Abstractions.LearningDomainTypeSet", null)
                        .WithMany()
                        .HasForeignKey("SetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Epsilon.Abstractions.LearningDomainType", null)
                        .WithMany()
                        .HasForeignKey("TypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
