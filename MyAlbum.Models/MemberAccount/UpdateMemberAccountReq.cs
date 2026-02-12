using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.MemberAccount
{
    public class UpdateMemberAccountReq
    {
        [Required]
        public Guid MemberId { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email必填")]
        [EmailAddress(ErrorMessage = "Email 格式不正確")]
        [StringLength(320)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "顯示名稱必填")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "顯示名稱長度需介於 1~50")]
        public string DisplayName { get; set; } = string.Empty;

        public Status Status { get; set; } = Status.Active;

        // 檔案用
        public string? AvatarPath { get; set; }
        // 檔案上傳用
        public byte[]? FileBytes { get; set; }
        public string? FileName { get; set; }

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
        }
    }
}
