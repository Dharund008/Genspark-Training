using System;
using EventBookingApi.Interface;
using EventBookingApi.Misc;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Service;

public class UserWalletService : IUserWalletService
{
    private readonly IRepository<int, UserWallet> _walletRepository;

    public UserWalletService(IRepository<int, UserWallet> walletRepository)
    {
        _walletRepository = walletRepository;
    }
    public async Task<bool> AddToWallet(Guid userId, decimal amount)
    {
        var wallet = await GetWalletByUserId(userId);
        if (wallet == null)
        {
            return false;
        }

        if (IsWalletExpired(wallet))
        {
            return false;
        }

        wallet.WalletBalance += amount;
        wallet.WalletBalanceExpiry = DateTime.UtcNow.AddDays(15); //updating epxiration for every wallet transaction(add).
        await _walletRepository.Update(wallet.Id, wallet);
        return true;
    }

    public async Task<UserWallet> CreateWalletForUser(User user)
    {
        var wallet = new UserWallet
        {
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username,
            Role = user.Role,
            //WalletBalance = 100m,
            WalletBalanceExpiry = DateTime.UtcNow.AddDays(15) //AddMonths(6);
        };
        return await _walletRepository.Add(wallet);
    }

    public async Task<bool> DeductFromWallet(Guid userId, decimal amount)
    {
        var wallet = await GetWalletByUserId(userId);
        if (wallet == null)
        {
            return false;
        }

        if (IsWalletExpired(wallet))
        {
            return false;
        }
        if (wallet.WalletBalance < amount)
        {
            return false;
        }

        wallet.WalletBalance -= amount;
        await _walletRepository.Update(wallet.Id, wallet);
        return true;
    }

    public bool IsWalletExpired(UserWallet wallet)
    {
        return wallet.WalletBalanceExpiry.HasValue && wallet.WalletBalanceExpiry < DateTime.UtcNow;
    }

    public async Task<UserWallet> GetWalletByUserId(Guid userId)
    {
        var all = await _walletRepository.GetAll();
        return all.FirstOrDefault(w => w.UserId == userId);
    }

}