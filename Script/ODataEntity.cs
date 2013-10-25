/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public class ODataEntity : IDataStoreType
    {
        private List<IDataStoreField> fields;
        private Dictionary<String, Field> fieldsByName;
        private List<IItem> items;
        private String name;
        private Dictionary<String, ODataItemSet> itemsByQuery;
        private ODataItemSet allItemsSet;
        private ODataStore store;

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

        public String Name 
        {
            get
            {
                return this.name;
            }
        }

        public ODataEntity(ODataStore store, String name)
        {
            this.name = name;
            this.store = store;
            this.fieldsByName = new Dictionary<string, Field>();
            this.itemsByQuery = new Dictionary<string, ODataItemSet>();
            this.fields = new List<IDataStoreField>();
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
            return null;
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

        public IDataStoreItemSet EnsureItemSet(Query query)
        {
            String queryString = query.ToString().ToLowerCase();

            ODataItemSet odis = this.itemsByQuery[queryString];

            if (odis == null)
            {
                odis =new ODataItemSet(this, query);

                this.itemsByQuery[queryString] = odis;
            }

            return null;
        }

        public void BeginUpdate(AsyncCallback callback, object asyncState)
        {
            ;
        }

    }
}
