//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostIsNullAndLogItAsync()
        {
            //given
            Host nullHost = null;
            var nullHostException = new NullHostException();

            var expectedHostValidationException =
                new HostValidationException(nullHostException);

            //when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(nullHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(modifyHostTask.AsTask);

            //then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))), Times.Once());
            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()), Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostIsInvalidAndLogItAsync(string invalidString)
        {
            // given 
            var invalidHost = new Host
            {
                FirstName = invalidString
            };

            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.Id),
                values: "Id is required");

            invalidHostException.AddData(
                key: nameof(Host.FirstName),
                values: "Text is required");

            invalidHostException.AddData(
                key: nameof(Host.LastName),
                values: "Text is required");

            invalidHostException.AddData(
                key: nameof(Host.DateOfBirth),
                values: "Value is required");

            invalidHostException.AddData(
                key: nameof(Host.Email),
                values: "Text is required");

            invalidHostException.AddData(
                key: nameof(Host.PhoneNumber),
                values: "Text is required");

            invalidHostException.AddData(
               key: nameof(Host.CreatedDate),
               values: "Value is required");

            invalidHostException.AddData(
                key: nameof(Host.UpdatedDate),
                    values: new[]
                    {
                        "Value is required",
                        "Date is not recent",
                        $"Date is the same as {nameof(Host.CreatedDate)}"
                    }
                );

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(GetRandomDateTimeOffset);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            //then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(randomDateTime);
            Host invalidHost = randomHost;
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.UpdatedDate),
                values: $"Date is the same as {nameof(Host.CreatedDate)}");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(dateTime);
            Host inputHost = randomHost;
            inputHost.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.UpdatedDate),
                values: "Date is not recent");

            var expectedHostValidatonException =
                new HostValidationException(invalidHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(dateTime);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(inputHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidatonException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(dateTime);
            Host nonExistHost = randomHost;
            nonExistHost.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Host nullHost = null;

            var notFoundHostException =
                new NotFoundHostException(nonExistHost.Id);

            var expectedHostValidationException =
                new HostValidationException(notFoundHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(nonExistHost.Id)).ReturnsAsync(nullHost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(dateTime);

            // when 
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(nonExistHost);

            HostValidationException actualHostValidationException =
               await Assert.ThrowsAsync<HostValidationException>(modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(nonExistHost.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomModifyHost(randomDateTime);
            Host invalidHost = randomHost.DeepClone();
            Host storageHost = invalidHost.DeepClone();
            storageHost.CreatedDate = storageHost.CreatedDate.AddMinutes(randomMinutes);
            storageHost.UpdatedDate = storageHost.UpdatedDate.AddMinutes(randomMinutes);
            var invalidHostException = new InvalidHostException();
            Guid hostId = invalidHost.Id;

            invalidHostException.AddData(
                key: nameof(Host.CreatedDate),
                values: $"Date is not same as {nameof(Host.CreatedDate)}");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(hostId)).ReturnsAsync(storageHost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(invalidHost.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomModifyHost(randomDateTime);
            Host invalidHost = randomHost;
            Host storageHost = randomHost.DeepClone();
            invalidHost.UpdatedDate = storageHost.UpdatedDate;
            Guid HostId = invalidHost.Id;
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
            key: nameof(Host.UpdatedDate),
                values: $"Date is the same as {nameof(Host.UpdatedDate)}");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectHostByIdAsync(invalidHost.Id)).ReturnsAsync(storageHost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> modifyHostTask = this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
               await Assert.ThrowsAsync<HostValidationException>(modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(HostId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
