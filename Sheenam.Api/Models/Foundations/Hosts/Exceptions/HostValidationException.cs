//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class HostValidationException : Xeption
    {
        public HostValidationException(Xeption innerException)
            : base(message: "Host validation error occured, fix the errors and try again.", innerException)
        { }
    }
}
