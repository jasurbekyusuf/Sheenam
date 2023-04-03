//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
using Moq;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Services.Foundations.Homes;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IHomeService homeService;

        public HomeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.homeService = new HomeService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static SqlException CreateSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedExceptoin) =>
            actualException => actualException.SameExceptionAs(expectedExceptoin);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 99).GetValue();

        private static Home CreateRandomHome() =>
            CreateHomeFiller(GetRandomDateTimeOffset()).Create();

        private static IQueryable<Home> CreateRandomHomes()
        {
            return CreateHomeFiller(dates: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static Filler<Home> CreateHomeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Home>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
