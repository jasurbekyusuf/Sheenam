//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(someDateTime);
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            SqlException sqlException = CreateSqlException();

            var failedHostStorageException =
                new FailedHostStorageException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(
                     modifyHostTask.AsTask);

            // then
            actualHostDependencyException.Should().BeEquivalentTo(
                expectedHostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(hostId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(someHost), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(randomDateTime);
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            someHost.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedHostException =
                new FailedHostStorageException(databaseUpdateException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(hostId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                 await Assert.ThrowsAsync<HostDependencyException>(
                     modifyHostTask.AsTask);

            // then
            actualHostDependencyException.Should().BeEquivalentTo(
                expectedHostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(hostId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(randomDateTime);
            Host someHost = randomHost;
            someHost.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid hostId = someHost.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedHostException =
                new LockedHostException(databaseUpdateConcurrencyException);

            var expectedHostDependencyValidationException =
                new HostDependencyValidationException(lockedHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(hostId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostDependencyValidationException actualHostDependencyValidationException =
                await Assert.ThrowsAsync<HostDependencyValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostDependencyValidationException.Should().BeEquivalentTo(
                expectedHostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(hostId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minuteInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(randomDateTime);
            Host someHost = randomHost;
            someHost.CreatedDate = randomDateTime.AddMinutes(minuteInPast);
            var serviceException = new Exception();

            var failedHostException =
                new FailedHostServiceException(serviceException);

            var expectedHostServiceException =
                new HostServiceException(failedHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(someHost.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostServiceException actualHostServiceException =
                await Assert.ThrowsAsync<HostServiceException>(
                    modifyHostTask.AsTask);

            // then
            actualHostServiceException.Should().BeEquivalentTo(
                expectedHostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(someHost.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
