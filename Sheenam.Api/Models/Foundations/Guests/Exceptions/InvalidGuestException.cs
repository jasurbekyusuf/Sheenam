//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class InvalidGuestException : Xeption
    {
        public InvalidGuestException()
            : base(message: "Guest is invalid")
        { }
    }
}
