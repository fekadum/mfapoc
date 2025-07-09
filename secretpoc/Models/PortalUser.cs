namespace secretpoc.Models
{
    public class PortalUser : Generic
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Phone { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string AuthenticatorKey { get; set; } = string.Empty;

    }
    public class PortalUserBalance : Generic
    {
        public int Id { get; set; }
        public double balance { get; set; }
        public double total { get; set; }
        public double totalBalance { get; set; }

    }

    public class Generic
    {
        public DateTime CreatedTime { get; set; }
        public DateTime RevisedTime { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string RevisedBy { get; set; } = string.Empty;
    }
    public class TOTPVerificationViewModel
    {
        public string ActionKey { get; set; }
        public string AuthenticatorCode { get; set; }
    }
}