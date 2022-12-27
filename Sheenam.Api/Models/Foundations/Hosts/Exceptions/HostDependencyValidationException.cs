//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class HostDependencyValidationException : Xeption
    {
        public HostDependencyValidationException(Xeption innerException)
            : base(message: "Host dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
