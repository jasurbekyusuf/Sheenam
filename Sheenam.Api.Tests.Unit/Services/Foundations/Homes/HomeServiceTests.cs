//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using Moq;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Services.Foundations.Homes;
using Tynamix.ObjectFiller;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IHomeService homeService;

        public HomeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.homeService=new HomeService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private DateTimeOffset GetRandomDateTimeOffset() =>
          new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Home CreateRandomHome() =>
            CreateHomeFiller(dates: GetRandomDateTimeOffset()).Create();

        private Filler<Home> CreateHomeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Home>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
