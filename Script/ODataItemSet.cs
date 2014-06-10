/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Net;
using System.Serialization;

namespace BL.Data
{
    public class ODataItemSet : IDataStoreItemSet
    {
        private List<IItem> items;
        private Dictionary<String, IItem> itemsById;

        private ODataEntityType entityType;
        private Query query;

        private Operation retrieveOperation;
        private bool isRetrieved = false;

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
            this.items = new List<IItem>();
        }

        public IItem Create()
        {
            ODataEntity item = new ODataEntity(this.entityType);
          
            ODataStore store = (ODataStore)this.Type.Store;

            store.NewItemsCreated++;

            item.SetId((-store.NewItemsCreated).ToString());

            item.SetStatus(ItemStatus.NewItem);

            this.items.Add(item);

            this.itemsById[item.Id] = item;

            return item;
        }

        public IItem GetItemById(String id)
        {
            return this.itemsById[id];
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
                            filter.Append(" and ");
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

            this.itemsById[item.Id] = item;
            this.Items.Add(item);


            if (this.ItemSetChanged != null)
            {
                DataStoreItemSetEventArgs dsiea = new DataStoreItemSetEventArgs(this);

                dsiea.AddedItems.Add(item);

                this.ItemSetChanged(this, dsiea);
            }
        }

        public void BeginRetrieve(AsyncCallback callback, object state)
        {
            if (this.isRetrieved)
            {
                if (callback != null)
                {
                    CallbackResult cr = new CallbackResult();
                    cr.Data = this;
                    cr.AsyncState = state;
                    cr.CompletedSynchronously = true;
                    cr.IsCompleted = true;

                    callback(cr);
                }

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
                        var newe = new BL.Data.ODataEntity({2});
  
                        for (var j=0; j<fieldarr.length; j++)
                        {{
                            var fi = fieldarr[j];
                            var fiName = fi.get_name();

                            var val = oc[fi.get_name()];

                            if (val != null)
                            {{
                                newe.setValue(fiName, val);
                            }}
                        }}
newe.setStatus(2);
this.add(newe);
                    }}
                    
", results, this.entityType.Fields, this.entityType);

            this.isRetrieved = true;
        }


        public void Remove(IItem item)
        {
            if (this.Items.Contains(item))
            {
                this.itemsById.Remove(item.Id);
                this.Items.Remove(item);
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
