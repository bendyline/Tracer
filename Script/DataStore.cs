/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */
using System;
using System.Collections.Generic;

#if NET

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public enum DataStoreType
    {
        File = 0,
        SharePoint = 1,
        Synchronizing = 2,
        Object = 3,
        OData = 4
    }

    public class DataStore : IDataStore
    {
        private String location;
        private String name;
        private bool requiresAuthentication;
        private List<IDataStoreType> lists;
        private Dictionary<String, IDataStoreType> listsByName;

        public static IDataStore Create(DataStoreType type)
        {
            IDataStore store = null;

            switch (type)
            {
                case DataStoreType.SharePoint:
#if SCRIPTSHARP
                    Script.Literal("{0}= new BL.SP.Web();",store);
#else
                    throw new NotImplementedException();
#endif
                    break;
             }

            return store;
        }

        public static IDataStore CreateByTypeName(String typeName)
        {
            throw new Exception(String.Format("Specified type '{0}' is not of the appropriate type.", typeName));
        }

        public ICollection<IDataStoreType> Types 
        { 
            get
            {
                return this.lists;
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

        public DataStore()
        {
            this.lists = new List<IDataStoreType>();
            this.listsByName = new Dictionary<string, IDataStoreType>();
        }

        public IDataStoreType Type(String listName)
        {
            return null;
        }

    }
}
