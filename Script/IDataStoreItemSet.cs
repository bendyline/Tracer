using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public interface IDataStoreItemSet
    {
        List<IItem> Items
        {
            get;
        }

        IDataStoreType Type
        {
            get;
        }

        Query Query
        {
            get;
        }

        event DataStoreItemSetEventHandler ItemSetChanged;

        IItem GetItemById(String id);
        void BeginRetrieve(AsyncCallback callback, object state);
        ICollection<IItem> EndRetrieve(IAsyncResult result);
    }
}
