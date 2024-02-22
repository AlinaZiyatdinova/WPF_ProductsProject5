using System;
using System.Collections.Generic;

namespace WPF5.Models;

public partial class ProductCategory
{
    public int CategriesId { get; set; }

    public string CategoriesName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
