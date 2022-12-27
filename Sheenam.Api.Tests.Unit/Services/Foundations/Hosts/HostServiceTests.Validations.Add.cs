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
    }
}
