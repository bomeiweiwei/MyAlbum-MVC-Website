using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.EmployeeAccount
{
    public interface IEmployeeAccountCreateRepository
    {
        Task<Guid> CreateEmployeeWithAccountAsync(AccountCreateDto accountDto, EmployeeCreateDto employeeDto, CancellationToken ct = default);
    }
}
