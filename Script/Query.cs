/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public class Query : ClauseGroup
    {
        private int rowLimit = 1000;
        private static Query allQuery = new Query();

        public int RowLimit
        {
            get
            {
                return this.rowLimit;
            }

            set
            {
                this.rowLimit = value;
            }
        }

        public static Query All
        {
            get
            {
                return allQuery;
            }
        }

    }
}
