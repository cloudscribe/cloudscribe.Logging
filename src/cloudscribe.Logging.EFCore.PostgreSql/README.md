﻿# cloudscribe.Logging.EFCore.pgsql

## How to generate migrations

Since this project is a netstandard20 class library it is not executable, therefore you have to pass in the --startup-project that is executable

dotnet ef --startup-project ../Demo.WebApp migrations add --context cloudscribe.Logging.EFCore.PostgreSql.LoggingDbContext  cs-logging-yyyymmdd