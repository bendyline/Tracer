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
    public interface IDataStoreType
    {
        String KeyFieldId { get; }
        String TitleFieldId { get; }

        ICollection<IDataStoreField> Fields { get; }
        ICollection<IItem> AllLocalItems{ get; }
        String Name { get; }

        String Title { get; set;  }
        String Description { get; set; }

        IDataStore Store { get; }
        IDataStoreField AssumeField(String name, FieldType fieldType);
        IDataStoreField GetField(String fieldName);

        IItem CreateItem();

        IDataStoreItemSet EnsureAllItemsSet();
        IDataStoreItemSet EnsureItemSet(Query query);
       
        void BeginUpdate(AsyncCallback callback, object asyncState);
    }
}
