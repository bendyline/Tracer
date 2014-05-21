﻿/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public abstract class Item : IItem
    {
        private Dictionary<String, object> data;
        private ItemStatus status = ItemStatus.Unchanged;
        
        public event DataStoreItemEventHandler ItemChanged;

        public abstract String Id { get;  }

        private IDataStoreType parentType;

        public IDataStoreType Type
        {
            get { return this.parentType; }
            set { this.parentType = value; }
        }

        public ItemStatus Status
        {
            get
            {
                return this.status;
            }

        }

        protected Dictionary<String, object> Data
        {
            get
            {
                return this.data;
            }
        }

        public Item(IDataStoreType list)
        {
            this.parentType = list;
            this.data = new Dictionary<string, object>();
        }

        public void SetData(object data)
        {
            this.data = (Dictionary<String, object>)data;
        }

        public Int32 GetInt32Value(String name)
        {
            if (!this.data.ContainsKey(name))
            {
                return 0;
            }

            return Int32.Parse((String)this.data[name]);
        }

        public Int64 GetInt64Value(String name)
        {
            if (!this.data.ContainsKey(name))
            {
                return 0;
            }

            return (Int64)Number.ParseInt((String)this.data[name]);
        }

        public object GetValue(String name)
        {
            if (!this.data.ContainsKey(name))
            {
                return null;
            }

            return this.data[name];
        }

        public void SetStatus(ItemStatus status)
        {
            this.status = status;
        }

        public String GetValueAsString(String name)
        {
            object val = GetValue(name);

            if (val == null)
            {
                return null;
            }

            return val.ToString();
        }

        public void SetValue(String name, object value)
        {
            if (this.data[name] == value)
            {
                return;
            }

            if (this.status == ItemStatus.Unchanged)
            {
                this.status = ItemStatus.Update;
            }

            this.data[name] = value;

            if (this.ItemChanged != null)
            {
                DataStoreItemEventArgs dsiea = new DataStoreItemEventArgs(this);

                this.ItemChanged(this, dsiea);
            }
        }

        public String GetStringValue(String name)
        {
            if (!this.data.ContainsKey(name))
            {
                return null;
            }

            object val = this.data[name];

            if (val == null)
            {
                return null;
            }

            return val.ToString();
        }

        public void SetStringValue(String name, String value)
        {
            if (this.data.ContainsKey(name) && this.data[name] == value)
            {
                return;
            }

            this.data[name] = value;

            if (this.status == ItemStatus.Unchanged)
            {
                this.status = ItemStatus.Update;
            }



            if (this.ItemChanged != null)
            {
                DataStoreItemEventArgs dsiea = new DataStoreItemEventArgs(this);

                this.ItemChanged(this, dsiea);
            }
        }

        public bool SetFromValues(Dictionary<String, String> values)
        {
            bool hasChanged = false;

            foreach (KeyValuePair<String, String> kvp in values)
            {
                if (!this.data.ContainsKey(kvp.Key) || this.data[kvp.Key] != kvp.Value)
                {
                    hasChanged = true;
                    this.data[kvp.Key] = kvp.Value;
                }
            }

            if (hasChanged && this.ItemChanged != null)
            {
                DataStoreItemEventArgs dsiea = new DataStoreItemEventArgs(this);

                this.ItemChanged(this, dsiea);
            }

            return hasChanged;
        }
    }
}
