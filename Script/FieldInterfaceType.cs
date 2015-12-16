/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */


#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public enum FieldInterfaceType
    {
        TypeDefault = 0,
        User = 1,
        Choice = 2,
        Scale = 3,
        Phone = 4,
        Email = 5,
        Numeric = 6,
        UserList = 7,
        Checkbox = 8,
        SwitchToggle = 9,
        DateTimePicker = 10,
        DatePicker = 11,
        TimePicker = 12,
        Order = 13,
        Image = 14,
        MeUser = 15,
        NoValue = 99
    }
}
