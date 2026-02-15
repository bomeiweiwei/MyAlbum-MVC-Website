using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models.Base;
using MyAlbum.Models.EmployeeAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.EmployeeAccount.implement
{
    public class EmployeeAccountReadService : BaseService, IEmployeeAccountReadService
    {
        private readonly IEmployeeAccountReadRepository _employeeAccountReadRepository;
        public EmployeeAccountReadService(
          IAlbumDbContextFactory factory,
          IEmployeeAccountReadRepository employeeAccountReadRepository) : base(factory)
        {
            _employeeAccountReadRepository = employeeAccountReadRepository;
        }

        public async Task<EmployeeAccountDto?> GetEmployeeAccountAsync(GetEmployeeAccountReq req, CancellationToken ct = default)
        {
            return await _employeeAccountReadRepository.GetEmployeeAccountAsync(req, ct);
        }

        public async Task<ResponseBase<List<EmployeeAccountDto>>> GetEmployeeAccountListAsync(PageRequestBase<GetEmployeeAccountListReq> req, CancellationToken ct = default)
        {
            return await _employeeAccountReadRepository.GetEmployeeAccountListAsync(req, ct);
        }
    }
}
