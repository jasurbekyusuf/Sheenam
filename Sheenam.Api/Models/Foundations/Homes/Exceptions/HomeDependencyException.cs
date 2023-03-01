//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class HomeDependencyException : Xeption
    {
        public HomeDependencyException(Xeption innerException)
            : base(message: "Host dependency error occurred, contact support.", innerException)
        { }
    }
}
