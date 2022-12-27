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
        private static void ValidateHost(Host host)
        {
            ValidateHostNotNull(host);
            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(Host.Id)),
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(Host.FirstName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(Host.LastName)),
                (Rule: IsInvalid(host.DateOfBirth), Parameter: nameof(Host.DateOfBirth)),
                (Rule: IsInvalid(host.Email), Parameter: nameof(Host.Email)),
                (Rule: IsInvalid(host.PhoneNumber), Parameter: nameof(Host.PhoneNumber)),
                (Rule: IsInvalid(host.CreatedDate), Parameter: nameof(Host.CreatedDate)),
                (Rule: IsInvalid(host.UpdatedDate), Parameter: nameof(Host.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: host.CreatedDate,
                    secondDate: host.UpdatedDate,
                    secondDateName: nameof(host.UpdatedDate)),

                Parameter: nameof(host.CreatedDate)));
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

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
