using System;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Interface;

public interface IUserWalletService
{
    Task<UserWallet> CreateWalletForUser(User user);
    Task<UserWallet> GetWalletByUserId(Guid userId);
    Task<bool> AddToWallet(Guid userId, decimal amount);
    Task<bool> DeductFromWallet(Guid userId, decimal amount);
    bool IsWalletExpired(UserWallet wallet);
}
