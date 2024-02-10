using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.EmailServices;
using DeviseHR_Server.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeviseHR_Server.Controllers.User_Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<User>>> Login([FromBody] LoginUserRequest loginUserRequest)
        {
            try
            {
                var serviceResponceUser = await RegistrationUserServices.LoginUserByCredencials(loginUserRequest.Email, loginUserRequest.Password);

                return Ok(serviceResponceUser);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<User>(null!, false, ex.Message, null!);
                return BadRequest(serviceResponse);
            }
        }

        [HttpPost("refresh")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<ServiceResponse<User>>> Refresh([FromBody] string refreshToken)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);

                var serviceResponceUser = await RegistrationUserServices.RefreshUserToken(myId, companyId, refreshToken);

                return Ok(serviceResponceUser);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<User>(null!, false, ex.Message, null!);
                return BadRequest(serviceResponse);
            }
        }


        [HttpDelete("logout")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<ServiceResponse<bool>>> Logout([FromBody] string refreshToken)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int userId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                await RegistrationUserServices.LogoutUserByRefreshToken(userId, companyId, refreshToken);

                var serviceResponse = new ServiceResponse<bool>(true, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


        [HttpGet("logoutAllDevices")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<ServiceResponse<bool>>> LogoutAllDevices()
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                await RegistrationUserServices.LogoutAllDevicesByUserId(myId, companyId);

                var serviceResponse = new ServiceResponse<bool>(true, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }

        [HttpPatch("resetPassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword([FromBody] string email)
        {
            try
            {
                await RegistrationUserServices.resetPasswordByEmail(email.Trim());

                var serviceResponse = new ServiceResponse<string>(email, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<string>("", false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }

        [HttpPatch("confermResetPassword")]
        public async Task<ActionResult<ServiceResponse<User>>> ConfermResetPassword([FromBody] ResetUserPasswordRequest requestBody)
        {
            try
            {
                var serviceResponse = await RegistrationUserServices.confirmVerificationCodeByEmail(requestBody.Email, requestBody.VerificationCode, requestBody.Password);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<User>(null!, false, ex.Message, null!);
                return BadRequest(serviceResponse);
            }
        }




    }
}
