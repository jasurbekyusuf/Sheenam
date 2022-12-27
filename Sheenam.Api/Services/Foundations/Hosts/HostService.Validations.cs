//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private static void ValidateHostNotNull(Host host)
        {
            if (host is null)
            {
                throw new NullHostException();
            }
        }
    }
}
