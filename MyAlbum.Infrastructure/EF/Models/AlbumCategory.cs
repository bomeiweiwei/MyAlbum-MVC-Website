using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class AlbumCategory
{
    public Guid AlbumCategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int SortOrder { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Account UpdatedByNavigation { get; set; } = null!;
}
