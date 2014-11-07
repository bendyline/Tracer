/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Net;
using System.Serialization;

namespace BL.Data
{
    public class StandaloneItemSet : IDataStoreItemSet
    {
        private List<IItem> items;
        private Dictionary<String, IItem> itemsById;
        private Dictionary<String, IItem> itemsByLocalOnlyUniqueId;

        private IDataStoreType entity;

        private int newItemsCreated = 0;

        public event DataStoreItemSetEventHandler ItemSetChanged;
        public event DataStoreItemEventHandler ItemInSetChanged;

        public IItem FirstItem
        {
            get
            {
                return this.items[0];
            }
        }

        public List<IItem> Items 
        { 
            get
            {
                return items;
            }
        }

        public Query Query
        {
            get
            {
                throw new Exception("Not implemented");
            }
        }

        public IDataStoreType Type
        {
            get
            {
                return this.entity;
            }
        }

        public StandaloneItemSet(IDataStoreType list)
        {
            this.entity = (ODataEntityType)list;

            this.itemsById = new Dictionary<string, IItem>();
            this.itemsByLocalOnlyUniqueId = new Dictionary<string, IItem>();
            this.items = new List<IItem>();
        }

        public IItem Create()
        {
            StandaloneItem item = new StandaloneItem(this.entity);
          
            this.newItemsCreated++;

            item.SetId((-this.newItemsCreated).ToString());

            item.SetLocalStatus(ItemLocalStatus.NewItem);

            this.Add(item);

            return item;
        }

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

        public virtual void Add(IItem item)
        {
            IItem existingItem = this.itemsById[item.Id];

            if (existingItem != null && existingItem != item)
            {
                throw new Exception("Adding two items representing the same ID.");
            }

            if (existingItem != null && existingItem == item)
            {
                return;
            }

            this.itemsByLocalOnlyUniqueId[item.LocalOnlyUniqueId] = item;
            this.itemsById[item.Id] = item;
            this.Items.Add(item);
        }

        public void Remove(IItem item)
        {
            if (this.Items.Contains(item))
            {
                this.itemsByLocalOnlyUniqueId.Remove(item.LocalOnlyUniqueId);
                this.itemsById.Remove(item.Id);
                this.Items.Remove(item);
            }
        }

        public IItem GetItemByLocalOnlyUniqueId(String id)
        {
            return this.itemsByLocalOnlyUniqueId[id];
        }

        public IItem GetItemById(String id)
        {
            return this.itemsById[id];
        }

        public void BeginRetrieve(AsyncCallback callback, object state)
        {
            CallbackResult.NotifySynchronousSuccess(callback, state, this);
        }
        
    }
}
