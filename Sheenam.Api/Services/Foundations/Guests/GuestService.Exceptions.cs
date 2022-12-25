//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private delegate IQueryable<Guest> ReturningGuestsFunction();
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
            catch (NotFoundGuestException notFoundGuestException)
            {
                throw CreateAndLogValidationException(notFoundGuestException);
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException = new FailedGuestStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedGuestStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistGuestException =
                    new AlreadyExistGuestException(duplicateKeyException);

                throw CreateAndLogDependencyValidation(alreadyExistGuestException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedGuestException = new LockedGuestException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedGuestException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedGuestStorageException = new FailedGuestStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedGuestStorageException);
            }
            catch (Exception exception)
            {
                var failedGuestServiceException =
                    new FailedGuestServiceException(exception);

                throw CreateAndLogServiceException(failedGuestServiceException);
            }
        }

        private IQueryable<Guest> TryCatch(ReturningGuestsFunction returningGuestsFunction)
        {
            try
            {
                return returningGuestsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedGuestServiceException = new FailedGuestServiceException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGuestServiceException);
            }
            catch (Exception serviException)
            {
                var failedGuestServiceException = new FailedGuestServiceException(serviException);

                throw CreateAndLogServiceException(failedGuestServiceException);
            }
        }

        private GuestDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var guestDependencyException = new GuestDependencyException(exception);
            this.loggingBroker.LogError(guestDependencyException);

            return guestDependencyException;
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

        private GuestDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var guestDependencyValidationException = new GuestDependencyValidationException(exception);
            this.loggingBroker.LogError(guestDependencyValidationException);

            return guestDependencyValidationException;
        }

        private GuestDependencyValidationException CreateAndLogDependencyValidation(
            Xeption exeption)
        {
            var guestDependencyValidationException =
                new GuestDependencyValidationException(exeption);

            this.loggingBroker.LogError(guestDependencyValidationException);

            return guestDependencyValidationException;
        }

        private GuestServiceException CreateAndLogServiceException(Xeption exception)
        {
            GuestServiceException guestServiceException = new GuestServiceException(exception);
            this.loggingBroker.LogError(guestServiceException);

            return guestServiceException;
        }
    }
}
