using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Member
{
    public interface IMemberUpdateRepository
    {
        Task<bool> UpdateMemberAvatarPathAsync(Guid memberId, string fileKey, Guid operatorId, CancellationToken ct);
    }
}
