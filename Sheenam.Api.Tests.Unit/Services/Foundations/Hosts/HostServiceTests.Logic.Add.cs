//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldAddHostAsync()
        {
            // given
            Host randomHost = CreateRandomHost();
            Host inputHost = randomHost;
            Host persistedHost = inputHost;
            Host expectedHost = persistedHost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHostAsync(inputHost))
                    .ReturnsAsync(persistedHost);

            // When
            Host actualHost = await this.hostService.AddHostAsync(inputHost);

            // Then
            actualHost.Should().BeEquivalentTo(expectedHost);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(inputHost), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
