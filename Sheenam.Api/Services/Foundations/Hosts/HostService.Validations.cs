//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private void ValidateHost(Host host)
        {
            ValidateHostNotNull(host);

            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(Host.Id)),
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(Host.FirstName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(Host.LastName)),
                (Rule: IsInvalid(host.DateOfBirth), Parameter: nameof(Host.DateOfBirth)),
                (Rule: IsInvalid(host.Email), Parameter: nameof(Host.Email)),
                (Rule: IsInvalid(host.PhoneNumber), Parameter: nameof(Host.PhoneNumber)),
                (Rule: IsInvalid(host.GenderType), Parameter: nameof(Host.GenderType)),
                (Rule: IsInvalid(host.CreatedDate), Parameter: nameof(Host.CreatedDate)),
                (Rule: IsInvalid(host.UpdatedDate), Parameter: nameof(Host.UpdatedDate)),
                (Rule: IsNotRecent(host.CreatedDate), Parameter: nameof(Host.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: host.CreatedDate,
                    secondDate: host.UpdatedDate,
                    secondDateName: nameof(Host.UpdatedDate)),

                Parameter: nameof(Host.CreatedDate)));
        }

        private void ValidateHostId(Guid HostId) =>
            Validate((Rule: IsInvalid(HostId), Parameter: nameof(Host.Id)));

        private static void ValidateStorageHostExists(Host maybeHost, Guid hostId)
        {
            if (maybeHost is null)
            {
                throw new NotFoundHostException(hostId);
            }
        }

        private static void ValidateStorageHostOnModify(Host inputHost, Host storageHost)
        {
            ValidateStorageHostExists(storageHost, inputHost.Id);

            Validate(
            (Rule: IsNotSame(
                    firstDate: inputHost.CreatedDate,
                    secondDate: storageHost.CreatedDate,
                    secondDateName: nameof(Host.CreatedDate)),
                Parameter: nameof(Host.CreatedDate)),

            (Rule: IsSame(
                        firstDate: inputHost.UpdatedDate,
                        secondDate: storageHost.UpdatedDate,
                        secondDateName: nameof(Host.UpdatedDate)),
                Parameter: nameof(Host.UpdatedDate)));
        }

        private void ValidateHostOnModify(Host host)
        {
            ValidateHostNotNull(host);

            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(Host.Id)),
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(Host.FirstName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(Host.LastName)),
                (Rule: IsInvalid(host.DateOfBirth), Parameter: nameof(Host.DateOfBirth)),
                (Rule: IsInvalid(host.Email), Parameter: nameof(Host.Email)),
                (Rule: IsInvalid(host.PhoneNumber), Parameter: nameof(Host.PhoneNumber)),
                (Rule: IsInvalid(host.GenderType), Parameter: nameof(Host.GenderType)),
                (Rule: IsInvalid(host.CreatedDate), Parameter: nameof(Host.CreatedDate)),
                (Rule: IsInvalid(host.UpdatedDate), Parameter: nameof(Host.UpdatedDate)),
                (Rule: IsNotRecent(host.UpdatedDate), Parameter: nameof(Host.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: host.UpdatedDate,
                    secondDate: host.CreatedDate,
                    secondDateName: nameof(Host.CreatedDate)),

                Parameter: nameof(Host.UpdatedDate)));
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

        private static void ValidateHostNotNull(Host host)
        {
            if (host is null)
            {
                throw new NullHostException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHostException = new InvalidHostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHostException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHostException.ThrowIfContainsErrors();
        }
    }
}
