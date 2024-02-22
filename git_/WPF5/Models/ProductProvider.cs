using System;
using System.Collections.Generic;

namespace WPF5.Models;

public partial class ProductProvider
{
    public int ProviderId { get; set; }

    public string ProviderName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
