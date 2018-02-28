using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;


namespace cloudscribe.Logging.EFCore.MSSQL.Migrations
{
    [DbContext(typeof(LoggingDbContext))]
    [Migration("20160701180450_changes20160701")]
    partial class changes20160701
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                //.HasAnnotation("Relational:Sequence:.LogIds", "'LogIds', '', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Logging.Web.LogItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:DefaultValueSql", "newid()");

                    b.Property<string>("Culture")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<int>("EventId");

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

                    b.Property<string>("StateJson");

                    b.Property<string>("Thread")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("cs_SystemLog");
                });
        }
    }
}
