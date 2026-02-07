using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyAlbum.Shared.Enums
{
    public enum Status
    {
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disabled = 0,
        /// <summary>
        /// 啟用
        /// </summary>
        [Description("啟用")]
        Active = 1,
    }
}
