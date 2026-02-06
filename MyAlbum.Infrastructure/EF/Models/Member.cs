using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class Member
{
    public Guid MemberId { get; set; }

    public Guid AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? AvatarPath { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<AlbumComment> AlbumComments { get; set; } = new List<AlbumComment>();

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Account UpdatedByNavigation { get; set; } = null!;
}
