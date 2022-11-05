//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================



using System;

namespace Sheenam.Api.Models.Foundations.Guests
{
    public class Guest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Adrees { get; set; }
        public  GenderType Gender { get; set; }
    }
}
