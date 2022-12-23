//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllGuests()
        {
            // given
            IQueryable<Guest> randomGuests = CreateRandomGuests();
            IQueryable<Guest> storageGuests = randomGuests;
            IQueryable<Guest> expectedGuests = storageGuests;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests()).Returns(storageGuests);

            // when
            IQueryable<Guest> actualGuest =
                this.guestService.RetrieveAllGuests();

            // then
            actualGuest.Should().BeEquivalentTo(expectedGuests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
