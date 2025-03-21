using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(DateOfBirthId), IsUnique = true)]
public class DateOfBirthEntity
{
    [Key]
    public int DateOfBirthId { get; set; }

    // -> DateTime ?
    [Required]
    [Column(TypeName = "varchar(2)")]
    public int? Date {  get; set; }

    [Required]
    [Column(TypeName = "varchar(2)")]
    public int? Month { get; set; }

    [Required]
    [Column(TypeName = "varchar(4)")]
    public int? Year { get; set; }
}
