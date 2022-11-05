//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use Comfort and Pease
//===================================================




using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest>Guests {get; set;}
    }
}
