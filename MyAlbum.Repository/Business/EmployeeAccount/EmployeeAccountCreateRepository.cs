using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.EmployeeAccount
{
    public sealed class EmployeeAccountCreateRepository : IEmployeeAccountCreateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeAccountCreateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<Guid> CreateEmployeeWithAccountAsync(AccountCreateDto accountDto, EmployeeCreateDto employeeDto, CancellationToken ct = default)
        {
            Guid result = Guid.Empty;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();


            var exists = await db.Accounts.AsNoTracking()
                .AnyAsync(a => a.UserName == accountDto.UserName, ct);

            if (exists)
            {
                throw new InvalidOperationException("帳號已存在。");
            }

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    DateTime now = DateTime.UtcNow;

                    var account = new Account
                    {
                        AccountId = accountDto.AccountId,
                        UserName = accountDto.UserName,
                        PasswordHash = accountDto.PasswordHash,
                        AccountType = (int)AccountType.Admin,
                        Status = (int)Status.Active,
                        CreatedAtUtc = now,
                        CreatedBy = accountDto.CreatedBy,
                        UpdatedAtUtc = now,
                        UpdatedBy = accountDto.CreatedBy
                    };
                    await db.Accounts.AddAsync(account, ct);

                    var employee = new Employee
                    {
                        EmployeeId = employeeDto.EmployeeId,
                        AccountId = account.AccountId,
                        Email = employeeDto.Email,
                        Phone = employeeDto.Phone,
                        Status = (int)Status.Active,
                        CreatedAtUtc = now,
                        CreatedBy = employeeDto.CreatedBy,
                        UpdatedAtUtc = now,
                        UpdatedBy = employeeDto.CreatedBy
                    };
                    await db.Employees.AddAsync(employee, ct);

                    await db.SaveChangesAsync(ct);

                    await tx.CommitAsync(ct);

                    result = employee.EmployeeId;
                }
                catch
                {
                    await tx.RollbackAsync(ct);
                    throw;
                }
            });

            return result;
        }
    }
}
