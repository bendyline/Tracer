/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */


#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    interface IPersistenceAdapter
    {
        DataStore DataStore { get; set; }
        void LoadAsync();
        void SaveAsync();
    }
}
