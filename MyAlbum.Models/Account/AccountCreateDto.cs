using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Account
{
    public class AccountCreateDto
    {
        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public Guid CreatedBy { get; set; }
    }
}
