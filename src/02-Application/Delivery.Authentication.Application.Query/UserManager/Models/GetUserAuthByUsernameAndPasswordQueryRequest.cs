namespace Delivery.Authentication.Application.Query.UserManager.Models
{
    public class GetUserAuthByUsernameAndPasswordQueryRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
