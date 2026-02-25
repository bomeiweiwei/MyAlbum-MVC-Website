using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Employee;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Employee
{
    public class EmployeeUpdateRepository : IEmployeeUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<UpdateResult> UpdateEmployeeAsync(IAlbumDbContext ctx, EmployeeUpdateDto employeeDto, CancellationToken ct = default)
        {
            var db = ctx.AsDbContext<MyAlbumContext>();
            var data = await db.Employees.Where(x => x.EmployeeId == employeeDto.EmployeeId && x.AccountId == employeeDto.AccountId).FirstOrDefaultAsync(ct);
            if (data == null)
                return UpdateResult.NotFound;

            data.Email = employeeDto.Email;
            data.Phone = employeeDto.Phone;
            data.Status = (byte)employeeDto.Status;
            data.UpdatedBy = employeeDto.UpdateBy;
            data.UpdatedAtUtc = DateTime.UtcNow;
            int check = await ctx.SaveChangesAsync(ct);
            if (check == 1)
                return UpdateResult.Updated;
            return UpdateResult.NoChange;
        }

        public async Task<UpdateResult> UpdateEmployeeActiveAsync(IAlbumDbContext ctx, EmployeeUpdateDto employeeDto, CancellationToken ct = default)
        {
            var db = ctx.AsDbContext<MyAlbumContext>();
            var data = await db.Employees.Where(x => x.EmployeeId == employeeDto.EmployeeId && x.AccountId == employeeDto.AccountId).FirstOrDefaultAsync(ct);
            if (data == null)
                return UpdateResult.NotFound;

            data.Status = (byte)employeeDto.Status;
            data.UpdatedBy = employeeDto.UpdateBy;
            data.UpdatedAtUtc = DateTime.UtcNow;
            int check = await ctx.SaveChangesAsync(ct);
            if (check == 1)
                return UpdateResult.Updated;
            return UpdateResult.NoChange;
        }
    }
}
