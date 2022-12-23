//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedGuestServiceException = new FailedGuestServiceException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests()).Throws(sqlException);

            // when
            Action retrieveAllGuestAction = () =>
                this.guestService.RetrieveAllGuests();

            GuestDependencyException actualGuestDependencyException =
                Assert.Throws<GuestDependencyException>(retrieveAllGuestAction);

            // then
            actualGuestDependencyException.Should().BeEquivalentTo(expectedGuestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuestDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);
            var failedGuestServiceException = new FailedGuestServiceException(serviceException);

            var expectedGuestServiceException =
                new GuestServiceException(failedGuestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests()).Throws(serviceException);

            // when
            Action retrieveAllGuestAction = () =>
                this.guestService.RetrieveAllGuests();

            GuestServiceException actualGuestServiceException =
                Assert.Throws<GuestServiceException>(retrieveAllGuestAction);

            // then
            actualGuestServiceException.Should().BeEquivalentTo(expectedGuestServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
