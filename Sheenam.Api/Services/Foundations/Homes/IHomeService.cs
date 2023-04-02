//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Services.Foundations.Homes
{
    public interface IHomeService
    {
        ValueTask<Home> RetrieveHomeByIdAsync(Guid homeId);
    }
}
