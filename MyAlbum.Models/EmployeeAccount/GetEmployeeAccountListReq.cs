using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.EmployeeAccount
{
    public class GetEmployeeAccountListReq
    {
        public string? UserName { get; set; }
        public Status? Status { get; set; }
    }
}
