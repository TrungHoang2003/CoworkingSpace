using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class PaymentInfo
{
    [Key] public int Id { get; set; }

    public int UserId { get; set; }
    [ForeignKey("UserId")] public User User { get; set; }

    public string FullName { get; set; }
    public string BankCode { get; set; }
    public string AccountNumber { get; set; }
    public string IdentityNumber { get; set; }
    public string Note { get; set; } 
}