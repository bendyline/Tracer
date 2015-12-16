/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bendyline.Base
{
    public interface IDataStoreList
    {
        ICollection<IDataStoreField> Fields { get; }
        ICollection<IDataStoreItem> AllLocalItems{ get; }
        String Name { get; }

        IDataStoreField AssumeField(String name, DataStoreFieldType fieldType);

        IDataStoreItem CreateItem();

        IDataStoreItemSet EnsureAllItemsSet();
        IDataStoreItemSet EnsureItemSet(DataStoreQuery query);
       
        void BeginUpdate(AsyncCallback callback, object asyncState);
    }
}
