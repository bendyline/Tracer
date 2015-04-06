/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public class ODataStore : IDataStore
    {
        private String location;
        private String name;
        private List<IDataStoreType> entityTypes;
        private Dictionary<String, ODataEntityType> entitiesTypesByName;
        private bool requiresAuthentication;
        private Int32 newItemsCreated;
        private String serviceUrl;
        private String storeNamespace;
        public ICollection<IDataStoreType> Types 
        {
            get
            {
                return this.entityTypes;
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
        
        public String Location 
        {
            get
            {
                return this.location;
            }

            set
            {
                this.location = value;
            }
        }

        public String Namespace
        {
            get
            {
                return this.storeNamespace;
            }
            set
            {
                this.storeNamespace = value;
            }
        }

        public String ServiceUrl
        {
            get
            {
                return this.serviceUrl;
            }
            set
            {
                this.serviceUrl = value;
            }
        }

        public int NewItemsCreated
        {
            get
            {
                return this.newItemsCreated;
            }

            set
            {
                this.newItemsCreated = value;
            }
        }

        public bool RequiresAuthentication 
        {
            get
            {
                return this.requiresAuthentication;
            }
            set
            {
                this.requiresAuthentication = value;
            }
        }

        public ODataStore()
        {
            this.entityTypes = new List<IDataStoreType>();
            this.entitiesTypesByName = new Dictionary<string, ODataEntityType>();
        }

        public IDataStoreType Type(String typeName)
        {
            ODataEntityType entity = this.entitiesTypesByName[typeName];

            if (entity == null)
            {
                entity = new ODataEntityType(this, typeName);
                this.entitiesTypesByName[typeName] = entity;
                this.entityTypes.Add(entity);
            }

            return entity;
        }

        public void Save()
        {
            foreach (ODataEntityType e in this.entityTypes)
            {
                e.Save();
            }
        }
    }
}
