using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WhosOnFirst.Data.Sql.Controllers;
using WhosOnFirstWeb.Models;

namespace WhosOnFirstWeb.Actions
{
    public class PlayerActions
    {
        #region GetPlayersInTeam
        public static IEnumerable<PlayersModel> GetPlayersInTeam(int teamId)
        {
            var playerManager = new PlayerManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var playerList = playerManager.GetAll().ToList();
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var personList = personManager.GetAll();

            var personsInTeam = personList.Where(p => p.TeamId == teamId).ToList();
            var list = new List<PlayersModel>();
            var tempPlayer = (from Person in personsInTeam
                              join Players in playerList
                              on Person.PersonId equals Players.PersonId
                              select new { Person, Players }).ToList();

            foreach (var element in tempPlayer)
            {
                var player = new PlayersModel();
                player.PlayerId = element.Players.PlayerId;
                player.PersonId = element.Person.PersonId;
                player.PositionRequested = element.Players.PositionRequested;
                player.TeamRequested = element.Players.TeamRequested;
                player.JerseyNumber = element.Players.JerseyNumber;
                player.Note = element.Players.Note;
                player.IsPitcher = element.Players.IsPitcher;
                player.FirstName = element.Person.FirstName;
                player.LastName = element.Person.LastName;
                player.PhoneNumber = element.Person.PhoneNumber;
                player.EMail = element.Person.EMail;
                player.IsPlayer = element.Person.IsPlayer;
                player.IsCoach = element.Person.IsCoach;
                player.IsValid = element.Person.IsValid;
                player.IsAdmin = element.Person.IsAdmin;
                player.TeamId = element.Person.TeamId;
                list.Add(player);
            }
            return list;

        }

        public static async Task<IEnumerable<PlayersModel>> GetPlayersInTeamAsync(int teamId)
        {
            var playerManager = new PlayerManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var playerList = await playerManager.GetAllAsync();
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var personList = await personManager.GetAllAsync();

            var personsInTeam = personList.Where(p => p.TeamId == teamId).ToList();
            var list = new List<PlayersModel>();
            var tempPlayer = (from Person in personsInTeam
                              join Players in playerList
                              on Person.PersonId equals Players.PersonId
                              select new { Person, Players }).ToList();

            foreach (var element in tempPlayer)
            {
                var player = new PlayersModel();
                player.PlayerId = element.Players.PlayerId;
                player.PersonId = element.Person.PersonId;
                player.PositionRequested = element.Players.PositionRequested;
                player.TeamRequested = element.Players.TeamRequested;
                player.JerseyNumber = element.Players.JerseyNumber;
                player.Note = element.Players.Note;
                player.IsPitcher = element.Players.IsPitcher;
                player.FirstName = element.Person.FirstName;
                player.LastName = element.Person.LastName;
                player.PhoneNumber = element.Person.PhoneNumber;
                player.EMail = element.Person.EMail;
                player.IsPlayer = element.Person.IsPlayer;
                player.IsCoach = element.Person.IsCoach;
                player.IsValid = element.Person.IsValid;
                player.IsAdmin = element.Person.IsAdmin;
                player.TeamId = element.Person.TeamId;
                list.Add(player);
            }
            return list;

        }
        #endregion
    }
}