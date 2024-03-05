using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Services.RoleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeviseHR_Server.Models;

namespace DeviseHR_Server.Controllers.Role_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerRoleController : ControllerBase
    {

        [HttpGet("GetCompanyRoles")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<List<Role>>>> GetCompanyRoles()
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                List<Role> roles = await ManagerRoleService.GetExistingRolesService(companyId);

                var serviceResponse = new ServiceResponse<List<Role>>(roles, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<UserAndRolesDto>(null!, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


    }
}
