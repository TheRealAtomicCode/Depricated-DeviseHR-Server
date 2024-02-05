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

                string myIdStr = claimsPrincipal.FindFirst("id")!.Value;
                int userId = int.Parse(myIdStr);

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

                string myIdStr = claimsPrincipal.FindFirst("id")!.Value;
                int myId = int.Parse(myIdStr);
                string userTypeStr = claimsPrincipal.FindFirst("userType")!.Value;
                int userType = int.Parse(userTypeStr);
                string companyIdStr = claimsPrincipal.FindFirst("companyId")!.Value;
                int companyId = int.Parse(companyIdStr);
                string enableShowEmployeesStr = claimsPrincipal.FindFirst("enableShowEmployees")!.Value;
                bool enableShowEmployees = bool.Parse(enableShowEmployeesStr);



                List<FoundUser> users = await ProfileService.GetAllCompanyUsers(myId, companyId, pageNo, userType, enableShowEmployees);

                var serviceResponse = new ServiceResponse<List<User>>(users, true, null!, null!);

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
