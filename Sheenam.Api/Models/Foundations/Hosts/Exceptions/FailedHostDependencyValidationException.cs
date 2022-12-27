﻿//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class FailedHostDependencyValidationException : Xeption
    {
        public FailedHostDependencyValidationException(Exception innerException)
            : base(message: "Failed Host dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
