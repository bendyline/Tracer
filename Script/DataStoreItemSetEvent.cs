/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;

namespace BL.Data
{
    public delegate void DataStoreItemSetEventHandler(object sender, DataStoreItemSetEventArgs e);

    public class DataStoreItemSetEventArgs : EventArgs
    {
        private IDataStoreItemSet itemSet;
        private List<IItem> addedItems;
        private List<IItem> removedItems;
        private List<IItem> changedItems;

        public List<IItem> AddedItems
        {
            get
            {
                return this.addedItems;
            }
        }

        public List<IItem> RemovedItems
        {
            get
            {
                return this.removedItems;
            }
        }
        
        public List<IItem> ChangedItems
        {
            get
            {
                return this.changedItems;
            }
        }

        public IDataStoreItemSet ItemSet
        {
            get
            {
                return this.itemSet;
            }

            set
            {
                this.itemSet = value;
            }
        }

        public DataStoreItemSetEventArgs(IDataStoreItemSet itemSet)
        {
            this.itemSet = itemSet;
            this.addedItems = new List<IItem>();
            this.removedItems = new List<IItem>();
            this.changedItems = new List<IItem>();
        }

        public static DataStoreItemSetEventArgs ItemAdded(IDataStoreItemSet itemSet, IItem item)
        {
            DataStoreItemSetEventArgs dsisea = new DataStoreItemSetEventArgs(itemSet);

            dsisea.AddedItems.Add(item);

            return dsisea;
        }
        public static DataStoreItemSetEventArgs ItemRemoved(IDataStoreItemSet itemSet, IItem item)
        {
            DataStoreItemSetEventArgs dsisea = new DataStoreItemSetEventArgs(itemSet);

            dsisea.RemovedItems.Add(item);

            return dsisea;
        }
    }
}
