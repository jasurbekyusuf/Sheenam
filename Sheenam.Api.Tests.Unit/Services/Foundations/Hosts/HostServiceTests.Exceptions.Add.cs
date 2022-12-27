//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();
            SqlException sqlException = CreateSqlException();
            var failedHostStorageException = new FailedHostStorageException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);
            this.storageBrokerMock.Setup(broker =>
                broker.InsertHostAsync(It.IsAny<Host>())).ThrowsAsync(sqlException);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(addHostTask.AsTask);

            // then
            actualHostDependencyException.Should().BeEquivalentTo(expectedHostDependencyException);
            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsHostException =
                new AlreadyExistsHostException(duplicateKeyException);

            var expectedHostDependencyValidationException =
                new HostDependencyValidationException(alreadyExistsHostException);

            this.storageBrokerMock.Setup(broker => broker.InsertHostAsync(It.IsAny<Host>()))
                .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(someHost);

            HostDependencyValidationException actualHostDependencyValidationException =
                await Assert.ThrowsAsync<HostDependencyValidationException>(addHostTask.AsTask);

            // then
            actualHostDependencyValidationException.Should().BeEquivalentTo(
                expectedHostDependencyValidationException);

            this.storageBrokerMock.Verify(broker => broker.InsertHostAsync(
                It.IsAny<Host>()), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedHostDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedHostException = new LockedHostException(dbUpdateConcurrencyException);
            var expectedHostDependencyValidationException = new HostDependencyValidationException(lockedHostException);

            this.storageBrokerMock.Setup(broker => broker.InsertHostAsync(It.IsAny<Host>()))
                .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(someHost);

            HostDependencyValidationException actualHostDependencyValidationException =
                 await Assert.ThrowsAsync<HostDependencyValidationException>(addHostTask.AsTask);

            // then
            actualHostDependencyValidationException.Should().BeEquivalentTo(expectedHostDependencyValidationException);

            this.storageBrokerMock.Verify(broker => broker.InsertHostAsync(It.IsAny<Host>()), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedHostDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();
            var serviceException = new Exception();

            var failedHostServiceException =
                new FailedHostServiceException(serviceException);

            var expectedHostServiceException =
                new HostServiceException(failedHostServiceException);

            this.storageBrokerMock.Setup(broker => broker.InsertHostAsync(It.IsAny<Host>()))
                .ThrowsAsync(serviceException);

            // when
            ValueTask<Host> addHostTask =
                this.hostService.AddHostAsync(someHost);

            HostServiceException actualHostServiceException =
                await Assert.ThrowsAsync<HostServiceException>(addHostTask.AsTask);

            // then
            actualHostServiceException.Should().BeEquivalentTo(
                expectedHostServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostServiceException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
