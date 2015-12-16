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
    public delegate void DataStoreItemChangedEventHandler(object sender, DataStoreItemChangedEventArgs e);

    public class DataStoreItemChangedEventArgs : DataStoreItemEventArgs
    {
        private String singlePropertyChangedName;
        private List<String> allChangedProperties;

        public List<String> ChangedProperties
        {
            get
            {
                if (allChangedProperties == null && this.singlePropertyChangedName != null)
                {
                    this.allChangedProperties = new List<String>();
                    this.allChangedProperties.Add(this.singlePropertyChangedName);
                }

                return this.allChangedProperties;
            }
        }

        public DataStoreItemChangedEventArgs(IItem item, String propertyName, List<String> allChangedProperties) : base(item)
        {
            this.singlePropertyChangedName = propertyName;
            this.allChangedProperties = allChangedProperties;
        }
    }
}
