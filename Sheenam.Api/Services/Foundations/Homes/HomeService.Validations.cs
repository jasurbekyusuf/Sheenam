//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;

namespace Sheenam.Api.Services.Foundations.Homes
{
    public partial class HomeService
    {
        public void ValidateHome(Home home)
        {
            ValidateHomeNotNull(home);

            Validate(
                (Rule: IsInvalid(home.Id), Parameter: nameof(Home.Id)),
                (Rule: IsInvalid(home.HostId), Parameter: nameof(Home.HostId)),
                (Rule: IsInvalid(home.Adress), Parameter: nameof(Home.Adress)),
                (Rule: IsInvalid(home.AdditionalInfo), Parameter: nameof(Home.AdditionalInfo)),
                (Rule: IsInvalid(home.IsVacant), Parameter: nameof(Home.IsVacant)),
                (Rule: IsInvalid(home.IsPetAllowed), Parameter: nameof(Home.IsPetAllowed)),
                (Rule: IsInvalid(home.IsShared), Parameter: nameof(Home.IsShared)),
                (Rule: IsInvalid(home.NumberOfBedrooms), Parameter: nameof(Home.NumberOfBedrooms)),
                (Rule: IsInvalid(home.NumberOfBathrooms), Parameter: nameof(Home.NumberOfBathrooms)),
                (Rule: IsInvalid(home.Area), Parameter: nameof(Home.Area)),
                (Rule: IsInvalid(home.Price), Parameter: nameof(Home.Price)),
                (Rule: IsInvalid(home.Type), Parameter: nameof(Home.Type)),
                (Rule: IsInvalid(home.UpdatedDate), Parameter: nameof(Home.UpdatedDate)),
                (Rule: IsNotRecent(home.CreatedDate), Parameter: nameof(Home.CreatedDate)),
                (Rule: IsInvalid(home.UpdatedDate), Parameter: nameof(Home.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: home.UpdatedDate,
                    secondDate: home.CreatedDate,
                    secondDateName: nameof(Home.CreatedDate)),

                Parameter: nameof(Home.UpdatedDate)));
        }

        private void ValidateHomeId(Guid HomeId) =>
            Validate((Rule: IsInvalid(HomeId), Parameter: nameof(Home.Id)));

        private static void ValidateStorageHomeExists(Home maybeHome, Guid homeId)
        {
            if (maybeHome is null)
            {
                throw new NotFoundHomeException(homeId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static dynamic IsInvalid<T>(T value) => new
        {
            Condition = IsEnumInvalid(value),
            Message = "Value is not recognized"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static bool IsEnumInvalid<T>(T value)
        {
            bool isDefined = Enum.IsDefined(typeof(T), value);

            return isDefined is false;
        }

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void ValidateHomeNotNull(Home home)
        {
            if (home is null)
            {
                throw new NullHomeException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHomeException = new InvalidHomeException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHomeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHomeException.ThrowIfContainsErrors();
        }
    }
}
