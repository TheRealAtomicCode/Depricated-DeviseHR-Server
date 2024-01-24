using DeviseHR_Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeviseHR_Server.Common
{
    public static class Tokens
    {
        public static string GenerateUserAuthJWT(User user)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("SecretUYJHy787987iu&*(&^&UI^&*^(*&uiiyu^&^%^%^&RTrtyty$YTRGTY$^75ty5^&%&^&%^ry5675yttYETUtui6&*^&*(^&*6)))"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

          

            var claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()), 
                    new Claim("userType", user.UserType.ToString()),
                    new Claim("enableAddEmployees", user.Role.EnableAddEmployees ? "true" : null),
                    new Claim("enableAddLateness", user.Role.EnableAddLateness ? "true" : null),
                    new Claim("enableAddManditoryLeave", user.Role.EnableAddManditoryLeave ? "true" : null),
                    new Claim("enableApproveAbsence", user.Role.EnableApproveAbsence ? "1" : null),
                    new Claim("enableCreatePattern", user.Role.EnableCreatePattern ? "1" : null),
                    new Claim("enableCreateRotas", user.Role.EnableCreateRotas ? "1" : null),
                    new Claim("enableDeleteEmployee", user.Role.EnableDeleteEmployee ? "1" : null),
                    new Claim("enableTerminateEmployees", user.Role.EnableTerminateEmployees ? "1" : null),
                    new Claim("enableViewEmployeeNotifications", user.Role.EnableViewEmployeeNotifications ? "1" : null),
                    new Claim("enableViewEmployeePayroll", user.Role.EnableViewEmployeePayroll ? "1" : null),
                    new Claim("enableViewEmployeeSensitiveInformation", user.Role.EnableViewEmployeeSensitiveInformation ? "1" : null)
                };
            
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(15), 
                signingCredentials: credentials,
                claims: claims // Add the claims to the token
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
