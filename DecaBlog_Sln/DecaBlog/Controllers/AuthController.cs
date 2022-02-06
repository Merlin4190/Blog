using DecaBlog.Helpers;
using DecaBlog.Models.DTO;
using DecaBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using DecaBlog.Commons.Helpers;
using Microsoft.AspNetCore.Identity;
using DecaBlog.Models;

namespace DecaBlog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IUserService userService, IMailService mailService, IJwtService jwtService, UserManager<User> userManager)
        {
            _authService = authService;
            _userService = userService;
            _mailService = mailService;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseHelper.BuildResponse<string>(false, "Email is Invalid", ModelState, ""));
            }
            var response = await _authService.ForgotPassword(email);
            if (response == null)
            {
                return BadRequest(ResponseHelper.BuildResponse<string>(false, "Email does not exist", ModelState, ""));
            }
            var origin = HttpContext.Request.Headers["Origin"][0];
            var uriBuilder = new UriBuilder(origin + "/forgotpassword");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["email"] = email;
            query["token"] = response;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();
            var placeholders = new Dictionary<string, string>
            {
                ["{firstmsg}"] = "To Reset your Password, Please click the link below.",
                ["{Link}"] = urlString,
            };

            var mail = new EmailMessage
            {
                Subject = "Forgot Password",
                Template = "ForgotPassword",
                PlainTextMessage = "To Reset your Password, Please click the link below.",
                ToEmail = new List<string> { email },
                PlaceHolders = placeholders
            };

            await _mailService.SendMailAsync(mail);
            return Ok(ResponseHelper.BuildResponse<string>(true, "Successfully sent mail", null, response));
        }

        [HttpPatch("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.BuildResponse<string>(false, "Password reset not successful", ModelState, null));
            
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                ModelState.AddModelError("Not found", "User not found");
                return NotFound(ResponseHelper.BuildResponse<string>(false, $"User with email: {model.Email} not found", ModelState, null));
            }
            
            if(!(await _userManager.IsEmailConfirmedAsync(user)))
            {
                ModelState.AddModelError("Password reset failed", "Email not confirmed");
                return BadRequest(ResponseHelper.BuildResponse<string>(false, "Email is not confirmed", ModelState, null));
            }

            var response = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!response.Succeeded)
            {
                ModelState.AddModelError("Unsuccessful","Password reset failed");
                return BadRequest(ResponseHelper.BuildResponse<string>(false, "Password reset not successful", ModelState, null));
            }

            return Ok(ResponseHelper.BuildResponse<string>(true, "Password reset successful", ResponseHelper.NoErrors, null));
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            //find the user by his email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Invalid", "Invalid Credentials");
                return BadRequest(ResponseHelper.BuildResponse<LoginResponseDto>(false, "User does not exist", ModelState, null));
            }
            // check if user's email is confirmed
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                var result = await _authService.Login(model);
                if (result == null)
                {
                    ModelState.AddModelError("Failed", "Login Failed");
                    return BadRequest(ResponseHelper.BuildResponse<LoginResponseDto>(false, "Email or Password incorrect, please check and try again.", ModelState, null));
                }
                return Ok(ResponseHelper.BuildResponse<LoginResponseDto>(true, "Login is sucessful!", null, result));
            }
            ModelState.AddModelError("Invalid", "Please confirm your email.");
            return BadRequest(ResponseHelper.BuildResponse<LoginResponseDto>(false, "Email not confirmed", ModelState, null));
        }
    }
}
