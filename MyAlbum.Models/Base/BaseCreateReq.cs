using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.Base
{
    public class BaseCreateReq
    {
        [Required(ErrorMessage = "帳號必填")]
        [Display(Name = "帳號")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "登入帳號長度需介於 2~20")]
        [RegularExpression(@"^[A-Za-z0-9._-]+$", ErrorMessage = "登入帳號僅允許英數字與 . _ -")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼必填")]
        [Display(Name = "密碼")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "密碼至少 6 碼")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "請再次輸入密碼")]
        [Display(Name = "確認密碼")]
        [Compare(nameof(Password), ErrorMessage = "兩次密碼不一致")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email必填")]
        [Display(Name = "信箱")]
        [EmailAddress(ErrorMessage = "Email 格式不正確")]
        [StringLength(320)]
        public string Email { get; set; } = string.Empty;
    }
}
