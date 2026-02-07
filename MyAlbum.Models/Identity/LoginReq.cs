using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.Identity
{
    public class LoginReq
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public UserRole Role { get; set; } = UserRole.Member;
    }
}
