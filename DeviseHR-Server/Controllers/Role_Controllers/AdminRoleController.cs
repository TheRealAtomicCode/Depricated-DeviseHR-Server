using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.RoleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeviseHR_Server.Controllers.Role_Controllers
{
    [Route("api/Roles/[controller]")]
    [ApiController]
    public class AdminRoleController : ControllerBase
    {

        [HttpPost("CreateRole")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<Role>>> CreateRole(RoleData newRole)
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

        [HttpPatch("EditRole/{roleId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<Role>>> EditRole(int roleId, [FromBody] RoleData roleData)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                Role editedRole = await AdminRoleService.EditRoleService(roleData, roleId, myId, companyId);

                var serviceResponse = new ServiceResponse<Role>(editedRole, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<Role>(null!, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


        [HttpGet("GetUsersAndRoles")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserAndRolesDto>>> GetUsersAndRoles()
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                UserAndRolesDto usersAndRoles = await AdminRoleService.GetUsersAndRolesService(myId, companyId);
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                var serviceResponse = new ServiceResponse<UserAndRolesDto>(usersAndRoles, true, "");        

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<UserAndRolesDto>(null!, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }

        [HttpPatch("EditUserTypes")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<string>>> EditUserTypes(List<UsersRoles> usersRoles)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                await AdminRoleService.EditUserTypesService(usersRoles, myId, companyId);

                var serviceResponse = new ServiceResponse<string>("Roles Successfully edited", true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<string>(null!, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPatch("editSubordinates")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ServiceResponse<string>>> EditSubordinates(ManagersAndSubordinates managersAndSubordinates)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);

                await AdminRoleService.EditSubordinatesService(managersAndSubordinates, myId, companyId);

                var serviceResponse = new ServiceResponse<string>("Subordinates Successfully edited", true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                string cleanedErrorMessage = ex.Message.Substring(ex.Message.IndexOf(": ") + 2);
                var serviceResponse = new ServiceResponse<string>(null!, false, cleanedErrorMessage);
                return BadRequest(serviceResponse);
            }
        }
    }
}
