/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public abstract class ItemSet : IDataStoreItemSet
    {
        private IDataStoreType list;
        private Query query;

        public event DataStoreItemSetEventHandler ItemSetChanged;
        public event DataStoreItemEventHandler ItemInSetChanged;
        public event DataStoreItemSetEventHandler SaveStateChanged;

        public bool IsSaving
        {
            get
            {
                return this.IsSaving;
            }
        }

        public bool NeedsSaving
        {
            get
            {
                return false;
            }
        }
        public IItem FirstItem
        {
            get
            {
                return this.Items[0];
            }
        }
        public abstract List<IItem> Items { get; }

        public Query Query
        {
            get
            {
                return this.query;
            }
        }

        public IDataStoreType Type
        {
            get
            {
                return this.list;
            }
        }

        public ItemSet(IDataStoreType list, Query query)
        {
            this.list = list;
            this.query = query;
        }

        public static object GetDataObject(IDataStoreItemSet itemSet, IItem item)
        {
            Dictionary<String, object> newObject = new Dictionary<string, object>();

            foreach (IDataStoreField field in itemSet.Type.Fields)
            {
                object val = item.GetValue(field.Name);

                newObject[field.Name] = val;
            }

            newObject["LocalOnlyUniqueId"] = item.LocalOnlyUniqueId;

            return newObject;
        }

        public static void SetDataObject(IDataStoreItemSet itemSet, IItem item, object data)
        {
            foreach (IDataStoreField field in itemSet.Type.Fields)
            {
                object newValue = null;

                Script.Literal("if ({1}[{2}] != null) {{{0}={1}[{2}];}}", newValue, data, field.Name);

                if (newValue != null)
                {
                    item.SetValue(field.Name, newValue);
                }
            }
        }


        public abstract IItem GetItemById(String id);
        public abstract IItem GetItemByLocalOnlyUniqueId(String id);
        public abstract void BeginRetrieve(AsyncCallback callback, object state);
        public abstract ICollection<IItem> EndRetrieve(IAsyncResult result);

        public void Clear()
        {
            List<IItem> itemsTemp = new List<IItem>();

            foreach (IItem item in this.Items)
            {
                itemsTemp.Add(item);
            }

            foreach (IItem item in itemsTemp)
            {
                this.Remove(item);
            }
        }

        public void Remove(IItem item)
        {
            if (this.Items.Contains(item))
            {
                this.Items.Remove(item);
            }
        }

        public virtual void Add(IItem item)
        {
            if (!this.Items.Contains(item))
            {
                this.Items.Add(item);
            }
        }
    }
}
