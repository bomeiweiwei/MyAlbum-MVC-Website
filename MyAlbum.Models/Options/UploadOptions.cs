using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Options
{
    public sealed class UploadOptions
    {
        public string RootPath { get; set; } = string.Empty;

        public string MemberImgRoot { get; set; } = "MemberImages";
        public string CoverImgRoot { get; set; } = "CoverImages";
        public string PhotoImgRoot { get; set; } = "PhotoImages";
    }
}
