/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public class FieldChoice : SerializableObject
    {
        private String displayName;
        private String id;
        private String imageUrl;
        
        public String Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        public String DisplayName
        {
            get
            {

                if (this.displayName == null)
                {
                    return this.id;
                }

                return this.displayName;
            }

            set
            {
                this.displayName = value;
            }
        }
        public String ImageUrl
        {
            get
            {
                return this.imageUrl;
            }

            set
            {
                this.imageUrl = value;
            }
        }

        public FieldChoice()
        {
        }
    }
}
