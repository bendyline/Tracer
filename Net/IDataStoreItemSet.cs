/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bendyline.Base
{
    public interface IDataStoreItemSet
    {
        IList<IDataStoreItem> Items
        {
            get;
        }

        IDataStoreList List
        {
            get;
        }

        DataStoreQuery Query
        {
            get;
        }

        event DataStoreItemSetEventHandler ItemSetChanged;

        IDataStoreItem GetItemById(String id);
        void BeginRetrieve(AsyncCallback callback, object state);
        ICollection<IDataStoreItem> EndRetrieve(IAsyncResult result);
    }
}
