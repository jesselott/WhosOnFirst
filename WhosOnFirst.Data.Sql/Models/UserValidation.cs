namespace WhosOnFirst.Data.Sql.Models
{
    public class UserValidation
    {
        public int UserValidationId { get; set; }
        public string UserName { get; set; }
        public int PersonId { get; set; }
        public string Password { get; set; }
    }
}
