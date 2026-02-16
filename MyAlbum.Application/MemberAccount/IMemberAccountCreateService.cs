using MyAlbum.Models.Account;
using MyAlbum.Models.Member;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.MemberAccount
{
    public interface IMemberAccountCreateService
    {
        Task<Guid> CreateMemberWithAccountAsync(CreateMemberReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default);
    }
}
