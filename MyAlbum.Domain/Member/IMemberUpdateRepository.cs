using MyAlbum.Models.Employee;
using MyAlbum.Models.Member;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Member
{
    public interface IMemberUpdateRepository
    {
        Task<bool> UpdateMemberAvatarPathAsync(Guid memberId, string fileKey, Guid operatorId, CancellationToken ct);

        Task<bool> UpdateMemberAsync(IAlbumDbContext ctx, MemberUpdateDto memberDto, CancellationToken ct = default);

        Task<bool> UpdateMemberActiveAsync(IAlbumDbContext ctx, MemberUpdateDto memberDto, CancellationToken ct = default);
    }
}
