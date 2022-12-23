//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.HomeRequests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<HomeRequest> HomeRequests { get; set; }

        public async ValueTask<HomeRequest> InsertHomeRequestAsync(HomeRequest homeRequest) =>
            await InsertAsync(homeRequest);
    }
}
