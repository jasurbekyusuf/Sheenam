//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Home> Homes { get; set; }

        public async ValueTask<Home> InsertHomeAsync(Home home) =>
            await InsertAsync(home);
    }
}
