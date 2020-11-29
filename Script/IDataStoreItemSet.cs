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
    public enum ItemSetRole
    {
        Query = 0,
        AllItems = 1,
        AllLocalItems =2
    }

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

        bool NeedsSaving
        {
            get;
        }


        bool IsSaving
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
            set;
        }

        event DataStoreItemSetEventHandler ItemSetChanged;
        event DataStoreItemChangedEventHandler ItemInSetChanged;
        event DataStoreItemSetEventHandler SaveStateChanged;

        List<IItem> GetSortedItems(ItemSetSort sort, String fieldName);


        IItem GetItemById(String id);
        IItem GetItemByLocalOnlyUniqueId(String id);

        void Retrieve(AsyncCallback callback, object state);

        void Add(IItem item);

        void SetFromData(object results);

        void Remove(IItem item);

        void Clear();
    }
}
