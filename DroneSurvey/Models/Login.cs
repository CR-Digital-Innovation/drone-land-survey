namespace DroneSurvey.Models
{
    public class Login
    {
        public string? EmailId { get; set; }
        public string? Password { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? ErrorMsg { get; set; }
    }

    public class ValidateMessage
    {
        public string? Message { get; set; }  
    }
}
