using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Account
{
    public class AccountLoginResp
    {
        public bool IsLoginSuccess { get; set; }

        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Member;
    }
}
