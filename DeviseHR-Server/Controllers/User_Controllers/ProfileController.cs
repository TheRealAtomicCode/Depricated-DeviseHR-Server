﻿using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeviseHR_Server.Middleware;
using Microsoft.AspNetCore.Authorization;

namespace DeviseHR_Server.Controllers.User_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        [HttpPost("profile")]
        [Authorize(Policy = "Manager")]
        [Authorize(Policy = "EnableAddEmployees")]
        public ActionResult<ServiceResponse<string>> Profile()
        {
            try
            {
                var serviceResponse = new ServiceResponse<string>("sadsda", true, null!, null!);

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