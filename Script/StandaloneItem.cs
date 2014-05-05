/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

namespace BL.Data
{
    public class StandaloneItem : Item
    {
        private String id;

        public override String Id 
        { 
            get
            {
                return this.id;
            }
        }
        public void SetId(String id)
        {
            this.id = id;
        }

        
        public StandaloneItem(IDataStoreType list) : base(list)
        {
        }
    }    
}
