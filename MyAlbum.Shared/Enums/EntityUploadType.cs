using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyAlbum.Shared.Enums
{
    public enum EntityUploadType
    {
        [Description("會員")]
        Member = 1,
        [Description("相簿")]
        Album = 2,
        [Description("相簿照片")]
        AlbumPhoto = 3,
    }
}
