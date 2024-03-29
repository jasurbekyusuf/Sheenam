﻿//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions;
using Sheenam.Api.Services.Foundations.Hosts;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : RESTFulController
    {
        private readonly IHostService hostService;

        public HostsController(IHostService hostService) =>
            this.hostService = hostService;

        [HttpPost]
        public async ValueTask<ActionResult<Host>> PostHostAsync(Host host)
        {
            try
            {
                return await this.hostService.AddHostAsync(host);
            }
            catch (HostValidationException hostValidationExpection)
            {
                return BadRequest(hostValidationExpection.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
                when (hostDependencyValidationException.InnerException is AlreadyExistsHostException)
            {
                return Conflict(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
            {
                return BadRequest(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyException hostDependencyException)
            {
                return InternalServerError(hostDependencyException.InnerException);
            }
            catch (HostServiceException hostServiceException)
            {
                return InternalServerError(hostServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Host>> GetAllHosts()
        {
            try
            {
                IQueryable<Host> allHosts = this.hostService.RetrieveAllHosts();

                return Ok(allHosts);
            }
            catch (HostDependencyException hostDependencyException)
            {
                return InternalServerError(hostDependencyException.InnerException);
            }
            catch (HostServiceException hostServiceException)
            {
                return InternalServerError(hostServiceException.InnerException);
            }
        }

        [HttpGet("{hostId}")]
        public async ValueTask<ActionResult<Host>> GetHostByIdAsync(Guid hostId)
        {
            try
            {
                return await this.hostService.RetrieveHostByIdAsync(hostId);
            }
            catch (HostDependencyException hostDependencyException)
            {
                return InternalServerError(hostDependencyException.InnerException);
            }
            catch (HostValidationException hostValidationException)
                when (hostValidationException.InnerException is InvalidHostException)
            {
                return BadRequest(hostValidationException.InnerException);
            }
            catch (HostValidationException hostValidationException)
                when (hostValidationException.InnerException is NotFoundHostException)
            {
                return NotFound(hostValidationException.InnerException);
            }
            catch (HostServiceException hostServiceException)
            {
                return InternalServerError(hostServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Host>> PutHostAsync(Host host)
        {
            try
            {
                Host modifiedHost =
                    await this.hostService.ModifyHostAsync(host);

                return Ok(modifiedHost);
            }
            catch (HostValidationException hostValidationException)
                when (hostValidationException.InnerException is NotFoundHostException)
            {
                return NotFound(hostValidationException.InnerException);
            }
            catch (HostValidationException hostValidationException)
            {
                return BadRequest(hostValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
                when (hostDependencyValidationException.InnerException is AlreadyExistsHostException)
            {
                return Conflict(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyException hostDependencyException)
            {
                return InternalServerError(hostDependencyException.InnerException);
            }
            catch (HostServiceException hostServiceException)
            {
                return InternalServerError(hostServiceException.InnerException);
            }
        }

        [HttpDelete("{hostId}")]
        public async ValueTask<ActionResult<Host>> DeleteHostByIdAsync(Guid hostId)
        {
            try
            {
                Host deletedHost =
                    await this.hostService.RemoveHostByIdAsync(hostId);

                return Ok(deletedHost);
            }
            catch (HostValidationException hostValidationException)
                when (hostValidationException.InnerException is NotFoundHostException)
            {
                return NotFound(hostValidationException.InnerException);
            }
            catch (HostValidationException hostValidationException)
            {
                return BadRequest(hostValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
                when (hostDependencyValidationException.InnerException is LockedHostException)
            {
                return Locked(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
            {
                return BadRequest(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyException hostDependencyException)
            {
                return InternalServerError(hostDependencyException);
            }
            catch (HostServiceException hostServiceException)
            {
                return InternalServerError(hostServiceException);
            }
        }
    }
}