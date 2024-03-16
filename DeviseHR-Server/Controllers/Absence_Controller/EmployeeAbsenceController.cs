using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Services.ContractServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.LeaveServices;

namespace DeviseHR_Server.Controllers.Leave_Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {


        [HttpPost("AddAbsence")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddLeave([FromBody] AddAbsenceRequest newAbsence)
        {

            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);

                await EmployeeLeaveService.RequestLeaveService(newAbsence, myId, companyId, userType);

                var serviceResponse = new ServiceResponse<bool>(true, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<Contract>(null!, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }



    }
}
