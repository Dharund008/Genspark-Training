using System;
using System.ComponentModel.DataAnnotations;

namespace EventBookingApi.Model;
public class UserWallet
{
    [Key]
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Username { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

    // New wallet balance property
    public decimal WalletBalance { get; set; } = 100m;

    //wallet balance expiration date
    public DateTime? WalletBalanceExpiry { get; set; }
}
