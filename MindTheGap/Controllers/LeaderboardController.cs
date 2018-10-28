using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MindTheGap.Models;
using MindTheGap.Repositories;

namespace MindTheGap.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly GameHistoryRepository _gameHistoryRepository;

        public LeaderboardController()
        {
            _gameHistoryRepository = new GameHistoryRepository();
        }

        // GET: Leaderboard
        public ActionResult ByTrain(string id)
        {
            var gameHistories = _gameHistoryRepository.GetGameHistory(id);
            if (gameHistories == null || !gameHistories.Any())
            {
                ViewBag.ErrorMessage = "No games found.";
                return View();
            }

            var result = new LeaderboardByTrainModel();
            result.Train = gameHistories.First().Train;
            result.Leaders = new List<LeaderModel>();
            result.ComputedAt = DateTime.Now;

            var currentUser = Session["User"] as User;
            var rank = 1;
            var lastLeader = new LeaderModel();
            foreach (var gameHistory in gameHistories)
            {
                var leader = new LeaderModel();
                leader.UserName = gameHistory.User.UserName;
                leader.CorrectlyAnsweredCount = gameHistory.NumberAnsweredCorrectly;
                leader.TotalNumberOfQuestionsAnswered = gameHistory.NumberOfQuestions;
                leader.CorrectPercentage = (gameHistory.CorrectPercentage) + "%";
                leader.IsCurrentUser = currentUser != null && currentUser.UserName == gameHistory.User.UserName;
                leader.Rank = leader.CorrectPercentage == lastLeader.CorrectPercentage ? lastLeader.Rank : rank;                    
                result.Leaders.Add(leader);
                rank++;
                lastLeader = leader;
            }

            return View(result);
        }
    }
}