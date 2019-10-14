// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-11-10
// Last Modified:			2019-09-30
// 


using cloudscribe.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Logging.EFCore.PostgreSql
{
    public class LoggingDbContext : LoggingDbContextBase, ILoggingDbContext
    {

        public LoggingDbContext(
            DbContextOptions<LoggingDbContext> options) : base(options)
        {
            // we don't want to track any logitems because we dont edit them
            // we add them delete them and view them
            //ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasSequence<long>("LogIds")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<LogItem>(entity =>
            {
                entity.ToTable("cs_system_log");
                entity.HasKey(p => p.Id).HasName("pk_cs_system_log");

                entity.Property(p => p.Id).HasColumnName("id").IsRequired();

                entity.Property(p => p.LogDateUtc).HasColumnName("log_date");

                entity.Property(p => p.IpAddress).HasColumnName("ip_address").HasMaxLength(50);

                entity.Property(p => p.Culture).HasColumnName("culture").HasMaxLength(10);

                entity.Property(p => p.Url).HasColumnName("url");

                entity.Property(p => p.ShortUrl).HasColumnName("short_url").HasMaxLength(255);

                entity.Property(p => p.Thread).HasColumnName("thread").HasMaxLength(255);

                entity.Property(p => p.LogLevel).HasColumnName("log_level").HasMaxLength(20);

                entity.Property(p => p.Logger).HasColumnName("logger").HasMaxLength(255);

                entity.Property(p => p.Message).HasColumnName("message");

                entity.Property(p => p.StateJson).HasColumnName("state_json");
                entity.Property(p => p.EventId).HasColumnName("event_id");

            });

          

        }

    }
}
