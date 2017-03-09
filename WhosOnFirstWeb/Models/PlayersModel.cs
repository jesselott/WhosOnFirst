using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirstWeb.Models
{
    public class PlayersModel : Person
    {
        public UserModel UserModel { get; set; }
        public int PlayerId { get; set; }
        public string PositionRequested { get; set; }
        public string TeamRequested { get; set; }
        public int JerseyNumber { get; set; }
        public string Note { get; set; }
        public bool IsPitcher { get; set; }

    }
}