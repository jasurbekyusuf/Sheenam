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
        public async Task ShouldModifyHostAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomModifyHost(randomDate);
            Host inputHost = randomHost;
            Host storageHost = inputHost.DeepClone();
            storageHost.UpdatedDate = randomHost.CreatedDate;
            Host updatedHost = inputHost;
            Host exceptedHost = updatedHost.DeepClone();
            Guid hostId = inputHost.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(hostId))
                    .ReturnsAsync(storageHost);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateHostAsync(inputHost))
                    .ReturnsAsync(updatedHost);

            //when
            Host actualHost =
                await this.hostService.ModifyHostAsync(inputHost);

            //then
            actualHost.Should().BeEquivalentTo(exceptedHost);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(hostId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(inputHost), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
