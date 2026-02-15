using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.MemberAccount
{
    public class GetMemberAccountListReq
    {
        public string? UserName { get; set; }

        public string? DisplayName { get; set; }

        public Status? Status { get; set; }
    }
}
