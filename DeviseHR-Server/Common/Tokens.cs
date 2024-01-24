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
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("JhdjG78897&*hDWewfhjJHuii()()90*789^^&6%56rtRE#w3321@!#ER789*(UIJjhGHGFSXcVjkhaiuohRDIiu&(*iJJKUIO78(iu()*^&)))"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>();

            if (user.UserType == 1)
            {
                claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("userType", user.UserType.ToString()),
                    new Claim("enableAddEmployees", "true"),
                    new Claim("enableAddLateness", "true"),
                    new Claim("enableAddManditoryLeave", "true"),
                    new Claim("enableApproveAbsence", "true"),
                    new Claim("enableCreatePattern", "true"),
                    new Claim("enableCreateRotas", "true"),
                    new Claim("enableDeleteEmployee", "true"),
                    new Claim("enableTerminateEmployees", "true"),
                    new Claim("enableViewEmployeeNotifications", "true"),
                    new Claim("enableViewEmployeePayroll", "true"),
                    new Claim("enableViewEmployeeSensitiveInformation", "true")
                };
            }else if(user.UserType == 2)
            {
                claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("userType", user.UserType.ToString()),
                    new Claim("enableAddEmployees", user.Role!.EnableAddEmployees ? "true" : "false"),
                    new Claim("enableAddLateness", user.Role!.EnableAddLateness ? "true" : "false"),
                    new Claim("enableAddManditoryLeave", user.Role!.EnableAddManditoryLeave ? "true" : "false"),
                    new Claim("enableApproveAbsence", user.Role!.EnableApproveAbsence ? "true" : "false"),
                    new Claim("enableCreatePattern", user.Role!.EnableCreatePattern ? "true" : "false"),
                    new Claim("enableCreateRotas", user.Role!.EnableCreateRotas ? "true" : "false"),
                    new Claim("enableDeleteEmployee", user.Role!.EnableDeleteEmployee ? "true" : "false"),
                    new Claim("enableTerminateEmployees", user.Role!.EnableTerminateEmployees ? "true" : "false"),
                    new Claim("enableViewEmployeeNotifications", user.Role!.EnableViewEmployeeNotifications ? "true" : "false"),
                    new Claim("enableViewEmployeePayroll", user.Role!.EnableViewEmployeePayroll ? "true" : "false"),
                    new Claim("enableViewEmployeeSensitiveInformation", user.Role!.EnableViewEmployeeSensitiveInformation ? "true" : "false")
                };
            }
            else if (user.UserType == 3)
            {
                claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("userType", user.UserType.ToString()),
                };
            }

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
