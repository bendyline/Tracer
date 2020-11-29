/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;

#if NET
using Bendyline.Base;
using Bendyline.Base.ScriptCompatibility;
using System.Text;

namespace Bendyline.Data
#elif SCRIPTSHARP
using System.Html;

namespace BL.Data
#endif
{
    public class ItemSet : IDataStoreItemSet
    {
        private List<IItem> items;
        private Dictionary<String, IItem> itemsById;
        private Dictionary<String, IItem> itemsByLocalOnlyUniqueId;

        private ODataEntityType entityType;
        private Query query;

        private Operation retrieveOperation;
        private bool isRetrieved = false;
        private bool autoSave = false;
        private bool autoSavePending = false;

        public event DataStoreItemSetEventHandler ItemSetChanged;
        public event DataStoreItemChangedEventHandler ItemInSetChanged;

        public event DataStoreItemSetEventHandler SaveStateChanged;

#if SCRIPTSHARP
        private ElementEventListener shuttingDownHandler;
#endif
        private DataStoreItemEventHandler itemDeletedHandler;

        public bool IsSaving
        {
            get
            {
                foreach (ODataEntity ode in this.items)
                {
                    if (ode.IsSaving)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsRetrieved
        {
            get
            {
                return this.isRetrieved;
            }

            set
            {
                this.isRetrieved = value;
            }
        }

        public bool NeedsSaving
        {
            get
            {
                foreach (ODataEntity ode in this.items)
                {
                    if (ode.LocalStatus == ItemLocalStatus.NewItem || ode.LocalStatus == ItemLocalStatus.Update)
                    {
                        return true;
                    }
                }

                return false;
            }
        }


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
                return this.query;
            }

            set
            {
                this.query = value;
            }
        }

        public IDataStoreType Type
        {
            get
            {
                return this.entityType;
            }
        }

        public ItemSet(IDataStoreType list, Query query)
        {
            this.entityType = (ODataEntityType)list;
            this.query = query;
            this.itemsById = new Dictionary<string, IItem>();
            this.itemsByLocalOnlyUniqueId = new Dictionary<string, IItem>();
            this.items = new List<IItem>();

            this.itemDeletedHandler = this.item_ItemDeleted;
        }

        public IItem Create()
        {
            ODataEntity item = new ODataEntity(this.entityType);
          
            ODataStore store = (ODataStore)this.Type.Store;

            store.NewItemsCreated++;

            item.SetId((-store.NewItemsCreated).ToString());

            item.SetLocalStatus(ItemLocalStatus.NewItem);
            item.SetCreatedDateTime(Date.Now);
            item.SetModifiedDateTime(item.CreatedDateTime);

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

        public List<IItem> GetSortedItems(ItemSetSort sort, String fieldName)
        {
            return ItemSetBase.GetSortedItemList(this.Items, sort, fieldName);
        }

        public IItem GetItemById(String id)
        {
            return this.itemsById[id];
        }

        public IItem GetItemByLocalOnlyUniqueId(String id)
        {
            return this.itemsByLocalOnlyUniqueId[id];
        }

        private void AddManagersToItem(IItem item)
        {
            item.ItemDeleted += this.itemDeletedHandler;
        }

        private void RemoveManagersFromItem(IItem item)
        {
            item.ItemDeleted -= this.itemDeletedHandler;
        }

        private void item_ItemDeleted(object sender, DataStoreItemEventArgs e)
        {
            if (!this.Items.Contains(e.Item))
            {
                return;
            }

            if (this.autoSave)
            {
                ((ODataEntity)e.Item).Save(null, null);
            }

            this.Remove(e.Item);
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

            this.AddManagersToItem(item);

            item.ItemChanged += item_ItemChanged;

            if (this.ItemSetChanged != null)
            {
                DataStoreItemSetEventArgs dsiea = new DataStoreItemSetEventArgs(this);

                dsiea.AddedItems.Add(item);

                this.ItemSetChanged(this, dsiea);
            }
        }

        private void item_ItemChanged(object sender, DataStoreItemChangedEventArgs e)
        {
            if (this.ItemInSetChanged != null)
            {
                this.ItemInSetChanged(this, e);
            }
        }
        public void Retrieve(AsyncCallback callback, object state)
        {
            CallbackResult.NotifySynchronousSuccess(callback, state, null);
        }

        public void InvalidateRetrieval()
        {
            this.isRetrieved = false;
        }
        public void SetFromData(object results)
        {
            if (results == null)
            {
                return;
            }

            Debug.Fail("Not implemented");
#if SCRIPTSHARP
            Script.Literal(@"
var fieldarr = {1};
for (var i=0; i<{0}.length; i++)
{{
    var oc = {0}[i];

    if (oc != null)
    {{
        var idVal = oc[""Id""];


        var newe = null;

        if (idVal == null) 
        {{
            newe = new BL.Data.ODataEntity({2});
        }}
        else
        {{
            newe = {2}.ensureItem(idVal);
        }}

        for (var j=0; j<fieldarr.length; j++)
        {{
            var fi = fieldarr[j];
            var fiName = fi.get_name();
            var fiNameL = fiName.toLowerCase();

            var val = oc[fi.get_name()];

            if (val != null)
            {{
                newe.localSetValue(fiName, val);
            }}

            if (fiNameL == ""createddate"")
            {{
                newe.setCreatedDateTime(newe.getValue(fiName));
            }}
            else if (fiNameL == ""modifieddate"")
            {{
                newe.setModifiedDateTime(newe.getValue(fiName));
            }}
        }}
        newe.setLocalStatus(2);
        this.add(newe);
    }}
}}
                    
", results, this.entityType.Fields, this.entityType);
#else
            throw new NotImplementedException();
#endif

            this.isRetrieved = true;
        }

        public void Remove(IItem item)
        {
            if (this.Items.Contains(item))
            {
                this.RemoveManagersFromItem(item);

                this.itemsByLocalOnlyUniqueId.Remove(item.LocalOnlyUniqueId);
                this.itemsById.Remove(item.Id);
                this.Items.Remove(item);

                if (this.ItemSetChanged != null)
                {
                    DataStoreItemSetEventArgs dsiea = new DataStoreItemSetEventArgs(this);

                    dsiea.RemovedItems.Add(item);

                    this.ItemSetChanged(this, dsiea);
                }
            }
        }
    }
}
