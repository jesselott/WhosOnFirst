namespace WhosOnFirst.Data.Sql.Models
{
    public class Person
    {

        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EMail { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsCoach { get; set; }
        public bool IsValid { get; set; }
        public bool IsAdmin { get; set; } = false;
        public int TeamId { get; set; }
    }
}
