using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using WhosOnFirst.Data.Sql.Controllers;
using WhosOnFirst.Data.Sql.Models;
using WhosOnFirstWeb.Models;
using static WhosOnFirstWeb.Actions.PlayerActions;

namespace WhosOnFirstWeb.Controllers
{
    public class PlayerController : Controller
    {
        #region PlayerIndex
        public async Task<ActionResult> PlayerIndex()
        {
            var userModel = Session["userModel"] as UserModel;
            var playerManager = new PlayerManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var playerModel = new Players();
            var player = new PlayersModel();
            var person = new Person();
            player.UserModel = userModel;
            playerModel = await playerManager.RetrieveAsync(player.UserModel.PersonId);
            player.PlayerId = player.UserModel.PersonId;
            player.PersonId = playerModel.PersonId;
            player.PositionRequested = playerModel.PositionRequested;
            player.TeamRequested = playerModel.TeamRequested;
            player.JerseyNumber = playerModel.JerseyNumber;
            player.Note = playerModel.Note;
            player.IsPitcher = playerModel.IsPitcher;
            person = await personManager.RetrieveAsync(playerModel.PersonId);
            player.FirstName = person.FirstName;
            player.LastName = person.LastName;
            player.PhoneNumber = person.PhoneNumber;
            player.EMail = person.EMail;
            player.IsPlayer = person.IsPlayer;
            player.IsCoach = person.IsCoach;
            player.IsValid = person.IsValid;
            player.IsAdmin = person.IsAdmin;
            player.TeamId = person.TeamId;

            return View(player);
        }

        #endregion

        #region EditPlayer
        public async Task<ActionResult> EditPlayer()
        {
            var userModel = Session["userModel"] as UserModel;
            var playerManager = new PlayerManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var playerModel = new Players();
            var player = new PlayersModel();
            var person = new Person();
            player.UserModel = userModel;
            playerModel = await playerManager.RetrieveAsync(player.UserModel.PersonId);
            player.PlayerId = playerModel.PlayerId;
            player.PersonId = playerModel.PersonId;
            player.PositionRequested = playerModel.PositionRequested;
            player.TeamRequested = playerModel.TeamRequested;
            player.JerseyNumber = playerModel.JerseyNumber;
            player.Note = playerModel.Note;
            player.IsPitcher = playerModel.IsPitcher;
            person = await personManager.RetrieveAsync(playerModel.PersonId);
            player.FirstName = person.FirstName;
            player.LastName = person.LastName;
            player.PhoneNumber = person.PhoneNumber;
            player.EMail = person.EMail;
            player.IsPlayer = person.IsPlayer;
            player.IsCoach = person.IsCoach;
            player.IsValid = person.IsValid;
            player.IsAdmin = person.IsAdmin;
            player.TeamId = person.TeamId;

            return View(player);
        }

        [HttpPost]
        public ActionResult EditPlayer(PlayersModel editedPlayers)
        {

            var playerManager = new PlayerManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var playerModel = new Players();
            var person = new Person();
            //playerModel = playerManager.Retrieve(editedPlayers.PersonId);
            playerModel.PlayerId = editedPlayers.PlayerId;
            playerModel.PositionRequested = editedPlayers.PositionRequested;
            playerModel.TeamRequested = editedPlayers.TeamRequested;
            playerModel.JerseyNumber = editedPlayers.JerseyNumber;
            playerModel.Note = editedPlayers.Note;
            playerModel.IsPitcher = editedPlayers.IsPitcher;
            playerModel.PersonId = editedPlayers.PersonId;
            playerManager.Update(playerModel);
            person.PersonId = editedPlayers.PersonId;
            person.FirstName = editedPlayers.FirstName;
            person.LastName = editedPlayers.LastName;
            person.PhoneNumber = editedPlayers.PhoneNumber;
            person.EMail = editedPlayers.EMail;
            person.IsPlayer = editedPlayers.IsPlayer;
            person.IsCoach = editedPlayers.IsCoach;
            person.IsValid = editedPlayers.IsValid;
            person.IsAdmin = editedPlayers.IsAdmin;
            person.TeamId = editedPlayers.TeamId;
            personManager.Update(person);

            return RedirectToAction("PlayerIndex");
        }

        #endregion

        #region ViewTeamPlayers
        public async Task<ActionResult> ViewTeamPlayers(int Id)
        {
            var players = await GetPlayersInTeamAsync(Id);

            return View(players);
        }
        #endregion

    }
}