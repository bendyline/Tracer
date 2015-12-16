/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bendyline.Data
{
    public class DataStoreClauseGroup
    {
        private List<DataStoreClause> clauses;
        private List<DataStoreClauseGroup> groups;

        public IList<DataStoreClauseGroup> Groups
        {
            get
            {
                return this.groups;
            }
        }

        public IList<DataStoreClause> Clauses
        {
            get
            {
                return this.clauses;
            }
        }

        public DataStoreClauseGroup()
        {
            this.clauses = new List<DataStoreClause>();
            this.groups = new List<DataStoreClauseGroup>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            foreach (DataStoreClause dsc in this.clauses)
            {

            }

            foreach (DataStoreClauseGroup dsg in this.groups)
            {
                sb.Append(dsg.ToString());
            }

            sb.Append("]");
            return sb.ToString();
        }
    }
}
