using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Shared.Idenyity
{
    public interface ICurrentUserAccessor
    {
        Guid? AccountId { get; }
        string? UserName { get; }
        string? AccountType { get; }
    }

}
