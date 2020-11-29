/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public class FieldChoice : SerializableObject, IFieldChoice
    {
        private String displayName;
        private object id;
        private String imageUrl;
        
        public object EffectiveId
        {
            get
            {
                object abstractedId = this.id;

                if (abstractedId == null)
                {
                    abstractedId = this.DisplayName;
                }

                return abstractedId;
            }
        }

        public object Id
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
                    if (this.id == null)
                    {
                        return String.Empty;
                    }

                    return this.id.ToString();
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
