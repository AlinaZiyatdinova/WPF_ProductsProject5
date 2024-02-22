using System;
using System.Collections.Generic;

namespace WPF5.Models;

public partial class PickupPoint
{
    public int PickUpId { get; set; }

    public string Index { get; set; } = null!;

    public string? City { get; set; }

    public string? Adress { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
