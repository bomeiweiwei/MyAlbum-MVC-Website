using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.MemberAccount
{
    public class GetMemberAccountReq
    {
        public Guid AccountId { get; set; }
        public Guid MemberId { get; set; }
    }
}
