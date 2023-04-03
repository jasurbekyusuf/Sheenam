//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Homes.Exceptions
{
    public class NullHomeException : Xeption
    {
        public NullHomeException()
            : base(message: "Hpst is null.")
        { }
    }
}
