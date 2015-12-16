/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public class LessThanClause : ComparisonClause
    {
        public override bool ItemMatches(IItem item)
        {
            IDataStoreField field = item.Type.GetField(this.FieldName);

            if (field.Type == FieldType.BigInteger || field.Type == FieldType.Integer)
            {
                return item.GetInt32Value(this.FieldName) > (Int32)this.Value;
            }

            return (item.GetStringValue(this.FieldName) == this.Value.ToString());
        }

        public override string ToString()
        {
            return "{" + this.FieldName + "<" + this.GetJsonStringValue() + "}";
        }
    }
}
