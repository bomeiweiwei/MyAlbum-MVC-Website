using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Employee
{
    public interface IEmployeeUpdateRepository
    {
        Task<bool> UpdateEmployeeAsync(IAlbumDbContext ctx, EmployeeUpdateDto employeeDto, CancellationToken ct = default);

        Task<bool> UpdateEmployeeActiveAsync(IAlbumDbContext ctx, EmployeeUpdateDto employeeDto, CancellationToken ct = default);
    }
}
