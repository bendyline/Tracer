/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public enum GroupJoiner
    {
        And = 0,
        Or = 1
    }

    public class ClauseGroup
    {
        private List<Clause> clauses;
        private List<ClauseGroup> groups;
        private GroupJoiner joiner = GroupJoiner.And;

        public GroupJoiner Joiner
        {
            get
            {
                return this.joiner;
            }

            set
            {
                this.joiner = value;
            }
        }

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

        public bool ItemMatches(IItem item)
        {
            foreach (Clause c in this.clauses)
            {
                if (!c.ItemMatches(item))
                {
                    return false;
                }
            }

            foreach (ClauseGroup cg in this.groups)
            {
                if (!cg.ItemMatches(item))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            
            bool isFirst = true;

            foreach (Clause dsc in this.clauses)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }

                sb.Append(dsc.ToString());

                isFirst = false;
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
