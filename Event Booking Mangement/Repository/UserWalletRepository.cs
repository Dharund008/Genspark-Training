using System;
using EventBookingApi.Context;
using EventBookingApi.Interface;
using EventBookingApi.Model;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApi.Repository;

public class UserWalletRepository : Repository<int, UserWallet>
{

    public UserWalletRepository(EventContext context):base(context)
    {
        
    }

    public override async Task<UserWallet> GetById(int id)
    {
        return await _eventContext.UserWallets.FindAsync(id);
    }

    public override  async Task<IEnumerable<UserWallet>> GetAll()
    {
        return await _eventContext.UserWallets.ToListAsync();
    }

}
