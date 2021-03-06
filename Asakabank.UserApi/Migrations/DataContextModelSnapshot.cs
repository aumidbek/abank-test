// <auto-generated />
using System;
using Asakabank.UserApi.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Asakabank.UserApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Asakabank.UserApi.Entities.DbUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Firstname")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsIdentified")
                        .HasColumnType("boolean");

                    b.Property<string>("Lastname")
                        .HasColumnType("text");

                    b.Property<string>("Middlename")
                        .HasColumnType("text");

                    b.Property<string>("OTP")
                        .HasColumnType("text");

                    b.Property<DateTime>("OTPSentTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Passport")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0ebae847-3b98-4372-93a8-1499f21f3ae8"),
                            CreatedAt = new DateTime(2021, 5, 25, 15, 4, 59, 934, DateTimeKind.Local).AddTicks(978),
                            IsDeleted = false,
                            IsIdentified = false,
                            OTPSentTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Password = "$2a$11$TNpoNe8DmA3jvddpKTE/7O/NpbusaIzaM10KZ3RFxBrIivviAHQHG",
                            Username = "test1"
                        },
                        new
                        {
                            Id = new Guid("d4dd5488-22b3-42cc-8119-e5329aeb7770"),
                            CreatedAt = new DateTime(2021, 5, 25, 15, 5, 0, 104, DateTimeKind.Local).AddTicks(7091),
                            IsDeleted = false,
                            IsIdentified = false,
                            OTPSentTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Password = "$2a$11$kAJo7B8Ob4sYTjdhkY/Re.TVc/9mwYhzlywpMBaGj.e3qnEOOxFde",
                            Username = "test2"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
