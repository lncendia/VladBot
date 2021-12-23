using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VladBot.Core.Enums;

namespace VladBot.Core.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public State State { get; set; } = State.Main;
    public bool IsAdmin { get; set; } = false;
}