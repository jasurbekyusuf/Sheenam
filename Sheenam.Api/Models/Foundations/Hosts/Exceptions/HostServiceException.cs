//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class HostServiceException : Xeption
    {
        public HostServiceException(Exception innerException)
            : base(message: "Host service error occurred, contact support.", innerException)
        { }
    }
}
