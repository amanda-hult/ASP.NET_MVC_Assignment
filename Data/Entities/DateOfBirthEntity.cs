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
    public int Date {  get; set; }

    [Required]
    [Column(TypeName = "varchar(10)")]
    public string Month { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(4)")]
    public int Year { get; set; }
}
