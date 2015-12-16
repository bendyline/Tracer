/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

namespace Bendyline.Data
{
    public delegate void DataStoreItemEventHandler(object sender, DataStoreItemEventArgs e);

    public class DataStoreItemEventArgs : EventArgs
    {
        private IDataStoreItem item;

        public IDataStoreItem Item
        {
            get
            {
                return this.item;
            }

            set
            {
                this.item = value;
            }
        }

        public DataStoreItemEventArgs(IDataStoreItem item)
        {
            this.item = item;
        }
    }
}
