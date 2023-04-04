//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Homes.Exceptions
{
    public class HomeServiceException : Xeption
    {
        public HomeServiceException(Exception innerException)
            : base(message: "Home service error occurred, contact support.", innerException)
        { }
    }
}
