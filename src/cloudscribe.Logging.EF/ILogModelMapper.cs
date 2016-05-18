// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-26
// Last Modified:			2016-05-17
// 

using cloudscribe.Logging.Web;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cloudscribe.Logging.EF
{
    public interface ILogModelMapper
    {
        void Map(EntityTypeBuilder<LogItem> entity);
    }
}
