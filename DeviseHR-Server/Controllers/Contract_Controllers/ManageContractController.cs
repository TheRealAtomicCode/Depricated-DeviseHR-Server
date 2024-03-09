﻿using DeviseHR_Server.Common;
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
using DeviseHR_Server.Services.ContractServices;

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

                Contract addedContract = await ManagerContractService.AddContract(newConract, myId, companyId, userType);

                var serviceResponse = new ServiceResponse<Contract>(addedContract, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPatch("EndLastContract/{userId}")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<bool>>> EndLastContract([FromRoute] int userId, [FromBody] string endDate)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);

                await ManagerContractService.EndLastContractService(userId, endDate, myId, companyId, userType);

                var serviceResponse = new ServiceResponse<bool>(true, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }


        [HttpPost("calculateLeaveYear/{userId}")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public async Task<ActionResult<ServiceResponse<CreateContractRequest>>> GetLeaveYear([FromRoute] int userId, [FromBody] CreateContractRequest newConract)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);

                var contract = await ManagerContractService.CalculateLeaveYear(userId, myId, companyId, userType, newConract);

                var serviceResponse = new ServiceResponse<CreateContractRequest>(contract, true, "");

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<bool>(false, false, ex.Message);
                return BadRequest(serviceResponse);
            }
        }



        [HttpGet("GetLeaveYear/{userId}")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<ServiceResponse<bool>>> GetLeaveYear([FromRoute] int userId)
        {
            try
            {
                string clientJWT = Tokens.ExtractTokenFromRequestHeaders(HttpContext);
                Tokens.ExtractClaimsFromToken(clientJWT, false, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtToken);

                int myId = int.Parse(claimsPrincipal.FindFirst("id")!.Value);
                int companyId = int.Parse(claimsPrincipal.FindFirst("companyId")!.Value);
                int userType = int.Parse(claimsPrincipal.FindFirst("userType")!.Value);
                bool enableShowEmployees = bool.Parse(claimsPrincipal.FindFirst("enableShowEmployees")!.Value);

                await ManagerContractService.GetLeaveYearService(userId, myId, companyId, userType, enableShowEmployees);

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
