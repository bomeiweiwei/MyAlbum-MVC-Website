using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyAlbum.Shared.Enums
{
    public enum UserRole
    {
        /// <summary>
        /// 系統管理者
        /// </summary>
        [Description("系統管理者")]
        Admin = 1,
        /// <summary>
        /// 會員
        /// </summary>
        [Description("會員")]
        Member = 2,
    }
}
