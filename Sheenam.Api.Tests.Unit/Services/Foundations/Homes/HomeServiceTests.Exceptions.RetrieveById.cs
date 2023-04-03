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
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Homes
{
    public  partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedHomeStorageException =
                new FailedHomeStorageException(sqlException);

            var expectedHomeDependencyException =
                new HomeDependencyException(failedHomeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

            // when
            ValueTask<Home> retrieveHomeByIdTask =
                this.homeService.RetrieveHomeByIdAsync(someId);

            HomeDependencyException actaulHomeDependencyException =
                await Assert.ThrowsAsync<HomeDependencyException>(
                    retrieveHomeByIdTask.AsTask);

            // then
            actaulHomeDependencyException.Should().BeEquivalentTo(
                expectedHomeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHomeByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHomeDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
