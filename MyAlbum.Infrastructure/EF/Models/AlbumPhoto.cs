using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class AlbumPhoto
{
    public Guid AlbumPhotoId { get; set; }

    public Guid AlbumId { get; set; }

    public string FilePath { get; set; } = null!;

    public string? OriginalFileName { get; set; }

    public string? ContentType { get; set; }

    public long FileSizeBytes { get; set; }

    public int SortOrder { get; set; }

    public int CommentNum { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual ICollection<AlbumComment> AlbumComments { get; set; } = new List<AlbumComment>();
}
