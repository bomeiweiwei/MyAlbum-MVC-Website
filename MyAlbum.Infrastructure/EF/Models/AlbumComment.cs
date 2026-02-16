using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class AlbumComment
{
    public Guid AlbumCommentId { get; set; }

    public Guid AlbumPhotoId { get; set; }

    public Guid MemberId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime ReleaseTimeUtc { get; set; }

    public bool IsChanged { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public virtual AlbumPhoto AlbumPhoto { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
