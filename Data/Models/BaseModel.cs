using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class BaseModel
{
    [Key]
    public int Id { get; set; }
}