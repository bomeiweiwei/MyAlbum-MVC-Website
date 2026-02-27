using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Account
{
    public interface IAccountUpdateRepository
    {
        Task<UpdateResult> UpdateAccountAsync(IAlbumDbContext ctx, AccountUpdateDto accountDto, CancellationToken ct = default);

        Task<UpdateResult> UpdateAccountActiveAsync(IAlbumDbContext ctx, AccountUpdateDto accountDto, CancellationToken ct = default);
    }
}
