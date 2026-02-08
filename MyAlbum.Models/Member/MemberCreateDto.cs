using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Member
{
    public class MemberCreateDto
    {
        public Guid MemberId { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
