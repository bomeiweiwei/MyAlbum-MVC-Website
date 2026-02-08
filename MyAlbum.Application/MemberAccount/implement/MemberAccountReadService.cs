using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.MemberAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.MemberAccount.implement
{
    public class MemberAccountReadService : BaseService, IMemberAccountReadService
    {
        private readonly IMemberAccountReadRepository _memberAccountReadRepository;
        public MemberAccountReadService(
          IAlbumDbContextFactory factory,
          IMemberAccountReadRepository memberAccountReadRepository) : base(factory)
        {
            _memberAccountReadRepository = memberAccountReadRepository;
        }

        public async Task<MemberAccountDto?> GetMemberAccountAsync(GetMemberAccountReq req, CancellationToken ct = default)
        {
            return await _memberAccountReadRepository.GetMemberAccountAsync(req, ct);
        }

        public async Task<List<MemberAccountDto>> GetMemberAccountListAsync(GetMemberAccountListReq req, CancellationToken ct = default)
        {
            return await _memberAccountReadRepository.GetMemberAccountListAsync(req, ct);
        }
    }
}
