/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public class ODataEntityType : IDataStoreType
    {
        private List<IDataStoreField> fields;
        private Dictionary<String, Field> fieldsByName;
        private Dictionary<String, ODataEntity> itemsById;
        private List<IItem> items;
        private String name;
        private Dictionary<String, ODataItemSet> itemsByQuery;
        private ODataItemSet allItemsSet;
        private ODataStore store;
        private String entitySetName;
        private String keyFieldId;
        private String titleFieldId;

        private Query validCriteria;

        public String TitleFieldId
        {
            get
            {
                return this.titleFieldId;
            }

            set
            {
                this.titleFieldId = value;
            }
        }

        public String KeyFieldId
        {
            get
            {
                return this.keyFieldId;
            }
            set
            {
                this.keyFieldId = value;
            }
        }

        public Query ValidCriteria
        {
            get
            {
                return this.validCriteria;
            }

            set
            {
                this.validCriteria = value;
            }
        }

        public ICollection<IDataStoreField> Fields 
        {
            get
            {
                return this.fields;
            }
        }

        public ICollection<IItem> AllLocalItems 
        {
            get
            {
                return this.items;
            }     
        }

        public IDataStore Store
        {
            get
            {
                return this.store;
            }
        }

        public String Name 
        {
            get
            {
                return this.name;
            }
        }

        public String EntitySetName
        {
            get
            {
                return this.entitySetName;
            }

            set
            {
                this.entitySetName = value;
            }
        }

        public String Url
        {
            get
            {
                return this.store.ServiceUrl + "/" + this.entitySetName; 
            }
        }

        public ODataEntityType(ODataStore store, String name)
        {
            this.name = name;
            this.store = store;
            this.fieldsByName = new Dictionary<string, Field>();
            this.itemsByQuery = new Dictionary<string, ODataItemSet>();
            this.fields = new List<IDataStoreField>();
            this.itemsById = new Dictionary<String, ODataEntity>();
            this.items = new List<IItem>();
        }

        public IDataStoreField AssumeField(String name, FieldType fieldType)
        {
            Field f = (Field)this.GetField(name);

            if (f == null)
            {
                f = new Field(name, fieldType);

                this.fieldsByName[name] = f;
                this.fields.Add(f);
            }

            return f;
        }

        public IDataStoreField GetField(String fieldName)
        {
            return this.fieldsByName[fieldName];
        }

        public IItem CreateItem()
        {
            ODataItemSet itemSet = (ODataItemSet)this.EnsureAllItemsSet();
            
            return itemSet.Create();
        }

        public IItem CreateDisconnectedItem()
        {
            ODataEntity item = new ODataEntity(this);

            item.Disconnected = true;
            item.SetCreatedDateTime(Date.Now);
            item.SetModifiedDateTime(item.CreatedDateTime);
;
            return item;
        }

        public void Save()
        {
            foreach (ODataEntity ode in this.allItemsSet.Items)
            {
                ode.Save(null, null);
            }
        }

        public IDataStoreItemSet EnsureAllItemsSet()
        {
            if (this.allItemsSet != null)
            {
                return this.allItemsSet;
            }

            String queryString = "";

            ODataItemSet odis = this.itemsByQuery[queryString];

            if (odis == null)
            {
                odis = new ODataItemSet(this, Query.All);

                this.itemsByQuery[queryString] = odis;
            }


            this.allItemsSet = odis;
            return odis;
        }

        public ODataEntity EnsureItem(String id)
        {
            if (this.itemsById.ContainsKey(id))
            {
                return this.itemsById[id];
            }

            ODataEntity item = new ODataEntity(this);

            item.SetId(id);

            this.itemsById[id] = item;
            this.items.Add(item);

            return item;
        }

        public void UpdateItemInItemSets(ODataEntity entity)
        {
            foreach (String key in this.itemsByQuery.Keys)
            {
                ODataItemSet odis = this.itemsByQuery[key];

                // don't mess with arbitrarily query items, like the MyItems query
                if (String.IsNullOrEmpty(odis.Query.Source))
                {
                    // should this item be in this item set?
                    if (odis.GetItemByLocalOnlyUniqueId(entity.LocalOnlyUniqueId) == null && odis.Query.ItemMatches(entity))
                    {
                        odis.Add(entity);
                    }
                    // or should this item be removed from this set?
                    else if (odis.GetItemByLocalOnlyUniqueId(entity.LocalOnlyUniqueId) != null && !odis.Query.ItemMatches(entity))
                    {
                        odis.Remove(entity);
                    }
                }
            }
        }

        public void MoveItemSetToNewQuery(ODataItemSet odis, Query newQuery)
        {
            String oldQueryString = odis.Query.ToString().ToLowerCase();

            this.itemsByQuery.Remove(oldQueryString);

            odis.Query = newQuery;

            String newQueryString = newQuery.ToString().ToLowerCase();

            this.itemsByQuery[newQueryString] = odis;
        }

        public IDataStoreItemSet EnsureItemSet(Query query)
        {
            String queryString = query.ToString().ToLowerCase();

            ODataItemSet odis = this.itemsByQuery[queryString];

            if (odis == null)
            {
                odis = new ODataItemSet(this, query);

                this.itemsByQuery[queryString] = odis;
            }

            return odis;
        }

        public void SetDataForQuery(Query query, object data)
        {
            ODataItemSet dataSet = (ODataItemSet)EnsureItemSet(query);

            dataSet.SetFromData(data);
        }

        public void BeginUpdate(AsyncCallback callback, object asyncState)
        {
            ;
        }

    }
}
