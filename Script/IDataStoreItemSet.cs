/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public interface IDataStoreItemSet
    {
        IItem FirstItem
        {
            get;
        }

        List<IItem> Items
        {
            get;
        }

        IDataStoreType Type
        {
            get;
        }

        Query Query
        {
            get;
        }

        event DataStoreItemSetEventHandler ItemSetChanged;
        event DataStoreItemEventHandler ItemInSetChanged;

        IItem GetItemById(String id);
        IItem GetItemByLocalOnlyUniqueId(String id);

        void BeginRetrieve(AsyncCallback callback, object state);

        void Add(IItem item);

        void Remove(IItem item);

        void Clear();
    }
}
