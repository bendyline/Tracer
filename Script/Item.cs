/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Serialization;

namespace BL.Data
{
    public class Item : IItem
    {
        private Dictionary<String, object> data;
        private ItemLocalStatus localStatus = ItemLocalStatus.Unchanged;
        private String localOnlyUniqueId;
        private StandardStatus status = StandardStatus.Normal;

        private Date createdDateTime;
        private Date modifiedDateTime;
        private String modifiedUserId;
        private String createdUserId;
        
        public event DataStoreItemEventHandler ItemChanged;
        public event DataStoreItemEventHandler ItemDeleted;


        public Date CreatedDateTime 
        { 
            get
            {
                return this.createdDateTime;
            }
        }

        public Date ModifiedDateTime 
        { 
            get
            {
                return this.modifiedDateTime;
            }
        }

        public String CreatedByUserId 
        { 
            get
            {
                return this.createdUserId;
            }
        }

        public String ModifiedUserId 
        { 
            get
            {
                return this.modifiedUserId;
            }
        }


        public StandardStatus Status
        {
            get
            {
                return this.status;
            }
        }

        public object Data
        {
            get
            {
                object result = null;

                Script.Literal("{0}={1}", result, this.data);

                return result;
            }

            set
            {
                Script.Literal("{0}={1}", this.data, value);
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

        public virtual String Id 
        { 
            get
            {
                return (String)this.data["Id"];
            }
        }

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

        public Item(IDataStoreType itemType)
        {
            this.parentType = itemType;

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

        public void SetCreatedDateTime(Date dateTime)
        {
            this.createdDateTime = dateTime;
        }

        public void SetModifiedDateTime(Date dateTime)
        {
            this.modifiedDateTime = dateTime;
        }

        public void SetCreatedUserId(String userId)
        {
            this.createdUserId = userId;
        }

        public void SetModifiedUserId(String userId)
        {
            this.modifiedUserId = userId;
        }

        public int CompareTo(IItem item, ItemSetSort sort, String fieldName)
        {
            return CompareItems(this, item, sort, fieldName);
        }

        public static int CompareItems(IItem itemA, IItem itemB, ItemSetSort sort, String fieldName)
        {
            if (sort == ItemSetSort.None)
            {
                return 0;
            }

            if (sort == ItemSetSort.CreatedDateAscending)
            {
                return CompareItemsByModifiedDateAscending(itemA, itemB);
            }
            else if (sort == ItemSetSort.CreatedDateDescending)
            {
                return CompareItemsByCreatedDateDescending(itemA, itemB);
            }
            else if (sort == ItemSetSort.ModifiedDateDescending)
            {
                return CompareItemsByModifiedDateDescending(itemA, itemB);
            }
            else if (sort == ItemSetSort.ModifiedDateAscending)
            {
                return CompareItemsByModifiedDateAscending(itemA, itemB);
            }
            else if (sort == ItemSetSort.FieldAscending)
            {
                return CompareItemsByFieldAscending(itemA, itemB, fieldName);
            }
            else if (sort == ItemSetSort.FieldDescending)
            {
                return CompareItemsByFieldDescending(itemA, itemB, fieldName);
            }

            return 0;
        }

        public static object GetDataObject(IDataStoreItemSet itemSet, IItem item)
        {
            return ItemSet.GetDataObject(itemSet, item);
        }

        public static int CompareItemsByFieldDescending(IItem itemA, IItem itemB, String fieldName)
        {
            return -CompareItemsByFieldAscending(itemA, itemB, fieldName);
        }

        public static int CompareItemsByFieldAscending(IItem itemA, IItem itemB, String fieldName)
        {
            object valA = itemA.GetValue(fieldName);
            object valB = itemB.GetValue(fieldName);

            if (valA == null && valB == null)
            {
                return 0;
            }

            if (valA == null && valB != null)
            {
                return 1;
            }

            if (valA != null && valB == null)
            {
                return -1;
            }

            if (valA is String && valB is String)
            {
                return ((String)valA).CompareTo((String)valB);
            }

            if (valA is Date && valB is Date)
            {
                return ((Date)valA).GetTime() - ((Date)valB).GetTime();
            }

            if (valA is Int32 && valB is Int32)
            {
                return ((Int32)valA) - ((Int32)valB);
            }

            if (valA is Int64 && valB is Int64)
            {
                Int64 intA = (Int64)valA;
                Int64 intB = (Int64)valB;

                if (intA == intB)
                {
                    return 0;
                }
                else if (intA > intB)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            
            if (valA is double && valB is double)
            {
                double doubleA = (double)valA;
                double doubleB = (double)valB;

                if (doubleA == doubleB)
                {
                    return 0;
                }
                else if (doubleA > doubleB)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }

            if (valA is bool && valB is bool )
            {
                bool boolA = (bool)valA;
                bool boolB = (bool)valB;

                if (boolA == boolB)
                {
                    return 0;
                }
                else if (boolA)
                {
                    return -1;
                }

                return 1;
            }

            return 0;
        }


        public static int CompareItemsByModifiedDateAscending(IItem itemA, IItem itemB)
        {
            if (itemA.ModifiedDateTime == null && itemB.ModifiedDateTime == null)
            {
                return 0;
            }

            if (itemA.ModifiedDateTime == null && itemB.ModifiedDateTime != null)
            {
                return 1;
            }

            if (itemA.ModifiedDateTime != null && itemB.ModifiedDateTime == null)
            {
                return -1;
            }

            return itemA.ModifiedDateTime.GetTime() - itemB.ModifiedDateTime.GetTime();
        }

        public static int CompareItemsByModifiedDateDescending(IItem itemA, IItem itemB)
        {
            if (itemA.ModifiedDateTime == null && itemB.ModifiedDateTime == null)
            {
                return 0;
            }

            if (itemA.ModifiedDateTime == null && itemB.ModifiedDateTime != null)
            {
                return -1;
            }

            if (itemA.ModifiedDateTime != null && itemB.ModifiedDateTime == null)
            {
                return 1;
            }

            return itemB.ModifiedDateTime.GetTime() - itemA.ModifiedDateTime.GetTime();
        }


        public static int CompareItemsByCreatedDateAscending(IItem itemA, IItem itemB)
        {
            if (itemA.CreatedDateTime == null && itemB.CreatedDateTime == null)
            {
                return 0;
            }

            if (itemA.CreatedDateTime == null && itemB.CreatedDateTime != null)
            {
                return 1;
            }

            if (itemA.CreatedDateTime != null && itemB.CreatedDateTime == null)
            {
                return -1;
            }

            return itemA.CreatedDateTime.GetTime() - itemB.CreatedDateTime.GetTime();
        }

        public static int CompareItemsByCreatedDateDescending(IItem itemA, IItem itemB)
        {
            if (itemA.CreatedDateTime == null && itemB.CreatedDateTime == null)
            {
                return 0;
            }

            if (itemA.CreatedDateTime == null && itemB.CreatedDateTime != null)
            {
                return -1;
            }

            if (itemA.CreatedDateTime != null && itemB.CreatedDateTime == null)
            {
                return 1;
            }

            return itemB.CreatedDateTime.GetTime() - itemA.CreatedDateTime.GetTime();
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

            // attempt to canonicalize storage of data values.
            IDataStoreField fi = this.Type.GetField(name);

            if (fi != null)
            {
                if ((fi.Type == FieldType.Integer || fi.Type == FieldType.BigInteger) && value is String)
                {
                    value = Int32.Parse((String)value);
                }
                else if ((fi.Type == FieldType.DateTime) && value is String)
                {
                    Date dt;

                    int dateToken = ((String)value).IndexOf("/Date(");
                    int dateTokenEnd = ((String)value).LastIndexOf(")");

                    if (dateToken >= 0 && dateTokenEnd > dateToken)
                    {
                        dt = new Date(Int32.Parse(((String)value).Substring(dateToken + 6, dateTokenEnd)));
                    }
                    else
                    {
                        dt = Date.Parse((String)value);
                    }

                    value = Utilities.ConvertToUtc(dt);
                }
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

            if (val is Date)
            {
                return JsonUtilities.EncodeDate((Date)val);
            }

            return val.ToString();
        }

        public void SetStringValue(String name, String value)
        {
            if (this.data.ContainsKey(name) && (String)this.data[name] == value)
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
                if (!this.data.ContainsKey(kvp.Key) || (String)this.data[kvp.Key] != kvp.Value)
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
