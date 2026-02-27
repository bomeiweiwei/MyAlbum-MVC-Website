using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.EmployeeAccount
{
    public class CreateEmployeeWithAccountResp
    {
        public Guid AccountId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
