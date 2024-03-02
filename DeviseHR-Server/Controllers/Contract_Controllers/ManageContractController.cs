using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerUserRequests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;

namespace DeviseHR_Server.Controllers.Contract_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageContractController : ControllerBase
    {

        // Regular 3,
        // Variable 2,
        // Agency 1



        [HttpPost("CreateContract")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<Contract>>> CreateContract([FromBody] CreateContractRequest newConract)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);
                DateOnly companyAnnualLeaveDate = DateOnly.Parse(claimsPrincipal.FindFirst("annualLeaveStartDate")!.Value);

               // Contract addedContract = await ManagerContractService.AddContract(newConract, myId, companyId, userType, companyAnnualLeaveDate);
               Contract addedContract = new Contract();
                // complete this
                // complete this
                // complete this
                // complete this
                // complete this
                // complete this
                // complete this
                // complete this

                var serviceResponse = new ServiceResponse<Contract>(addedContract, true, "");

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
