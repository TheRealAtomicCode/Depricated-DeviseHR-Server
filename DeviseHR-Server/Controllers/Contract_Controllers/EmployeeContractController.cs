using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.ContractServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeviseHR_Server.Controllers.Contract_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeContractController : ControllerBase
    {

        [HttpGet("GetLeaveYear/{userId}")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<ServiceResponse<List<LeaveYear>>>> GetLeaveYear([FromRoute] int userId)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);
                bool enableShowEmployees = bool.Parse(claimsPrincipal.FindFirst("enableShowEmployees")!.Value);

                List<LeaveYear> leaveYears = await EmployeeContract.GetLeaveYearService(userId, myId, companyId, userType, enableShowEmployees);

                var serviceResponse = new ServiceResponse<List<LeaveYear>>(leaveYears, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<List<LeaveYear>>(null!, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


    }
}
