/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public abstract class Item : IItem
    {
        private Dictionary<String, object> data;
        private ItemLocalStatus localStatus = ItemLocalStatus.Unchanged;
        private String localOnlyUniqueId;
        private StandardStatus status = StandardStatus.Normal;
        
        public event DataStoreItemEventHandler ItemChanged;
        public event DataStoreItemEventHandler ItemDeleted;

        public StandardStatus Status
        {
            get
            {
                return this.status;
            }
        }

        public virtual String LocalOnlyUniqueId
        {
            get
            {
                if (this.localOnlyUniqueId == null)
                {
                    this.localOnlyUniqueId = Utilities.GetRandomId();
                }

                return this.localOnlyUniqueId;
            }
        }

        public abstract String Id { get;  }

        private IDataStoreType parentType;

        public IDataStoreType Type
        {
            get { return this.parentType; }
            set { this.parentType = value; }
        }

        public ItemLocalStatus LocalStatus
        {
            get
            {
                return this.localStatus;
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

        public void DeleteItem()
        {
            this.status = StandardStatus.Deleted;
            this.localStatus = ItemLocalStatus.Deleted;

            if (this.ItemDeleted != null)
            {
                DataStoreItemEventArgs dsiea = new DataStoreItemEventArgs(this);

                this.ItemDeleted(this, dsiea);
            }
        }

        public static object GetDataObject(IDataStoreItemSet itemSet, IItem item)
        {
            return ItemSet.GetDataObject(itemSet, item);
        }

        public static void SetDataObject(IDataStoreItemSet itemSet, IItem item, object data)
        {
            ItemSet.SetDataObject(itemSet, item, data);
        }

        public void SetData(object data)
        {
            this.data = (Dictionary<String, object>)data;
        }

        public object GetValue(String name)
        {
            if (!this.data.ContainsKey(name))
            {
                return null;
            }

            return this.data[name];
        }

        public void SetLocalStatus(ItemLocalStatus status)
        {
            this.localStatus = status;
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

            if (this.localStatus == ItemLocalStatus.Unchanged)
            {
                this.localStatus = ItemLocalStatus.Update;
            }

            this.data[name] = value;

            if (this.ItemChanged != null)
            {
                DataStoreItemEventArgs dsiea = new DataStoreItemEventArgs(this);

                this.ItemChanged(this, dsiea);
            }
        }

        public Nullable<Int64> GetInt64Value(String name)
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
            else if (val is Int64)
            {
                return (Int64)val;
            }
            else if (val is String)
            {
                return (Int64)Number.Parse((String)val);
            }

            return null;
        }

        public void SetInt64Value(String name, Nullable<Int64> value)
        {
            if (this.GetInt64Value(name) == value)
            {
                return;
            }

            this.data[name] = value;

            if (this.localStatus == ItemLocalStatus.Unchanged)
            {
                this.localStatus = ItemLocalStatus.Update;
            }

            if (this.ItemChanged != null)
            {
                DataStoreItemEventArgs dsiea = new DataStoreItemEventArgs(this);

                this.ItemChanged(this, dsiea);
            }
        }
        
        public Nullable<Int32> GetInt32Value(String name)
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
            if (val is Int32)
            {
                return (Int32)val;
            }
            else if (val is String)
            {
                return Int32.Parse((String)val);
            }

            return null;
        }

        public void SetInt32Value(String name, Nullable<Int32> value)
        {
            if (this.GetInt32Value(name) == value)
            {
                return;
            }

            this.data[name] = value;

            if (this.localStatus == ItemLocalStatus.Unchanged)
            {
                this.localStatus = ItemLocalStatus.Update;
            }

            if (this.ItemChanged != null)
            {
                DataStoreItemEventArgs dsiea = new DataStoreItemEventArgs(this);

                this.ItemChanged(this, dsiea);
            }
        }
        

        public Date GetDateValue(String name)
        {
            if (!this.data.ContainsKey(name))
            {
                return Date.Empty;
            }

            object val = this.data[name];

            if (val == null)
            {
                return null;
            }

            if (val is Date)
            {
                return (Date)val;
            }
            else if (val is String)
            {
                return Date.Parse((String)val);
            }

            return null;
        }

        public void SetDateValue(String name, Date value)
        {
            if (this.GetDateValue(name) == value)
            {
                return;
            }

            this.data[name] = value;

            if (this.localStatus == ItemLocalStatus.Unchanged)
            {
                this.localStatus = ItemLocalStatus.Update;
            }

            if (this.ItemChanged != null)
            {
                DataStoreItemEventArgs dsiea = new DataStoreItemEventArgs(this);

                this.ItemChanged(this, dsiea);
            }
        }
        
        public Nullable<bool> GetBooleanValue(String name)
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
            else if (val is bool)
            {
                return (bool)val;
            }
            else if (val is String)
            {
                return Boolean.Parse((String)val);
            }

            return null;
        }

        public void SetBooleanValue(String name, Nullable<bool> value)
        {
            if (this.GetBooleanValue(name) == value)
            {
                return;
            }

            this.data[name] = value;

            if (this.localStatus == ItemLocalStatus.Unchanged)
            {
                this.localStatus = ItemLocalStatus.Update;
            }

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

            if (this.localStatus == ItemLocalStatus.Unchanged)
            {
                this.localStatus = ItemLocalStatus.Update;
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
