namespace DeviseHR_Server.DTOs.ResponseDTOs
{
    public class FoundUser
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int UserType { get; set; }
    }
}
