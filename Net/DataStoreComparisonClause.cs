/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bendyline.Data
{
    public abstract class DataStoreComparisonClause : DataStoreClause
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
                this.value = value;
            }

        }

        protected String GetStringValue()
        {
            if (value is String)
            {
                return String.Format("\"{0}\"", value);
            }

            return value.ToString();
        }
    }
}
