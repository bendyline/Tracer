using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public interface IDataStoreItemSet
    {
        IItem FirstItem
        {
            get;
        }

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

        void Add(IItem item);
    }
}
