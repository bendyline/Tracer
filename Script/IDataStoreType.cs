/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public interface IDataStoreType
    {
        String KeyFieldId { get; }
        ICollection<IDataStoreField> Fields { get; }
        ICollection<IItem> AllLocalItems{ get; }
        String Name { get; }

        IDataStore Store { get; }
        IDataStoreField AssumeField(String name, FieldType fieldType);
        IDataStoreField GetField(String fieldName);

        IItem CreateItem();

        IDataStoreItemSet EnsureAllItemsSet();
        IDataStoreItemSet EnsureItemSet(Query query);
       
        void BeginUpdate(AsyncCallback callback, object asyncState);
    }
}
