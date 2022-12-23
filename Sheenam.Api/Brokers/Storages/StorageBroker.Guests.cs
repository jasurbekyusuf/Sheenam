//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest) =>
            await InsertAsync(guest);
    }
}
