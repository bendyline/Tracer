/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */


#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
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
        RichContent = 9,
        Unknown = -1
    }
}
