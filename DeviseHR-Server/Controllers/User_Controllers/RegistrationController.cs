using DeviseHR_Server.DTOs;
using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviseHR_Server.Controllers.User_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<User>>> Login([FromBody] LoginUserRequest loginUserRequest)
        {
            try
            {
                var serviceResponceUser = await RegistrationUserServices.LoginUser(loginUserRequest.Email, loginUserRequest.Password);

                return Ok(serviceResponceUser);
            }
            catch (Exception ex)
            {
                var serviceResponse = new ServiceResponse<User>(null!, false, ex.Message, null!);
                return BadRequest(serviceResponse);
            }
        }
    }
}
