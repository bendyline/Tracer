﻿/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

namespace BL.Data
{
    public enum FieldType
    {
        ShortText = 0,
        UnboundedText = 1,
        Integer = 2,
        BigInteger = 4,
        DateTime = 5,
        Geopoint = 6,
        BigNumber = 7,
        BoolChoice = 8,
    }
}
