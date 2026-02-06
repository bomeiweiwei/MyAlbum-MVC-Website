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

    public virtual ICollection<AlbumCategory> AlbumCategoryCreatedByNavigations { get; set; } = new List<AlbumCategory>();

    public virtual ICollection<AlbumCategory> AlbumCategoryUpdatedByNavigations { get; set; } = new List<AlbumCategory>();

    public virtual ICollection<AlbumComment> AlbumCommentCreatedByNavigations { get; set; } = new List<AlbumComment>();

    public virtual ICollection<AlbumComment> AlbumCommentUpdatedByNavigations { get; set; } = new List<AlbumComment>();

    public virtual ICollection<Album> AlbumCreatedByNavigations { get; set; } = new List<Album>();

    public virtual ICollection<Album> AlbumOwnerAccounts { get; set; } = new List<Album>();

    public virtual ICollection<AlbumPhoto> AlbumPhotoCreatedByNavigations { get; set; } = new List<AlbumPhoto>();

    public virtual ICollection<AlbumPhoto> AlbumPhotoUpdatedByNavigations { get; set; } = new List<AlbumPhoto>();

    public virtual ICollection<Album> AlbumUpdatedByNavigations { get; set; } = new List<Album>();

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Employee? EmployeeAccount { get; set; }

    public virtual ICollection<Employee> EmployeeCreatedByNavigations { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeeUpdatedByNavigations { get; set; } = new List<Employee>();

    public virtual ICollection<Account> InverseCreatedByNavigation { get; set; } = new List<Account>();

    public virtual ICollection<Account> InverseUpdatedByNavigation { get; set; } = new List<Account>();

    public virtual Member? MemberAccount { get; set; }

    public virtual ICollection<Member> MemberCreatedByNavigations { get; set; } = new List<Member>();

    public virtual ICollection<Member> MemberUpdatedByNavigations { get; set; } = new List<Member>();

    public virtual Account UpdatedByNavigation { get; set; } = null!;
}
