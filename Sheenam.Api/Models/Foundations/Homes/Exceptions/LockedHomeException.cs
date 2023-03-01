//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class LockedHomeException : Xeption
    {
        public LockedHomeException(Exception innerException)
            : base(message: "Host is locked, please try again.", innerException)
        { }
    }
}
