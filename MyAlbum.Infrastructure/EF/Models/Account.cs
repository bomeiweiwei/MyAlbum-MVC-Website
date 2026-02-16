using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class Account
{
    public Guid AccountId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte AccountType { get; set; }

    public byte Status { get; set; }

    public DateTime? LastLoginAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Employee? Employee { get; set; }

    public virtual Member? Member { get; set; }
}
