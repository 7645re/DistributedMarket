using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Domain.Models;


[Table("Category")]
public class CategoryEntity
{
    [Key]
    public int Id { get; set; } = default!;
    
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }
    
    public ICollection<ProductEntity> Products { get; set; }
}