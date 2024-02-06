using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Models;

[Table("ProductCategory")]
[PrimaryKey(nameof(ProductId), nameof(CategoryId))]
public class ProductEntityCategoryEntity
{
    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public int CategoryId { get; set; }

    public ProductEntity Product { get; set; }

    public CategoryEntity Category { get; set; }
}