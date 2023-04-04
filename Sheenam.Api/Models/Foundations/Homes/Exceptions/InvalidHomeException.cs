//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Homes.Exceptions
{
    public class InvalidHomeException : Xeption
    {
        public InvalidHomeException() : base(message: "Home is invalid.")
        { }
    }
}
