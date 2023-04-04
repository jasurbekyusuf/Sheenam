//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Homes
{
    public partial class HomeService
    {
        private delegate IQueryable<Home> ReturningHomesFunction();
        private delegate ValueTask<Home> ReturningHomeFunction();

        private async ValueTask<Home> TryCatch(ReturningHomeFunction returningHomeFunction)
        {
            try
            {
                return await returningHomeFunction();
            }
            catch (NullHomeException nullHomeException)
            {
                throw CreateAndLogValidationException(nullHomeException);
            }
            catch (InvalidHomeException invalidHomeException)
            {
                throw CreateAndLogValidationException(invalidHomeException);
            }
            catch (NotFoundHomeException notFoundHomeException)
            {
                throw CreateAndLogValidationException(notFoundHomeException);
            }
        }

        private HomeDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var homeDependencyException = new HomeDependencyException(exception);
            this.loggingBroker.LogError(homeDependencyException);

            return homeDependencyException;
        }

        private HomeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var homeValidationException = new HomeValidationException(exception);
            this.loggingBroker.LogError(homeValidationException);

            return homeValidationException;
        }

        private HomeServiceException CreateAndLogServiceException(Xeption exception)
        {
            var homeserviceException = new HomeServiceException(exception);
            this.loggingBroker.LogError(homeserviceException);

            return homeserviceException;
        }
    }
}
