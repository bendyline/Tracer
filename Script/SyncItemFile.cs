/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */


#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public class SyncItemFile
    {
        private IItem item;
        private File file;

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

        private File File
        {
            get
            {
                return this.file;
            }

            set
            {
                this.file = value;
            }
        }
    }
}
