//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class NullHostException : Xeption
    {
        public NullHostException()
            : base(message: "Hpst is null.")
        { }
    }
}
