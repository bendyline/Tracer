/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public class ItemType : IDataStoreType
    {
        private String name;
        private List<IDataStoreField> fields;
        private Dictionary<String, IDataStoreField> fieldsByName;
        private List<IItem> allItems;
        private Dictionary<String, ItemSet> itemsByQuery;
        private ItemSet allItemsSet;
        private String titleFieldId;
        private String keyFieldId;
        private String description;
        private string titleText;

        public IDataStore store;

        public String Title
        {
            get
            {
                return this.titleText;
            }

            set
            {
                this.titleText = value;
            }
        }

        public String Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
            }
        }

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

        public IDataStore Store
        {
            get
            {
                return this.store;
            }

            set
            {
                this.store = value;
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
                return this.allItems;
            }
        }

        public String Name 
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public ItemType()
        {
            this.fields = new List<IDataStoreField>();
            this.fieldsByName = new Dictionary<string, IDataStoreField>();
            this.allItems = new List<IItem>();
            this.itemsByQuery = new Dictionary<string, ItemSet>();
        }

        public IDataStoreField GetField(String name)
        {
            if (!this.fieldsByName.ContainsKey(name))
            {
                return null;
            }

            return this.fieldsByName[name];
        }

        public IDataStoreField AssumeField(String name, FieldType fieldType)
        {
            if (this.fieldsByName.ContainsKey(name))
            {
                return this.fieldsByName[name];
            }

            Field f = new Field(name, fieldType);
            
            this.fieldsByName[name] = f;
            this.fields.Add(f);

            return f;
        }

        public IItem CreateItem()
        {
            ItemSet itemSet = (ItemSet)this.EnsureAllItemsSet();

            return itemSet.Create();
        }

        public IItem CreateDisconnectedItem()
        {
            Item item = new Item(this);

            item.Disconnected = true;
            item.SetCreatedDateTime(Date.Now);
            item.SetModifiedDateTime(item.CreatedDateTime);

            return item;
        }

        public void MoveItemSetToNewQuery(IDataStoreItemSet odis, Query newQuery)
        {
            throw new Exception(String.Empty);
        }

        public void SetDataForQuery(Query query, object data)
        {
            throw new Exception(String.Empty);
        }

        public IDataStoreItemSet EnsureAllItemsSet()
        {
            if (this.allItemsSet != null)
            {
                return this.allItemsSet;
            }

            String queryString = "";

            ItemSet odis = this.itemsByQuery[queryString];

            if (odis == null)
            {
                odis = new ItemSet(this, Query.All);

                this.itemsByQuery[queryString] = odis;
            }

            this.allItemsSet = odis;

            return odis;
        }

        public IDataStoreItemSet EnsureItemSet(Query query)
        {
            String queryString = query.ToString().ToLowerCase();

            ItemSet odis = this.itemsByQuery[queryString];

            if (odis == null)
            {
                odis = new ItemSet(this, query);

                this.itemsByQuery[queryString] = odis;
            }

            return odis;
        }

        public void Save(AsyncCallback callback, object asyncState)
        {

        }
    }
}
