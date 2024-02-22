using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerUserRequests;

namespace DeviseHR_Server.Controllers.User_Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class ManagerUserController : ControllerBase
    {

        [HttpPost("addUser")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<NewUser>>> AddUser([FromBody] NewUser newUser)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);
                DateOnly companyAnnualLeaveDate = DateOnly.Parse(claimsPrincipal.FindFirst("annualLeaveStartDate")!.Value);

                await ManagerUserService.AddUser(newUser, myId, companyId, userType, companyAnnualLeaveDate);

                var serviceResponse = new ServiceResponse<NewUser>(newUser, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }



        [HttpPost("sendRegistration/:userId")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<ServiceResponse<string>>> SendRegistration(int userId)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int myUserType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);

                await ManagerUserService.SendRegistration(userId, myId, companyId, myUserType);

                var serviceResponse = new ServiceResponse<bool>(true, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


    }
}
