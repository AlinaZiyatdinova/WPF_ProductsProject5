using System;
using System.Collections.Generic;

namespace WPF5.Models;

public partial class UserOrder
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
