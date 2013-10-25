// ODataCallbacks.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BL.Data.Native
{
    [Imported]
    [IgnoreNamespace]
    public delegate void ODataRequestSuccess(object data, object response);
    
    [Imported]
    [IgnoreNamespace]
    public delegate void ODataRequestFailure(String error);
}
