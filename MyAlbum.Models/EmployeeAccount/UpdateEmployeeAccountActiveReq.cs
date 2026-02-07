using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.EmployeeAccount
{
    public class UpdateEmployeeAccountActiveReq
    {
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public Guid AccountId { get; set; }

        public Status AccountStatus { get; set; } = Status.Active;
        public Status EmployeeStatus { get; set; } = Status.Active;
    }
}
