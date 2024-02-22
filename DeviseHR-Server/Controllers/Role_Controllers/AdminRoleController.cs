using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.RoleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static DeviseHR_Server.DTOs.RequestDTOs.RoleRequests;

namespace DeviseHR_Server.Controllers.Role_Controllers
{
    [Route("api/Roles/[controller]")]
    [ApiController]
    public class AdminRoleController : ControllerBase
    {

        [HttpPost("CreateRole")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<Role>>> CreateRole(NewRole newRole)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                Role role = await AdminRoleService.CreateRoleService(newRole, myId, companyId);

                var serviceResponse = new ServiceResponse<Role>(role, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<Role>(null!, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


        //[HttpPost("GetUsersWithRoles")]
        //[Authorize(Policy = "Admin")]
        //public async Task<ActionResult<ServiceResponse<Role>>> GetUsersWithRoles(NewRole newRole)
        //{
        //    try
        //    {
        //        string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
        //        Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

        //        int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
        //        int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

        //        Role role = await AdminRoleService.CreateRoleService(newRole, myId, companyId);

        //        var serviceResponse = new ServiceResponse<Role>(role, true, "");

        //        return Ok(serviceResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        var serviceResponse = new ServiceResponse<Role>(null!, false, ex.Message);
        //        return BadRequest(serviceResponse);
        //    }
        //}
    }
}
