using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.MemberAccount
{
    public class UpdateMemberAccountActiveReq
    {
        [Required]
        public Guid MemberId { get; set; }
        [Required]
        public Guid AccountId { get; set; }

        public Status AccountStatus { get; set; } = Status.Active;
        public Status MemberStatus { get; set; } = Status.Active;
    }
}
