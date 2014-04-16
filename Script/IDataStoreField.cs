/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

namespace BL.Data
{
    public interface IDataStoreField
    {
        FieldChoiceCollection Choices { get;  }
        String DisplayName { get; }
        String Name { get; }
        FieldType Type { get; }
    }
}
