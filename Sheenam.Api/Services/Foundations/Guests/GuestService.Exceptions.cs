//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private delegate ValueTask<Guest> ReturningGuestFunction();

        private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
        {
            try
            {
                return await returningGuestFunction();
            }
            catch (NullGuestException nullGuestException)
            {
                

                throw CreateAndLogValidationException(nullGuestException);
            }
        }
        private GuestValidationException CreateAndLogValidationException(Xeption xeption)
        {
            var guestValidationException =
                new GuestValidationException(nullGuestException);

                this.loggingBroker.LogError(guestValidationException);
        }
    }
}
