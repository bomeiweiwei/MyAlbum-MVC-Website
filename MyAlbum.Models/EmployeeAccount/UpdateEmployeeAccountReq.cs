using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.EmployeeAccount
{
    public class UpdateEmployeeAccountReq
    {
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public Guid AccountId { get; set; }

        public string? Password { get; set; } = null;

        public string? ConfirmPassword { get; set; } = null;

        [Required(ErrorMessage = "Email必填")]
        [EmailAddress(ErrorMessage = "Email 格式不正確")]
        [StringLength(320)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        public Status Status { get; set; } = Status.Active;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 有輸入密碼才驗證
            if (!string.IsNullOrWhiteSpace(Password))
            {
                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    yield return new ValidationResult(
                        "請再次輸入密碼",
                        new[] { nameof(ConfirmPassword) }
                    );
                }
                else if (Password != ConfirmPassword)
                {
                    yield return new ValidationResult(
                        "兩次密碼不一致",
                        new[] { nameof(ConfirmPassword) }
                    );
                }

                if (Password.Length < 6)
                {
                    yield return new ValidationResult(
                        "密碼至少 6 碼",
                        new[] { nameof(Password) }
                    );
                }
            }

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
