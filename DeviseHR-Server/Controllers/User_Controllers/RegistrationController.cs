using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeviseHR_Server.Controllers.User_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<User>>> Login([FromBody] LoginUserRequest loginUserRequest)
        {
            try
            {
                var serviceResponceUser = await RegistrationUserServices.GetUserByCredencials(loginUserRequest.Email, loginUserRequest.Password);

                return Ok(serviceResponceUser);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<User>(null!, false, ex.Message, null!);
                return BadRequest(serviceResponse);
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ServiceResponse<User>>> Refresh([FromBody] string refreshToken)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                string myIdStr = claimsPrincipal.FindFirst("id")!.Value;
                int userId = int.Parse(myIdStr);
                string myUserTypeStr = claimsPrincipal.FindFirst("userType")!.Value;
                int userType = int.Parse(myUserTypeStr);

                var serviceResponceUser = await RegistrationUserServices.GetAndRefreshUserById(userId, userType, refreshToken);

                return Ok(serviceResponceUser);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<User>(null!, false, ex.Message, null!);
                return BadRequest(serviceResponse);
            }
        }
    }
}
