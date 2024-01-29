using DeviseHR_Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DeviseHR_Server.Common
{
    public static class Tokens
    {
        public static async Task<string> GenerateUserAuthJWT(User user)
        {
            string? jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

            if (string.IsNullOrWhiteSpace(jwtSecret)) throw new Exception("Environment Error");

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = GenerateUserJwtClaims(user);

            string? jwtExpTime = Environment.GetEnvironmentVariable("EXPTIME");

            if (string.IsNullOrWhiteSpace(jwtExpTime)) throw new Exception("Environment Error");

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtExpTime)), 
                signingCredentials: credentials,
                claims: claims // Add the claims to the token
            );

            var jwt = await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));

            return jwt;
        }

        public static List<Claim> GenerateUserJwtClaims(User user)
        {
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
            }
            else if (user.UserType == 2)
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

            return claims;
        }


        // refresh tokens
        public static async Task<string> GenerateUserRefreshToken(User user)
        {
            string? refreshJwtSecret = Environment.GetEnvironmentVariable("JWT_REFRESH_SECRET");

            if (string.IsNullOrWhiteSpace(refreshJwtSecret)) throw new Exception("Environment Error");

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(refreshJwtSecret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            var claims = GenerateUserRefreshJwtClaims(user);

            string? jwtExpTime = Environment.GetEnvironmentVariable("EXPTIME");

            if (string.IsNullOrWhiteSpace(jwtExpTime)) throw new Exception("Environment Error");

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtExpTime)),
                signingCredentials: credentials,
                claims: claims // Add the claims to the token
            );

            var refreshJwt = await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));

            return refreshJwt;
        }

        public static List<Claim> GenerateUserRefreshJwtClaims(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("userType", user.UserType.ToString()),
            };

            return claims;
        }


        // Extract claims 
        public static void ExtractClaimsFromToken(string clientJwt, bool isRefreshToken, out ClaimsPrincipal claimsPrincipal, out JwtSecurityToken jwtSecurityToken)
        {
            string envName;
            if (isRefreshToken) {
                envName = "JWT_REFRESH_SECRET";
            }
            else
            {
                envName = "JWT_SECRET";
            }

            string? secret = Environment.GetEnvironmentVariable(envName);
            if (secret == null) throw new Exception("Environment Error");

            // Validate Token
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret))
            };

            SecurityToken validateToken;
            claimsPrincipal = tokenHandler.ValidateToken(clientJwt, tokenValidationParameters, out validateToken);

            jwtSecurityToken = (JwtSecurityToken)validateToken;
        }

        public static string ExtractTokenFromRequestHeaders(HttpContext httpContext)
        {
            string? token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null) throw new Exception("Failed to Authenticate Request headers");

            return token;
        }



    }
}
