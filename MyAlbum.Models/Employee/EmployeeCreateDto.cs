using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Employee
{
    public class EmployeeCreateDto
    {
        public Guid EmployeeId { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
