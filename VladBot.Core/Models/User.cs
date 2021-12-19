using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VladBot.Core.Enums;

namespace VladBot.Core.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }
    public State State { get; set; }
    public bool IsAdmin { get; set; }
}