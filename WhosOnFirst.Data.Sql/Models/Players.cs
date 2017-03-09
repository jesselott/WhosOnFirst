namespace WhosOnFirst.Data.Sql.Models
{
    public class Players
    {
        public int PlayerId { get; set; }
        public int PersonId { get; set; }
        public string PositionRequested { get; set; }
        public string TeamRequested { get; set; }
        public string Note { get; set; }
        public int JerseyNumber { get; set; }
        public bool IsPitcher { get; set; } = false;
    }
}
