using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WPF5.Models;

public partial class Product
{
    [Required(ErrorMessage = "Артикул товара является обязательным")]
    [RegularExpression("[A-Z0-9]{6}", ErrorMessage = "Артикул должен состоять из 6 символов A-Z или 0-9")]
    public string ProductArticleNumber { get; set; } = null!;


    [Required(ErrorMessage = "Наименование товара является обязательным")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Наименование должно быть длиной от 2 до 200 символов")]
    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public int ProductCategoryId { get; set; }

    public int ProductManufacturerId { get; set; }

    [Required(ErrorMessage = "Стоимость товара является обязательной")]
    [Range(0, 20000, ErrorMessage = "Укажите стоимость товара в диапазоне от 0 до 20000")]
    public int ProductCost { get; set; }

    public int ProductDiscountAmount { get; set; }
    [Required(ErrorMessage = "Количество товара на складе является обязательным")]
    [Range(0, 1000, ErrorMessage = "Укажите количество товара на складе в диапазоне от 0 до 1000")]

    public int ProductQuantityInStock { get; set; }

    public int ProductProviderId { get; set; }

    public string? ProductPhoto { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    [Required(ErrorMessage = "Категория товара является обязательной")]

    public virtual ProductCategory ProductCategory { get; set; } = null!;
    [Required(ErrorMessage = "Производитель товара является обязательным")]

    public virtual ProductManufacture ProductManufacturer { get; set; } = null!;
    [Required(ErrorMessage = "Поставщик товара является обязательным")]

    public virtual ProductProvider ProductProvider { get; set; } = null!;
}
