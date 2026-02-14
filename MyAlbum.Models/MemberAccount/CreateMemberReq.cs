using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.MemberAccount
{
    public class CreateMemberReq: BaseCreateReq
    {
        [Required(ErrorMessage = "顯示名稱必填")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "顯示名稱長度需介於 1~50")]
        public string DisplayName { get; set; } = string.Empty;

        // 檔案用
        public string? AvatarPath { get; set; }
    }
}
