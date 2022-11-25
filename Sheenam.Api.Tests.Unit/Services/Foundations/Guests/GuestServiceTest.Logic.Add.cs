//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTest
    {
        [Fact]
        public async Task ShouldAddGuestAsync()
        {
            Guest randomGuest = CreateRandomGuest();
            Guest inputGuest = randomGuest;
            Guest returningGuest = inputGuest;
            Guest expectedGuest = returningGuest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuestAsync(inputGuest))
                    .ReturnsAsync(returningGuest);

            Guest actualGuest =
                await this.guestService.AddGuestAsync(inputGuest);

            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker=>
                broker.InsertGuestAsync(inputGuest),
                Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
