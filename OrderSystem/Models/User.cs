using System;
using System.Collections.Generic;

namespace OrderSystem.Models;

public partial class User
{
    public int UserId { get; set; }

    public string GoogleId { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
