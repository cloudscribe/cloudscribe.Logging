// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2014-12-29
//	Last Modified:              2016-01-05
// 

using cloudscribe.Core.Models.Setup;
using System;

namespace cloudscribe.Logging.Web
{
    public class CloudscribeLoggingVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe-logging"; } }

        public Guid ApplicationId { get { return new Guid("5edc860e-90c5-4e68-9b89-06c9b77e5184"); } }

        public Version GetCodeVersion()
        {
            // this version needs to be maintained in code to set the highest
            // schema script version script that will be run for cloudscribe-core
            // this allows us to work on the next version script without triggering it
            // to execute until we set this version to match the new script version
            return new Version(1, 0, 0, 0);
        }
    }
}
