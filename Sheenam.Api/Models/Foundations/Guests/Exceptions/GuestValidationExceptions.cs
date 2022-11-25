//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestValidationExceptions : Xeption
    {
        public GuestValidationExceptions(Xeption innerException)
            : base(message: "Guest valdation error occured, fix the errors and try again",
                  innerException)
        { }
    }
}
