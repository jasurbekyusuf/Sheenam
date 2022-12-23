//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Home> InsertHometAsync(Home home);
        IQueryable<Home> SelectAllHomes();
        ValueTask<Home> SelectHometByIdAsync(Guid id);
        ValueTask<Home> UpdateHomeAsync(Home home);
        ValueTask<Home> DeleteHomeAsync(Home home);
    }
}
