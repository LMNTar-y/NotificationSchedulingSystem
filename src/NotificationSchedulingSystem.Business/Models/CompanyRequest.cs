using System.ComponentModel.DataAnnotations;

namespace NotificationSchedulingSystem.Business.Models;

public class CompanyRequest
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public int Number { get; set; }
    [Required] 
    public string CompanyType { get; set; } = null!;
    [Required]
    public string Market { get; set; } = null!;
}