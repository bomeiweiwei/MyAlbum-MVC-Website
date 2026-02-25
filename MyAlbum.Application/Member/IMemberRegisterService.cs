using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Member
{
    public interface IMemberRegisterService
    {
        Task<Guid> Register(CreateMemberReq req, CancellationToken ct = default);
    }
}
