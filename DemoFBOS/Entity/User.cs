using System;
using System.Collections.Generic;

namespace DemoFBOS.Entity;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
