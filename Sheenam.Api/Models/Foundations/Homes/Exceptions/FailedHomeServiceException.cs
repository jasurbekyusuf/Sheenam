//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class FailedHomeServiceException : Xeption
    {
        public FailedHomeServiceException(Exception innerException)
            : base(message: "Failed host service occurred, please contact support",
                  innerException)
        { }
    }
}
