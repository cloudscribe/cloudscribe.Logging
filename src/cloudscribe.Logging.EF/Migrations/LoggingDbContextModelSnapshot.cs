using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.Logging.EF;

namespace cloudscribe.Logging.EF.Migrations
{
    [DbContext(typeof(LoggingDbContext))]
    partial class LoggingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901")
                .HasAnnotation("Relational:Sequence:.LogIds", "'LogIds', '', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Logging.Web.LogItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("Culture")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("IpAddress")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("LogDateUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("LogDate")
                        .HasAnnotation("SqlServer:ColumnType", "datetime")
                        .HasAnnotation("SqlServer:DefaultValueSql", "getutcdate()");

                    b.Property<string>("LogLevel")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("Logger")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Message");

                    b.Property<string>("ShortUrl")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Thread")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("cs_SystemLog");
                });
        }
    }
}
