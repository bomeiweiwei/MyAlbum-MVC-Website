using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Uploads
{
    public interface IFileUploadManagerService
    {
        Task<Dictionary<string,string>> FileUpload(UploadModel model, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default);
    }
}
