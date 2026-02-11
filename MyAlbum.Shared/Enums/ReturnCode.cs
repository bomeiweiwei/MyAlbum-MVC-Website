using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyAlbum.Shared.Enums
{
    public enum ReturnCode
    {
        [Description("成功")]
        Succeeded = 0,
        [Description("異常錯誤")]
        ExceptionError = 500,
    }
}
