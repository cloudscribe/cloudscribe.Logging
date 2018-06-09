using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace cloudscribe.Logging.EFCore.pgsql.Migrations
{
    [DbContext(typeof(LoggingDbContext))]
    partial class LoggingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("Relational:Sequence:.LogIds", "'LogIds', '', '1', '1', '', '', 'Int64', 'False'");

            modelBuilder.Entity("cloudscribe.Logging.Web.LogItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Culture")
                        .HasMaxLength(10);

                    b.Property<int>("EventId");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LogDateUtc")
                        .HasColumnName("LogDate");

                    b.Property<string>("LogLevel")
                        .HasMaxLength(20);

                    b.Property<string>("Logger")
                        .HasMaxLength(255);

                    b.Property<string>("Message");

                    b.Property<string>("ShortUrl")
                        .HasMaxLength(255);

                    b.Property<string>("StateJson");

                    b.Property<string>("Thread")
                        .HasMaxLength(255);

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("cs_SystemLog");
                });
        }
    }
}
