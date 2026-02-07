using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Account
{
    public interface IAccountUpdateRepository
    {
        Task<bool> UpdateAccountAsync(IAlbumDbContext ctx, AccountUpdateDto accountDto, CancellationToken ct = default);
    }
}
