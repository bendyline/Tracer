/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BL.Data
{
    public abstract class ComparisonClause : Clause
    {
        private String fieldName;
        private object value;

        public String FieldName
        {
            get
            {
                return this.fieldName; 
            }

            set
            {
                this.fieldName = value;
            }
        }

        public object Value
        {
            get
            {
                return this.value;
            }

            set
            {
                Debug.Assert(value != null, "Value must not be null in a comparison.");

                this.value = value;
            }
        }

        protected String GetJsonStringValue()
        {
            if (value is String)
            {
                return String.Format("\"{0}\"", value);
            }

            return value.ToString();
        }
    }
}
