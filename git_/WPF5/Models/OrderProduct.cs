using System;
using System.Collections.Generic;

namespace WPF5.Models;

public partial class OrderProduct
{
    public int OrderId { get; set; }

    public string ProductArticleNumber { get; set; } = null!;

    public int Amount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductArticleNumberNavigation { get; set; } = null!;
}
