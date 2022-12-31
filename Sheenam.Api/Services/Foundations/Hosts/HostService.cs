//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService : IHostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public HostService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Host> AddHostAsync(Host host) =>
        TryCatch(async () =>
        {
            ValidateHost(host);

            return await this.storageBroker.InsertHostAsync(host);
        });

        public IQueryable<Host> RetrieveAllHosts() =>
        TryCatch(() => this.storageBroker.SelectAllHosts());

        public ValueTask<Host> RetrieveHostByIdAsync(Guid hostId) =>
        TryCatch(async () =>
        {
            ValidateHostId(hostId);

            Host maybeHost =
                await storageBroker.SelectHostByIdAsync(hostId);

            ValidateStorageHostExists(maybeHost, hostId);

            return maybeHost;
        });

        public ValueTask<Host> ModifyHostAsync(Host host) =>
        TryCatch(async () =>
        {
            ValidateHostOnModify(host);

            var maybeHost = await this.storageBroker.SelectHostByIdAsync(host.Id);

            ValidateStorageHostOnModify(inputHost: host, storageHost: maybeHost);

            return await this.storageBroker.UpdateHostAsync(host);
        });

        public ValueTask<Host> RemoveHostByIdAsync(Guid hostId) =>
        TryCatch(async () =>
        {
            ValidateHostId(hostId);

            Host maybeHost = await this.storageBroker.SelectHostByIdAsync(hostId);

            ValidateStorageHostExists(maybeHost, hostId);

            return await this.storageBroker.DeleteHostAsync(maybeHost);
        });
    }
}
