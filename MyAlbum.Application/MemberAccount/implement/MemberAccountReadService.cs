using MyAlbum.Application.Member;
using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Models.Base;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.MemberAccount.implement
{
    public class MemberAccountReadService : BaseService, IMemberAccountReadService
    {
        private readonly IMemberAccountReadRepository _memberAccountReadRepository;
        private readonly IUploadPathService _paths;
        public MemberAccountReadService(
          IAlbumDbContextFactory factory,
          IMemberAccountReadRepository memberAccountReadRepository,
          IUploadPathService paths) : base(factory)
        {
            _memberAccountReadRepository = memberAccountReadRepository;
            _paths = paths;
        }

        public async Task<MemberAccountDto?> GetMemberAccountAsync(GetMemberAccountReq req, CancellationToken ct = default)
        {
            var data = await _memberAccountReadRepository.GetMemberAccountAsync(req, ct);
            if (data != null)
                data.PublicAvatarUrl = _paths.ToPublicUrl(data.PublicAvatarUrl);
            return data;
        }

        public async Task<ResponseBase<List<MemberAccountDto>>> GetMemberAccountListAsync(PageRequestBase<GetMemberAccountListReq> req, CancellationToken ct = default)
        {
            var list = await _memberAccountReadRepository.GetMemberAccountListAsync(req, ct);
            if (list.Data.Count > 0)
            {
                foreach (var data in list.Data)
                {
                    data.PublicAvatarUrl = _paths.ToPublicUrl(data.PublicAvatarUrl);
                }
            }
            return list;
        }

        public async Task<List<MemberAccountDto>> GetMemberAccountItemListAsync(GetMemberAccountListReq req, CancellationToken ct = default)
        {
            req.Status = Status.Active;
            var pageReq = new PageRequestBase<GetMemberAccountListReq>()
            {
                pageIndex = 1,
                pageSize = 99999,
                Data = req
            };
            //pageReq.Data.Status = Status.Active; // 由前端帶入
            var dataList = await GetMemberAccountListAsync(pageReq);

            var list = dataList.Data;
            list = list.Select(m => new MemberAccountDto
            {
                AccountId = m.AccountId,
                UserName = m.UserName,
                DisplayName = m.DisplayName,
                Status = m.Status
            }).ToList();

            return list;
        }

    }
}
