/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public interface IDataStore
    {
        ICollection<IDataStoreType> Types { get; }
        String Name { get; }
        String Location { get; set; }
        bool RequiresAuthentication { get; set; }

        IDataStoreType Type(String listName);
    }
}
