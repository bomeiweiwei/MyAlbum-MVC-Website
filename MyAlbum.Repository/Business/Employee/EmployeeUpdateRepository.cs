using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Employee;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Employee
{
    public class EmployeeUpdateRepository : IEmployeeUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<bool> UpdateEmployeeAsync(IAlbumDbContext ctx, EmployeeUpdateDto employeeDto, CancellationToken ct = default)
        {
            var result = false;

            var db = ctx.AsDbContext<MyAlbumContext>();
            var data = await db.Employees.Where(x => x.EmployeeId == employeeDto.EmployeeId && x.AccountId == employeeDto.AccountId).FirstOrDefaultAsync(ct);
            if (data == null)
                return result;

            data.Email = employeeDto.Email;
            data.Phone = employeeDto.Phone;
            data.Status = (byte)employeeDto.Status;
            data.UpdatedBy = employeeDto.UpdateBy;
            data.UpdatedAtUtc = DateTime.UtcNow;
            int check = await ctx.SaveChangesAsync(ct);
            if (check == 1)
                result = true;
            return result;
        }
    }
}
