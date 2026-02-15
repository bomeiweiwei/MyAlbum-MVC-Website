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
        [StringLength(20)]
        public string? Phone { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Phone))
            {
                var phoneAttr = new PhoneAttribute();
                if (!phoneAttr.IsValid(Phone))
                {
                    yield return new ValidationResult("手機格式不正確", new[] { nameof(Phone) });
                }
            }
        }
    }
}
