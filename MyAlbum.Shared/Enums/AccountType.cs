using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyAlbum.Shared.Enums
{
    public enum AccountType
    {
        [Description("Admin")]
        Admin = 1,
        [Description("Member")]
        Member = 2
    }
}
