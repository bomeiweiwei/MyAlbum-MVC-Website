using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.MemberAccount
{
    public class MemberAccountDto
    {
        public Guid MemberId { get; set; }
        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public Status Status { get; set; }

        public string? PublicAvatarUrl { get; set; }
    }
}
