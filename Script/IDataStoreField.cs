/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;


#if NET
using Bendyline.Base;
using System.ComponentModel;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public interface IDataStoreField
    {
        event PropertyChangedEventHandler PropertyChanged;
        bool Required { get; }

        bool AllowNull{ get; }

        FieldChoiceCollection Choices { get;  }
        String DisplayName { get; }
        String Name { get; }
        FieldType Type { get; }
        FieldInterfaceType InterfaceType { get; }
        FieldInterfaceTypeOptions InterfaceTypeOptions { get; set;  }
    }
}
