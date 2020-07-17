﻿namespace Catstagram.Server.Features.Profiles
{
    using System.Threading.Tasks;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class ProfilesController : ApiController
    {
        private readonly IProfileService profiles;
        private readonly ICurrentUserService currentUser;

        public ProfilesController(
            IProfileService profiles, 
            ICurrentUserService currentUser)
        {
            this.profiles = profiles;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProfileServiceModel>> Mine()
            => await this.profiles.ByUser(this.currentUser.GetId());

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Update(UpdateProfileRequestModel model)
        {
            var userId = this.currentUser.GetId();

            var result = await this.profiles.Update(
                userId,
                model.Email,
                model.UserName,
                model.Name,
                model.MainPhotoUrl,
                model.WebSite,
                model.Biography,
                model.Gender,
                model.IsPrivate);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }
}
