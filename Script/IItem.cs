/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

namespace BL.Data
{
    public interface IItem
    {
        event DataStoreItemEventHandler ItemChanged;

        string Id { get; }
        string LocalOnlyUniqueId { get;  }

        IDataStoreType Type { get; set; }
        object GetValue(String name);
        void SetValue(String name, object value);
        
        String GetStringValue(String name);
        void SetStringValue(String name, String value);
        
        Nullable<Int32> GetInt32Value(String name);
        void SetInt32Value(String name, Nullable<Int32> value);

        Nullable<bool> GetBooleanValue(String name);
        void SetBooleanValue(String name, Nullable<bool> value);

        Date GetDateValue(String name);
        void SetDateValue(String name, Date value);
    }
}
