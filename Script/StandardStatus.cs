
#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public enum StandardStatus
    {
        Normal = 0,
        Template = 10,
        Deleted = 86
    }
}
