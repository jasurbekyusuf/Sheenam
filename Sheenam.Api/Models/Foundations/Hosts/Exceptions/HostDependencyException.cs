//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class HostDependencyException : Xeption
    {
        public HostDependencyException(Xeption innerException)
            : base(message: "Host dependency error occurred, contact support.", innerException)
        { }
    }
}
