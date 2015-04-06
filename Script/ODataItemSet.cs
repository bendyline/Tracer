/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Html;
using System.Net;
using System.Serialization;

namespace BL.Data
{
    public class ODataItemSet : IDataStoreItemSet
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
        public event DataStoreItemEventHandler ItemInSetChanged;

        public event DataStoreItemSetEventHandler SaveStateChanged;

        private ElementEventListener shuttingDownHandler;
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

        public bool AutoSave
        {
            get
            {
                return autoSave;
            }

            set
            {
                if (this.autoSave == value)
                {
                    return;
                }

                this.autoSave = value;

                if (this.autoSave)
                {
                    Window.AddEventListener("beforeunload", this.shuttingDownHandler, true);

                    this.ConsiderAutoSave();
                }
                else
                {
                    Window.RemoveEventListener("beforeunload", this.shuttingDownHandler);
                }
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
        }

        public IDataStoreType Type
        {
            get
            {
                return this.entityType;
            }
        }

        public ODataItemSet(IDataStoreType list, Query query)
        {
            this.entityType = (ODataEntityType)list;
            this.query = query;
            this.itemsById = new Dictionary<string, IItem>();
            this.itemsByLocalOnlyUniqueId = new Dictionary<string, IItem>();
            this.items = new List<IItem>();
            this.shuttingDownHandler = this.WindowShuttingDown;

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
;

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
            return ItemSet.GetSortedItemList(this.Items, sort, fieldName);
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
            ((ODataEntity)item).Saving += ODataItemSet_Saving;
        }

        private void ODataItemSet_Saving(object sender, EventArgs e)
        {
            this.FireSaveStateChanged();
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

        private String GetODataQueryString()
        {
            StringBuilder filter = new StringBuilder();
            bool wroteData = false;
            foreach (Clause clause in this.query.Clauses)
            {
                if (clause is ComparisonClause)
                {
                    String fieldName = ((ComparisonClause)clause).FieldName;

                    IDataStoreField field = (IDataStoreField)this.Type.GetField(fieldName);

                    if (field != null)
                    {
                        if (wroteData)
                        {
                            if (this.query.Joiner == GroupJoiner.And)
                            {
                                filter.Append(" and ");
                            }
                            else
                            {
                                filter.Append(" or ");
                            }
                        }

                        wroteData = true;

                        String joiner = "";

                        if (clause is GreaterThanClause)
                        {
                            joiner = "gt";
                        }
                        else if (clause is LessThanClause)
                        {
                            joiner = "lt";
                        }
                        else
                        {
                            joiner = "eq";
                        }

                        if (field.Type == FieldType.Integer || field.Type == FieldType.BigInteger || field.Type == FieldType.BigNumber)
                        {
                            filter.Append(fieldName + " " + joiner + " " + ((EqualsClause)clause).Value + "");
                        }
                        else
                        {
                            filter.Append(fieldName + " " + joiner + " \'" + ((EqualsClause)clause).Value + "\'");
                        }

                    }
                }
            }

            String filterStr = "$filter=" + filter.ToString();

            return filterStr;
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

        private void item_ItemChanged(object sender, DataStoreItemEventArgs e)
        {
            if (this.ItemInSetChanged != null)
            {
                this.ItemInSetChanged(this, e);
            }

            this.ConsiderAutoSave();
        }

        private void ConsiderAutoSave()
        {
            if (this.autoSave == false)
            {
                return;
            }

            // queue up autosaves
            if (!this.autoSavePending)
            {
                this.autoSavePending = true;

                this.FireSaveStateChanged();

                Window.SetTimeout(this.AutoSaveUpdate, 3000);
            }
        }

        private void FireSaveStateChanged()
        {
            if (this.SaveStateChanged != null)
            {
                DataStoreItemSetEventArgs dsiea = new DataStoreItemSetEventArgs(this);

                this.SaveStateChanged(this, dsiea);
            }
        }

        private void WindowShuttingDown(ElementEvent e)
        {
            if (this.autoSavePending)
            {
                this.AutoSaveUpdate();
            }
        }

        private void AutoSaveUpdate()
        {
            this.autoSavePending = false;

            foreach (IItem item in this.Items)
            {
                ODataEntity ode = (ODataEntity)item;

                if (!ode.Disconnected && ode.IsValid && ode.LocalStatus != ItemLocalStatus.Unchanged)
                {
                    ode.Save(this.SaveComplete, null);
                    this.FireSaveStateChanged();
                }
            }

            this.FireSaveStateChanged();
        }

        private void SaveComplete(IAsyncResult result)
        {
            this.FireSaveStateChanged();
        }

        public void BeginRetrieve(AsyncCallback callback, object state)
        {
            if (this.isRetrieved)
            {
                CallbackResult.NotifySynchronousSuccess(callback, state, this);

                return;
            }

            String queryString = GetODataQueryString();

            String endpoint = ((ODataEntityType)this.Type).Url  + "?" + queryString;

            bool isNew = false;

            if (this.retrieveOperation == null)
            {
                this.retrieveOperation = new Operation();
                isNew = true;
            }

            this.retrieveOperation.AddCallback(callback, state);

            if (isNew)
            {
                XmlHttpRequest xhr = new XmlHttpRequest();
                xhr.Open("GET", endpoint);
                
                xhr.SetRequestHeader("Accept", "application/json;odata=minimalmetadata");
                xhr.SetRequestHeader("Content-Type", "application/json");

                xhr.OnReadyStateChange = new Action(this.EndRetrieve);
                this.retrieveOperation.Tag = xhr;

                xhr.Send();
            }
        }

        public void SetFromData(object results)
        {
            if (results == null)
            {
                return;
            }

            Script.Literal(@"
var fieldarr = {1};
for (var i=0; i<{0}.length; i++)
{{
    var oc = {0}[i];

    if (oc != null)
    {{
        var newe = new BL.Data.ODataEntity({2});
  
        for (var j=0; j<fieldarr.length; j++)
        {{
            var fi = fieldarr[j];
            var fiName = fi.get_name();
            var fiNameL = fiName.toLowerCase();

            var val = oc[fi.get_name()];

            if (val != null)
            {{
                newe.setValue(fiName, val);
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

        public void EndRetrieve()
        {
            Operation o = this.retrieveOperation;
            
            XmlHttpRequest xhr = (XmlHttpRequest)o.Tag;

            if (o != null && xhr.ReadyState == ReadyState.Loaded) 
            {
                this.retrieveOperation = null;

                String responseContent = xhr.ResponseText;

                object results = Json.Parse(responseContent);

                Script.Literal(@"{0}={0}.value", results);

                this.SetFromData(results);

                o.CompleteAsAsyncDone(this);
            }
        }
    }
}
