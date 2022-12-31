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
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private delegate IQueryable<Host> ReturningHostsFunction();
        private delegate ValueTask<Host> ReturningHostFunction();

        private async ValueTask<Host> TryCatch(ReturningHostFunction returningHostFunction)
        {
            try
            {
                return await returningHostFunction();
            }
            catch (NullHostException nullHostException)
            {
                throw CreateAndLogValidationException(nullHostException);
            }
            catch (InvalidHostException invalidHostException)
            {
                throw CreateAndLogValidationException(invalidHostException);
            }
            catch (NotFoundHostException notFoundHostException)
            {
                throw CreateAndLogValidationException(notFoundHostException);
            }
            catch (SqlException sqlException)
            {
                var failedHostStorageException = new FailedHostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedHostStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsHostException =
                     new AlreadyExistsHostException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsHostException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedHostException = new LockedHostException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedHostException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedHostStorageException = new FailedHostStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedHostStorageException);
            }
            catch (Exception serviceException)
            {
                var failedServiceHostException = new FailedHostServiceException(serviceException);

                throw CreateAndLogServiceException(failedServiceHostException);
            }
        }

        private IQueryable<Host> TryCatch(ReturningHostsFunction returningHostsFunction)
        {
            try
            {
                return returningHostsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedServiceHostException = new FailedHostServiceException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedServiceHostException);
            }
            catch (Exception serviException)
            {
                var failedServiceHostException = new FailedHostServiceException(serviException);

                throw CreateAndLogServiceException(failedServiceHostException);
            }
        }

        private HostValidationException CreateAndLogValidationException(Xeption exception)
        {
            var hostValidationException = new HostValidationException(exception);
            this.loggingBroker.LogError(hostValidationException);

            return hostValidationException;
        }

        private HostServiceException CreateAndLogServiceException(Xeption exception)
        {
            var hostserviceException = new HostServiceException(exception);
            this.loggingBroker.LogError(hostserviceException);

            return hostserviceException;
        }

        private HostDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var hostDependencyException = new HostDependencyException(exception);
            this.loggingBroker.LogError(hostDependencyException);

            return hostDependencyException;
        }

        private HostDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var hostDependencyException = new HostDependencyException(exception);
            this.loggingBroker.LogCritical(hostDependencyException);

            return hostDependencyException;
        }

        private HostDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var hostDependencyValidationException = new HostDependencyValidationException(exception);
            this.loggingBroker.LogError(hostDependencyValidationException);

            return hostDependencyValidationException;
        }
    }
}
