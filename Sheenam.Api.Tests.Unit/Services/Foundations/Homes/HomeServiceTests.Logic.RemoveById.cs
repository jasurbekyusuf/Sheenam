//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Homes;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldRemoveHomeById()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputHomeId = randomId;
            Home randomHome = CreateRandomHome();
            Home storageHome = randomHome;
            Home expectedInputHome = storageHome;
            Home deletedHome = expectedInputHome;
            Home expectedHome = deletedHome.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHometByIdAsync(inputHomeId))
                    .ReturnsAsync(storageHome);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteHomeAsync(expectedInputHome))
                    .ReturnsAsync(deletedHome);

            // when
            Home actualHome = await this.homeService
                .RemoveHomeByIdAsync(inputHomeId);

            // then
            actualHome.Should().BeEquivalentTo(expectedHome);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHometByIdAsync(inputHomeId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHomeAsync(expectedInputHome), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }

}
