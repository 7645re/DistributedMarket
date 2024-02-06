using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain.Models;


[Table("Product")]
public class ProductEntity
{
    [Key]
    public int Id { get; set; } = default!;
    
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }
    
    [Required]
    [Precision(19, 5)]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; }

    public ICollection<CategoryEntity> Categories { get; set; } = new List<CategoryEntity>();
}