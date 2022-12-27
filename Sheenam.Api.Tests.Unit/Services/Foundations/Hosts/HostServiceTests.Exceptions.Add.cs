//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System.Net.Sockets;
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
            
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(addHostTask.AsTask);

            // then
            actualHostDependencyException.Should().BeEquivalentTo(expectedHostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTime())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(someHost);

            HostDependencyValidationException actualHostDependencyValidationException =
                await Assert.ThrowsAsync<HostDependencyValidationException>(addHostTask.AsTask);

            // then
            actualHostDependencyValidationException.Should().BeEquivalentTo(
                expectedHostDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedHostDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertHostAsync(
                It.IsAny<Host>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(someHost);

            HostDependencyValidationException actualHostDependencyValidationException =
                 await Assert.ThrowsAsync<HostDependencyValidationException>(addHostTask.AsTask);

            // then
            actualHostDependencyValidationException.Should().BeEquivalentTo(expectedHostDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedHostDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(serviceException);

            // when
            ValueTask<Host> addHostTask =
                this.hostService.AddHostAsync(someHost);

            HostServiceException actualHostServiceException =
                await Assert.ThrowsAsync<HostServiceException>(addHostTask.AsTask);

            // then
            actualHostServiceException.Should().BeEquivalentTo(
                expectedHostServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostServiceException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
