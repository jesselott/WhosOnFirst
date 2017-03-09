using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using WhosOnFirst.Data.Sql.Controllers;
using WhosOnFirst.Data.Sql.Models;
using WhosOnFirstWeb.Models;
using static WhosOnFirstWeb.Actions.PlayerActions;

namespace WhosOnFirstWeb.Controllers
{
    public class CoachController : Controller
    {
        #region CoachIndex
        public ActionResult CoachIndex()
        {
            var userModel = Session["userModel"] as UserModel;
            var coachModel = new CoachModel();
            coachModel.UserModel = userModel;

            return View(coachModel);
        }


        #endregion

        #region AddTeam
        [HttpPost]
        public async Task<ActionResult> AddTeam(CoachModel coachModel)
        {

            var teamsManager = new TeamsManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var teamExists = await teamsManager.ExistsAsync(coachModel.TeamName);
            if (!teamExists)
                teamsManager.AddAsync(coachModel.TeamName);
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var person = new Person();
            var userModel = Session["userModel"] as UserModel;
            person.PersonId = userModel.PersonId;
            person.FirstName = userModel.FirstName;
            person.LastName = userModel.LastName;
            person.PhoneNumber = userModel.PhoneNumber;
            person.EMail = userModel.EMail;
            person.IsPlayer = userModel.IsPlayer;
            person.IsCoach = userModel.IsCoach;
            person.IsValid = userModel.IsValid;
            person.IsAdmin = userModel.IsAdmin;
            var team = await teamsManager.RetrieveAsync(coachModel.TeamName);
            person.TeamId = team.TeamsId;
            personManager.Update(person);
            userModel.TeamId = team.TeamsId;
            Session["userModel"] = userModel;

            return View(coachModel);
        }
        #endregion

        #region ViewTeamPlayers
        public async Task<ActionResult> ViewTeamPlayers(int Id)
        {
            var players = await GetPlayersInTeamAsync(Id);

            return View(players);
        }
        #endregion

        #region ViewAllTeams
        public async Task<ActionResult> ViewAllTeams()
        {
            var userModel = Session["userModel"] as UserModel;
            var teamsManager = new TeamsManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var teams = await teamsManager.GetAllAsync();

            ViewBag.TeamStatus = userModel.TeamId;

            return View(teams);
        }
        #endregion

        #region RemoveTeam
        public ActionResult RemoveTeam()
        {
            var userModel = Session["userModel"] as UserModel;
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var person = new Person();
            person.PersonId = userModel.PersonId;
            person.FirstName = userModel.FirstName;
            person.LastName = userModel.LastName;
            person.PhoneNumber = userModel.PhoneNumber;
            person.EMail = userModel.EMail;
            person.IsPlayer = userModel.IsPlayer;
            person.IsCoach = userModel.IsCoach;
            person.IsValid = userModel.IsValid;
            person.IsAdmin = userModel.IsAdmin;
            person.TeamId = 0;
            personManager.Update(person);
            userModel.TeamId = 0;
            Session["userModel"] = userModel;

            return RedirectToAction("CoachIndex");
        }

        #endregion

        #region EditPlayer
        public async Task<ActionResult> EditPlayer(int Id)
        {
            var playerManager = new PlayerManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var playerModel = new Players();
            var player = new PlayersModel();
            var person = new Person();
            playerModel = await playerManager.RetrieveAsync(Id);
            player.PlayerId = Id;
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

            return RedirectToAction("ViewTeamPlayers");
        }
        #endregion

        #region ViewAvailablePeople
        public async Task<ActionResult> ViewAvailablePeople()
        {
            var players = await GetPlayersInTeamAsync(0);

            return View(players);
        }
        #endregion

        #region AddPlayer
        public ActionResult AddPlayer(int Id)
        {
            var userModel = Session["userModel"] as UserModel;
            var playerManager = new PlayerManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var player = new Players();
            player = playerManager.Retrieve(Id);
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var person = new Person();
            person = personManager.Retrieve(player.PersonId);
            person.TeamId = userModel.TeamId;
            personManager.Update(person);

            return RedirectToAction("ViewAvailablePeople");
        }
        #endregion

        #region RemovePlayerFromTeam
        public ActionResult RemovePlayerFromTeam(int Id)
        {
            var userModel = Session["userModel"] as UserModel;
            var playerManager = new PlayerManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var player = new Players();
            player = playerManager.Retrieve(Id);
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var person = new Person();
            person = personManager.Retrieve(player.PersonId);
            var teamId = person.TeamId;
            person.TeamId = 0;
            personManager.Update(person);

            return RedirectToAction("ViewTeamPlayers", new { id = teamId });
        }
        #endregion

        #region CoachTeam
        public ActionResult CoachTeam(int Id)
        {
            var userModel = Session["userModel"] as UserModel;
            userModel.TeamId = Id;
            Session["userModel"] = userModel;
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var person = personManager.Retrieve(userModel.PersonId);
            person.TeamId = Id;
            personManager.Update(person);

            return RedirectToAction("ViewAllTeams");
        }
        #endregion

        #region LeaveTeam
        public ActionResult LeaveTeam()
        {
            var userModel = Session["userModel"] as UserModel;
            userModel.TeamId = 0;
            Session["userModel"] = userModel;
            var personManager = new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
            var person = personManager.Retrieve(userModel.PersonId);
            person.TeamId = 0;
            personManager.Update(person);

            return RedirectToAction("ViewAllTeams");
        }
        #endregion



    }
}