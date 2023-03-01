﻿//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class FailedHomeStorageException : Xeption
    {
        public FailedHomeStorageException(Exception innerException)
            : base(message: "Failed host storage error occurred, contact support.", innerException)
        { }
    }
}
