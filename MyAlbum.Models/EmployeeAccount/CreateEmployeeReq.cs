using MyAlbum.Models.Base;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.EmployeeAccount
{
    public class CreateEmployeeReq : BaseCreateReq
    {
        [Phone(ErrorMessage = "手機格式不正確")]
        [StringLength(20)]
        public string? Phone { get; set; } = null;
    }
}
