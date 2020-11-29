using System;

namespace BL.Data
{
    public interface IFieldChoice
    {

        object Id { get; set; }

        String DisplayName { get; set; }

        String ImageUrl { get; set;  }
    }
}
