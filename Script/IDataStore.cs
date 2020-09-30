/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;


#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP
namespace BL.Data
#endif
{
    public interface IDataStore
    {
        ICollection<IDataStoreType> Types { get; }
        String Name { get; }
        String Location { get; set; }
        bool RequiresAuthentication { get; set; }

        bool IsProvisioned { get; }

        IDataStoreType Type(String listName);

        void EnsureProvisioned(AsyncCallback callback, object state);
    }
}
