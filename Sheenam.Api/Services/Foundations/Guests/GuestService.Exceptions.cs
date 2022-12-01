//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
            catch (InvalidGuestException invalidGuestException)
            {
                throw CreateAndLogValidationException(invalidGuestException);
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException = new FailedGuestStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedGuestStorageException);
            }
        }

        private GuestValidationException CreateAndLogValidationException(Xeption exeption)
        {
            var guestValidationException =
                new GuestValidationException(exeption);

                this.loggingBroker.LogError(guestValidationException);

            return guestValidationException;
        }

        private GuestDependencyException CreateAndLogCriticalDependencyException(Xeption exeption)
        {
            var guesDepencyException = new GuestDependencyException(exeption);
            this.loggingBroker.LogCritical(guesDepencyException);
            return guesDepencyException;
        }
    }
}
