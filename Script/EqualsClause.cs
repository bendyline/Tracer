/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

#if NET

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public class EqualsClause : ComparisonClause
    {

        public override bool ItemMatches(IItem item)
        {
            return (item.GetStringValue(this.FieldName) == this.Value.ToString());
        }

        public override string ToString()
        {
            return "{" + this.FieldName + "=" + this.Value.ToString() + "}";
        }
    }
}
