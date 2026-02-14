using MyAlbum.Application.Identity;
using MyAlbum.Domain;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Uploads.implement
{
    public class FileUploadManagerService : BaseService, IFileUploadManagerService
    {
        private readonly IEnumerable<IFileUploadService> _fileUploadServices;
        public FileUploadManagerService(IAlbumDbContextFactory factory, IEnumerable<IFileUploadService> fileUploadServices) : base(factory)
        {
            _fileUploadServices = fileUploadServices;
        }
        public async Task<Dictionary<string, string>> FileUpload(UploadModel model, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            var service = _fileUploadServices.FirstOrDefault(x => x.entityUploadType == model.EntityUploadType);
            if (service == null)
            {
                throw new InvalidOperationException($"No upload service found for type {model.EntityUploadType}");
            }
            else
            {
                return await service.Upload(model, files, ct);
            }
        }
    }
}
