/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public class ODataItemSet : IDataStoreItemSet
    {
        private List<IItem> items;
        private Dictionary<String, Item> itemsById;

        private ODataEntity entity;
        private Query query;

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
            this.entity = (ODataEntity)list;
            this.query = query;
            this.itemsById = new Dictionary<string, Item>();
        }

        public IItem GetItemById(String id)
        {
            return this.itemsById[id];
        }

        public void BeginRetrieve(AsyncCallback callback, object state)
        {

        }

        public ICollection<IItem> EndRetrieve(IAsyncResult result)
        {
            return null;
        }
    }
}
