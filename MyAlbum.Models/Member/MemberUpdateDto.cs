using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Member
{
    public class MemberUpdateDto
    {
        public Guid MemberId { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public Status Status { get; set; }
        public Guid UpdateBy { get; set; }
    }
}
