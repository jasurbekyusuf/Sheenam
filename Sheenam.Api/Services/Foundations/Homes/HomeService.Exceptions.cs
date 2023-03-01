using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Homes;
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
        }

        private HomeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var homeValidationException = new HomeValidationException(exception);
            this.loggingBroker.LogError(homeValidationException);

            return homeValidationException;
        }
    }
}
