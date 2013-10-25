// ODataRequest.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BL.Data.Native
{
    [IgnoreNamespace]
    [Imported]
    public class ODataRequest
    {
        public String RequestUri;
        public String Method;
        public object Data;
    }
}
