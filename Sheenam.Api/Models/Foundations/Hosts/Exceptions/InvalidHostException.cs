//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions
{
    public class InvalidHostException : Xeption
    {
        public InvalidHostException() : base(message: "Host is invalid.")
        { }
    }
}
