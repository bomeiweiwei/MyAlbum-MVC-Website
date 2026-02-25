using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Employee
{
    public interface IEmployeeUpdateRepository
    {
        Task<UpdateResult> UpdateEmployeeAsync(IAlbumDbContext ctx, EmployeeUpdateDto employeeDto, CancellationToken ct = default);

        Task<UpdateResult> UpdateEmployeeActiveAsync(IAlbumDbContext ctx, EmployeeUpdateDto employeeDto, CancellationToken ct = default);
    }
}
