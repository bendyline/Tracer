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
        private List<IDataStoreType> entities;
        private Dictionary<String, ODataEntity> entitiesByName;
        private bool requiresAuthentication;
        
        public ICollection<IDataStoreType> Types 
        {
            get
            {
                return this.entities;
            }        
        }

        public String Name 
        {
            get
            {
                return this.name;
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
            this.entities = new List<IDataStoreType>();
            this.entitiesByName = new Dictionary<string, ODataEntity>();
        }

        public IDataStoreType Type(String typeName)
        {
            ODataEntity entity = this.entitiesByName[typeName];

            if (entity == null)
            {
                entity = new ODataEntity(this, typeName);
            }
            return null;
        }
    }
}
