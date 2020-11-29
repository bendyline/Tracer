/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;


#if NET
using Bendyline.Base;
using Bendyline.Base.ScriptCompatibility;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public interface IItem
    {
        event DataStoreItemChangedEventHandler ItemChanged;
        event DataStoreItemEventHandler ItemDeleted;

        Date CreatedDateTime { get; }
        
        Date ModifiedDateTime { get; }

        String CreatedByUserId { get; }

        String ModifiedUserId { get; }


        string Id { get; }
        string LocalOnlyUniqueId { get;  }
        string Title { get; }

        StandardStatus Status { get;  }

        IDataStoreType Type { get; set; }

        void DeleteItem();

        object GetValue(String name);
        void SetValue(String name, object value);
        void LocalSetValue(String name, object value);
        
        String GetStringValue(String name);
        void SetStringValue(String name, String value);
        
        Nullable<Int32> GetInt32Value(String name);

        void SetInt32Value(String name, Nullable<Int32> value);

        Nullable<Int64> GetInt64Value(String name);
        void SetInt64Value(String name, Nullable<Int64> value);

        Nullable<bool> GetBooleanValue(String name);
        void SetBooleanValue(String name, Nullable<bool> value);

        Date GetDateValue(String name);
        void SetDateValue(String name, Date value);

        void Save(AsyncCallback callback, object state);

        int CompareTo(IItem item, ItemSetSort sort, String fieldName);
    }
}
