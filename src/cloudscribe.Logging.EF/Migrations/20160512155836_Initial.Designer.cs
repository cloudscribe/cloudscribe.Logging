using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using cloudscribe.Logging.EF;

namespace cloudscribe.Logging.EF.Migrations
{
    [DbContext(typeof(LoggingDbContext))]
    [Migration("20160512155836_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("Relational:Sequence:.LogIds", "'LogIds', '', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Logging.Web.LogItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "uniqueidentifier")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "newid()");

                    b.Property<string>("Culture")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("IpAddress")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("LogDateUtc")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "LogDate")
                        .HasAnnotation("SqlServer:ColumnType", "datetime")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "getutcdate()");

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

                    b.HasAnnotation("Relational:TableName", "cs_SystemLog");
                });
        }
    }
}
