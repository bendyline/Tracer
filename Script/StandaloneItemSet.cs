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

        private IDataStoreType entity;

        private int newItemsCreated = 0;

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

        public event DataStoreItemSetEventHandler ItemSetChanged;

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
            this.items = new List<IItem>();
        }

        public IItem Create()
        {
            StandaloneItem item = new StandaloneItem(this.entity);
          
            this.newItemsCreated++;

            item.SetId((-this.newItemsCreated).ToString());

            item.SetStatus(ItemStatus.NewItem);

            this.items.Add(item);

            this.itemsById[item.Id] = item;

            return item;
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

            this.itemsById[item.Id] = item;
            this.Items.Add(item);
        }

        public IItem GetItemById(String id)
        {
            return this.itemsById[id];
        }

        public void BeginRetrieve(AsyncCallback callback, object state)
        {
           if (callback != null)
           {
               CallbackResult cr = new CallbackResult();

               cr.AsyncState = state;
               cr.Data = this;
               cr.IsCompleted = true;
               cr.CompletedSynchronously = true;

               callback(cr);
           }
        }
        
    }
}
