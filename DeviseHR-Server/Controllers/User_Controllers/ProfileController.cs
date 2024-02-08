using DeviseHR_Server.DTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeviseHR_Server.Common;
using System.ComponentModel.Design;
using DeviseHR_Server.DTOs.ResponseDTOs;

namespace DeviseHR_Server.Controllers.User_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        [HttpGet("me")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<ServiceResponse<User>>> Profile()
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                
                User myProfile = await ProfileService.GetMyProfile(userId);

                var serviceResponse = new ServiceResponse<User>(myProfile, true, null!, null!);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<string>(null!, false, ex.Message, null!);
                return BadRequest(serviceResponse);
            }
        }


        [HttpGet("getUsers")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<ServiceResponse<List<FoundUser>>>> GetUsers([FromQuery] int pageNo)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                bool enableShowEmployees = bool.Parse(claimsPrincipal.FindFirst("enableShowEmployees")!.Value);


                List<FoundUser> users = await ProfileService.GetAllCompanyUsers(myId, companyId, pageNo, userType, enableShowEmployees);

                var serviceResponse = new ServiceResponse<List<FoundUser>>(users, true, null!, null!);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<string>(null!, false, ex.Message, null!);
                return BadRequest(serviceResponse);
            }
        }


    }
}
