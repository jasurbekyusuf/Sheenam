//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedHostServiceException = new FailedHostServiceException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllHosts()).Throws(sqlException);

            // when
            Action retrieveAllHostAction = () =>
                this.hostService.RetrieveAllHosts();

            HostDependencyException actualHostDependencyException =
                Assert.Throws<HostDependencyException>(retrieveAllHostAction);

            // then
            actualHostDependencyException.Should().BeEquivalentTo(expectedHostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllHosts(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedHostDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
