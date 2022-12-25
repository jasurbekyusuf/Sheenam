//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class LockedGuestException : Xeption
    {
        public LockedGuestException(Exception innerException)
            : base(message: "Guest is locked, please try again.", innerException)
        { }
    }
}