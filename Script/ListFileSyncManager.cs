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
    public class ListFileSyncManager
    {
        private ItemType list;
        private FileManager fileManager;
        private String clientId;
        private Folder folder;

        public Folder ListFolder
        {
            get
            {
                return this.folder;
            }

            set
            {
                this.folder = value;
            }
        }

        public String ClientId
        {
            get
            {
                return this.clientId;
            }

            set
            {
                this.clientId = value;
            }
        }

        public ItemType List
        {
            get
            {
                return this.list;
            }

            set
            {
                this.list = value;
            }
        }

        public FileManager FileManager
        {
            get
            {
                return this.fileManager;
            }

            set
            {
                this.fileManager = value;
            }
        }

        public void SyncToFiles()
        {

        }
            
    }
}
