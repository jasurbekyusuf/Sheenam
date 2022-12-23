//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestServiceException : Xeption
    {
        public GuestServiceException(Xeption innerException)
            : base(message: "Guest service error occurred, contact support",
                 innerException)
        { }
    }
}
