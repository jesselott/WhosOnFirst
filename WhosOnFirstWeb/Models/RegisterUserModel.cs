using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirstWeb.Models
{
    public class RegisterUserModel : Person
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PositionRequested { get; set; }
        public string TeamRequested { get; set; }
        public string UserRole { get; set; }
    }
}