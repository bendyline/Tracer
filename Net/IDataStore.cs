/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace Bendyline.Base
{
    public interface IDataStore
    {
        ICollection<IDataStoreList> Lists { get; }
        String Name { get; }
        String Location { get; set; }
        bool RequiresAuthentication { get; set; }

        IDataStoreList AssumeList(String listName);
    }
}
