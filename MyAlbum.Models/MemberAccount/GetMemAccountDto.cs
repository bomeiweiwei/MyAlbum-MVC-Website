using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.MemberAccount
{
    public class GetMemAccountDto
    {
        public string UserName { get; set; }
        public Status AccountStatus { get; set; } = Status.Active;
        public Status MemStatus { get; set; } = Status.Active;
    }
}
