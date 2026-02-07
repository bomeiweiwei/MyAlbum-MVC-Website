using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.EmployeeAccount
{
    public class GetEmpAccountDto
    {
        public string UserName { get; set; }
        public Status AccountStatus { get; set; } = Status.Active;
        public Status EmpStatus { get; set; } = Status.Active;
    }
}
