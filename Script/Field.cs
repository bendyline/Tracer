/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public class Field : IDataStoreField
    {
        private String name;
        private FieldType fieldType;

        public String Name
        {
            get
            {
                return this.name;
            }
        }

        public FieldType Type
        {
            get
            {
                return this.fieldType;
            }
        } 

        public Field(String name, FieldType fieldType)
        {
            this.name = name;
            this.fieldType = fieldType;
        }
    }
}
