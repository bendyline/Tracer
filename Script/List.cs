﻿/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public class List : IDataStoreType
    {
        private String name;
        private List<IDataStoreField> fields;
        private Dictionary<String, IDataStoreField> fieldsByName;
        private List<IItem> allItems;

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
        }

        public List()
        {
            this.fields = new List<IDataStoreField>();
            this.fieldsByName = new Dictionary<string, IDataStoreField>();
            this.allItems = new List<IItem>();
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
            return null;
        }

        public IDataStoreItemSet EnsureAllItemsSet()
        {
            return null;
        }

        public IDataStoreItemSet EnsureItemSet(Query query)
        {
            return null;
        }

        public void BeginUpdate(AsyncCallback callback, object asyncState)
        {

        }
    }
}
