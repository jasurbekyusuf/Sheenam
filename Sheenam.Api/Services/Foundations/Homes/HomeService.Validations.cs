using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Services.Foundations.Homes
{
    public partial class HomeService
    {
        private static void ValidateHomeNotNull(Home home)
        {
            if (home is null)
            {
                throw new NullHomeException();
            }
        }
    }
}
