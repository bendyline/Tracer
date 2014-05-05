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

        private ODataEntityType entity;
        private Query query;

        private Operation retrieveOperation;

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
                return this.entity;
            }
        }

        public ODataItemSet(IDataStoreType list, Query query)
        {
            this.entity = (ODataEntityType)list;
            this.query = query;
            this.itemsById = new Dictionary<string, IItem>();
            this.items = new List<IItem>();
        }

        public IItem Create()
        {
            ODataEntity item = new ODataEntity(this.entity);
          
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

            foreach (Clause clause in this.query.Clauses)
            {
                if (clause is EqualsClause)
                {
                    String fieldName = ((EqualsClause)clause).FieldName;

                    IDataStoreField field = (IDataStoreField)this.Type.GetField(fieldName);

                    if (field != null)
                    {
                        if (field.Type == FieldType.Integer || field.Type == FieldType.BigInteger)
                        {
                            filter.Append(fieldName + " eq " + ((EqualsClause)clause).Value + "");
                        }
                        else
                        {
                            filter.Append(fieldName + " eq \'" + ((EqualsClause)clause).Value + "\'");
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
                xhr.SetRequestHeader("Content-Type", "application/json;odata=minimalmetadata");

                xhr.OnReadyStateChange = new Action(this.EndRetrieve);
                this.retrieveOperation.Tag = xhr;

                xhr.Send();
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

                Script.Literal(@"
                    var oarr = {0}.value;
                    var fieldarr = {1};
                    for (var i=0; i<oarr.length; i++)
                    {{
                        var oc = oarr[i];
                        var newe = new BL.Data.ODataEntity();

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

this.add(newe);
                    }}
                    
", results, this.entity.Fields);

                foreach (CallbackState cs in o.CallbackStates)
                {
                    CallbackResult cr = new CallbackResult();

                    cr.Data = this;
                    cr.IsCompleted = true;

                    cs.Callback(cr);
                }
            }
        }

    }
}
