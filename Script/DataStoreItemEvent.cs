/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

namespace BL.Data
{
    public delegate void DataStoreItemEventHandler(object sender, DataStoreItemEventArgs e);

    public class DataStoreItemEventArgs : EventArgs
    {
        private IItem item;

        public IItem Item
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

        public DataStoreItemEventArgs(IItem item)
        {
            this.item = item;
        }
    }
}
