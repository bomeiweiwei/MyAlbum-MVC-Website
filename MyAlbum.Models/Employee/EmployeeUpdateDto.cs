using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Employee
{
    public class EmployeeUpdateDto
    {
        public Guid EmployeeId { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public Status Status { get; set; }
        public Guid UpdateBy { get; set; }
    }
}
