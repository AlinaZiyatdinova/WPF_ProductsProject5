using System;
using System.Collections.Generic;

namespace WPF5.Models;

public partial class ProductManufacture
{
    public int ManufacturerId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
