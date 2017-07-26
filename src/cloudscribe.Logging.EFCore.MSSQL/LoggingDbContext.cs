// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-10
// Last Modified:			2017-07-26
// 

using cloudscribe.Logging.Web;
using Microsoft.EntityFrameworkCore;


namespace cloudscribe.Logging.EFCore.MSSQL
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
            modelBuilder.HasSequence<int>("LogIds")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<LogItem>(entity =>
            {
                entity.ToTable("cs_SystemLog");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("newid()")
                   .IsRequired();
                
                entity.Property(p => p.LogDateUtc)
                .HasColumnName("LogDate")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getutcdate()")
                ;

                entity.Property(p => p.IpAddress)
                .HasMaxLength(50)
                ;

                entity.Property(p => p.Culture)
                .HasMaxLength(10)
                ;

                entity.Property(p => p.ShortUrl)
                .HasMaxLength(255)
                ;

                entity.Property(p => p.Thread)
                .HasMaxLength(255)
                ;

                entity.Property(p => p.LogLevel)
                .HasMaxLength(20)
                ;

                entity.Property(p => p.Logger)
                .HasMaxLength(255)
                ;

                //Url
                //Message
            });

            // should this be called before or after we do our thing?

            base.OnModelCreating(modelBuilder);

        }


    }
}
