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
        private String titleFieldId;
        private String keyFieldId;

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


        public IDataStore store;

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
