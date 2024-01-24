

namespace DeviseHR_Server.Common
{
    public static class AccessMethods
    {
        public static void VerifyAccess(bool isVerified, bool isTerminated, int loginAttempt)
        {
            if (isTerminated) throw new Exception("Your Permissions have been Revoked.");

            if (isVerified == false) throw new Exception("Please sign into your account with the registration email that was sent to you. Please make sure to check the junk and spam folders. If you did not receive it, please contact your manager.");

            string? loginAtteptsAllowed = Environment.GetEnvironmentVariable("LOGIN_ATTEPTS_ALLOWED");

            if (loginAtteptsAllowed == null) throw new Exception("Environment Error");
            
            int loginAttemptsAllowed = int.Parse(loginAtteptsAllowed);

            if (loginAttempt > loginAttemptsAllowed) throw new Exception("You have attempted to login multiple times unsuccessfully. Please contact your manager to regain access to your account.");
        }
    }
}
