﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        //POST : /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName

            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded) 
            {
                //Add roles to this user
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any()) 
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser,registerRequestDto.Roles);
                    if(identityResult.Succeeded) 
                    {
                        return Ok("User was resgistered! Please login");
                    }
                }
                
            }
            return BadRequest("Something went wrong");
        }

        // Login Action method
        //POST : /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);
            if (user != null) 
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user,loginRequestDto.Password);
                if(checkPasswordResult)
                {
                    //Get roles for this token
                    var roles = await userManager.GetRolesAsync(user);

                    if(roles != null && roles.Any())
                    {
                        //Create token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList()); 
                        var response  = new LoginResponseDto 
                        {
                          JwtToken =  jwtToken 
                        }; 
                        return Ok(jwtToken);



                    }

                }
            
            }
            return BadRequest("USer Name Password Incorrect");
        
        }

    }
}
