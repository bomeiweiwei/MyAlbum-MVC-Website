using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Identity
{
    public class AccountDto
    {
        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
