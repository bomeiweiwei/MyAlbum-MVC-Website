using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Uploads
{
    public interface IFileUploadService
    {
        EntityUploadType entityUploadType { get; }
        /// <summary>
        /// 上傳
        /// </summary>
        /// <param name="model"></param>
        /// <param name="files"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> Upload(UploadModel model, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default);
    }
}
