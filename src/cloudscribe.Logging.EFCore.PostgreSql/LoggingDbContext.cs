﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-11-10
// Last Modified:			2018-10-08
// 

using cloudscribe.EFCore.PostgreSql.Conventions;
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
                entity.ToTable("cs_SystemLog");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).IsRequired();

                entity.Property(p => p.LogDateUtc).HasColumnName("LogDate");

                entity.Property(p => p.IpAddress).HasMaxLength(50);

                entity.Property(p => p.Culture).HasMaxLength(10);

                entity.Property(p => p.ShortUrl).HasMaxLength(255);

                entity.Property(p => p.Thread).HasMaxLength(255);

                entity.Property(p => p.LogLevel).HasMaxLength(20);

                entity.Property(p => p.Logger).HasMaxLength(255);

            });

            modelBuilder.ApplySnakeCaseConventions();

        }

    }
}
