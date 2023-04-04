//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Homes.Exceptions
{
    public class FailedHomeStorageException : Xeption
    {
        public FailedHomeStorageException(Exception innerException)
            : base(message: "Failed home storage error occurred, contact support.", innerException)
        { }
    }
}
