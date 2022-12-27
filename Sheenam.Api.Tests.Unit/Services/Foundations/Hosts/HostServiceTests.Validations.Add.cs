//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Host noHost = null;
            var nullHostException = new NullHostException();

            var expectedHostValidationException =
                new HostValidationException(nullHostException);

            // when
            ValueTask<Host> addHostTask =
                this.hostService.AddHostAsync(noHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(addHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(
                expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnAddIfHostIsInvalidAndLogItAsync(
            string invalidString)
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
               values: "Value is required");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(addHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset anotherRandomDate = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(randomDateTime);
            Host invalidHost = randomHost;
            invalidHost.UpdatedDate = anotherRandomDate;
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.CreatedDate),
                values: $"Date is not same as {nameof(Host.UpdatedDate)}");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(addHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(
                It.Is(SameExceptionAs(expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertHostAsync(
                It.IsAny<Host>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int invalidSeconds)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset invalidRandomDateTime = randomDateTime.AddSeconds(invalidSeconds);
            Host randomInvalidHost = CreateRandomHost(invalidRandomDateTime);
            Host invalidHost = randomInvalidHost;
            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.CreatedDate),
                values: "Date is not recent");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(addHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(
                expectedHostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfEnumsAreInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomHost(randomDateTime);
            Host invalidHost = randomHost;
            invalidHost.GenderType = GetInvalidEnum<GenderType>();
            var invalidHostException = new InvalidHostException();
            invalidHostException.AddData(
                key: nameof(Host.GenderType), values: "Value is not recognized");

            var expectedHostValidationException = new HostValidationException(invalidHostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Host> addHostTask = this.hostService.AddHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(addHostTask.AsTask);

            // then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(
                It.Is(SameExceptionAs(expectedHostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertHostAsync(
                It.IsAny<Host>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
