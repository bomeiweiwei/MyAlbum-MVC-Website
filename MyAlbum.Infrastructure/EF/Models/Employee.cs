using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class Employee
{
    public Guid EmployeeId { get; set; }

    public Guid AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public virtual Account Account { get; set; } = null!;
}
