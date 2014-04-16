/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public abstract class ItemSet : IDataStoreItemSet
    {
        private IDataStoreType list;
        private Query query;

        public IItem FirstItem
        {
            get
            {
                return this.Items[0];
            }
        }
        public abstract List<IItem> Items { get; }

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
                return this.list;
            }
        }

        public ItemSet(IDataStoreType list, Query query)
        {
            this.list = list;
            this.query = query;
        }

        public abstract IItem GetItemById(String id);
        public abstract void BeginRetrieve(AsyncCallback callback, object state);
        public abstract ICollection<IItem> EndRetrieve(IAsyncResult result);
    }
}
