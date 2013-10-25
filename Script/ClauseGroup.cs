/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public class ClauseGroup
    {
        private List<Clause> clauses;
        private List<ClauseGroup> groups;

        public List<ClauseGroup> Groups
        {
            get
            {
                return this.groups;
            }
        }

        public List<Clause> Clauses
        {
            get
            {
                return this.clauses;
            }
        }

        public ClauseGroup()
        {
            this.clauses = new List<Clause>();
            this.groups = new List<ClauseGroup>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            foreach (Clause dsc in this.clauses)
            {

            }

            foreach (ClauseGroup dsg in this.groups)
            {
                sb.Append(dsg.ToString());
            }

            sb.Append("]");
            return sb.ToString();
        }
    }
}
