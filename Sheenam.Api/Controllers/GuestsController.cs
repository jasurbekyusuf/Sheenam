//===================================================
// Copyright (c)  coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Pease
//===================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : RESTFulController
    {
        private readonly IGuestService guestService;

        public GuestsController(IGuestService guestService)
        {
            this.guestService = guestService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Guest>> PostGuestAsync(Guest guest)
        {
            try
            {
                Guest postedGuest = await this.guestService.AddGuestAsync(guest);

                return Created(postedGuest);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is AlreadyExistGuestException)
            {
                return Conflict(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
            {
                return BadRequest(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Guest>> GetAllGuests()
        {
            try
            {
                IQueryable<Guest> allGuests = this.guestService.RetrieveAllGuests();

                return Ok(allGuests);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException.InnerException);
            }
        }

        [HttpGet("{guestId}")]
        public async ValueTask<ActionResult<Guest>> GetGuestTicketByIdAsync(Guid guestId)
        {
            try
            {
                return await this.guestService.RetrieveGuestByIdAsync(guestId);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is InvalidGuestException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Guest>> PutGuestAsync(Guest guest)
        {
            try
            {
                Guest modifiedGuest =
                    await this.guestService.ModifyGuestAsync(guest);

                return Ok(modifiedGuest);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is AlreadyExistGuestException)
            {
                return Conflict(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException);
            }
        }

        [HttpDelete("{guestId}")]
        public async ValueTask<ActionResult<Guest>> DeleteGuestByIdAsync(Guid guestId)
        {
            try
            {
                Guest deletedGuest =
                    await this.guestService.RemoveGuestByIdAsync(guestId);

                return Ok(deletedGuest);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is LockedGuestException)
            {
                return Locked(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
            {
                return BadRequest(guestDependencyValidationException);
            }
            catch (GuestDependencyException GuestDependencyException)
            {
                return InternalServerError(GuestDependencyException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException);
            }
        }
    }
}
