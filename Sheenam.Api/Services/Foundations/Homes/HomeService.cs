//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Services.Foundations.Homes
{
    public class HomeService:IHomeService
    {
        private IStorageBroker storageBroker;

        public HomeService(IStorageBroker storageBroker)=>
            this.storageBroker = storageBroker;
        public ValueTask<Home> RemoveHomeByIdAsync(Guid guestId)=>
            throw new NotImplementedException();
    }
}
