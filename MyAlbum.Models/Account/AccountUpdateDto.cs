using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Account
{
    public class AccountUpdateDto
    {
        public Guid AccountId { get; set; }
        public string? PasswordHash { get; set; }
        public Status Status { get; set; }
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
        public Guid UpdateBy { get; set; }
    }
}
