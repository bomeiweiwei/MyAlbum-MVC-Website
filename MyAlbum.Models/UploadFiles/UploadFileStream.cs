using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.UploadFiles
{
    public sealed class UploadFileStream
    {
        public required string FileName { get; init; }
        public required string ContentType { get; init; }
        public required long Length { get; init; }
        public required Stream Stream { get; init; }
    }
}
