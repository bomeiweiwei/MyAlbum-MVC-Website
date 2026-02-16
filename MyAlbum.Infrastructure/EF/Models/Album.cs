using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class Album
{
    public Guid AlbumId { get; set; }

    public Guid AlbumCategoryId { get; set; }

    public Guid OwnerAccountId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? CoverPath { get; set; }

    public DateTime ReleaseTimeUtc { get; set; }

    public int TotalCommentNum { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public virtual AlbumCategory AlbumCategory { get; set; } = null!;

    public virtual ICollection<AlbumPhoto> AlbumPhotos { get; set; } = new List<AlbumPhoto>();

    public virtual Account OwnerAccount { get; set; } = null!;
}
