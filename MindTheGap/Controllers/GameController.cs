﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MindTheGap.Helpers;
using MindTheGap.Models;
using MindTheGap.Repositories;

namespace MindTheGap.Controllers
{
    public class GameController : Controller
    {
        private readonly QuestionAnswerRepository _questionAnswerRepository;
        private readonly GameHistoryRepository _gameHistoryRepository;

        public GameController()
        {
            _questionAnswerRepository = new QuestionAnswerRepository();
            _gameHistoryRepository = new GameHistoryRepository();
        }

        // GET: Game
        public ActionResult Index()
        {
            var selectedTrain = Session["SelectedTrain"] as TrainModel;
            if (selectedTrain == null)
                return RedirectToAction("Index", "JourneySelection");
            var journey = Session["Journey"] as JourneySelectionModel;
            if (journey == null)
                return RedirectToAction("Index", "JourneySelection");

            var eligibility = GetEligibility(journey, selectedTrain);

            if (!eligibility.Eligible)
            {
                return View();
            }

            Session["SelectedTrain"] = eligibility.Train;
            return View(eligibility);
        }

        private TrainEligibilityModel GetEligibility(JourneySelectionModel journey, TrainModel train)
        {
            var trainList = new TrainHelper().GetTrains(journey);
            if (trainList == null || !trainList.Any())
            {
                Session["SelectedTrain"] = null;
                Session["Journey"] = null;
                ViewBag.ErrorMessage = "Your train is no longer available.";
                return new TrainEligibilityModel { Eligible = false };
            }
            var latestTrainInfo = trainList.FirstOrDefault(t => t.RId == train.RId);
            if (latestTrainInfo == null)
            {
                Session["SelectedTrain"] = null;
                Session["Journey"] = null;
                ViewBag.ErrorMessage = "Your train is no longer available.";
                return new TrainEligibilityModel { Eligible = false };
            }
            if ((latestTrainInfo.EstimatedDepartureTime - latestTrainInfo.ScheduledDepartureTime).TotalSeconds < 60)
            {
                ViewBag.ErrorMessage = "Your train is not delayed.";
                return new TrainEligibilityModel { Eligible = false };
            }

            var now = DateTime.Now;
            if ((now - latestTrainInfo.EstimatedDepartureTime).TotalSeconds > -10)
            {
                ViewBag.ErrorMessage = "Your train is already on it's way.";
                return new TrainEligibilityModel { Eligible = false };
            }



            return new TrainEligibilityModel
            {
                Eligible = true,
                EligibilityEndTime = latestTrainInfo.EstimatedDepartureTime,
                Train = latestTrainInfo
            };
        }

        public ActionResult Start()
        {
            var selectedTrain = Session["SelectedTrain"] as TrainModel;
            if (selectedTrain == null)
                return RedirectToAction("Index", "JourneySelection");
            var journey = Session["Journey"] as JourneySelectionModel;
            if (journey == null)
                return RedirectToAction("Index", "JourneySelection");

            var eligibility = GetEligibility(journey, selectedTrain);

            if (!eligibility.Eligible)
            {
                ViewBag.ErrorMessage = "Your train is no longer available.";
                Session["SelectedTrain"] = null;
                Session["Journey"] = null;
                return View();
            }

            var model = new GamePlayModel { TrainEligibilityModel = eligibility, StartedAt = DateTime.Now };
            model.QuestionAnswer = _questionAnswerRepository.GetRandomQuestionAnswer();
            return View(model);
        }

        [HttpPost]
        public ActionResult Start(GamePlayModel model)
        {
            var user = Session["User"] as User;
            var selectedTrain = Session["SelectedTrain"] as TrainModel;
            if (selectedTrain == null)
                return RedirectToAction("Index", "JourneySelection");
            var journey = Session["Journey"] as JourneySelectionModel;
            if (journey == null)
                return RedirectToAction("Index", "JourneySelection");

            var eligibility = GetEligibility(journey, selectedTrain);
            var selectedAnswer = model.QuestionAnswer.Answers.FirstOrDefault(a => a.Answer == model.SelectedAnswer);
            var correctlyAnswered = selectedAnswer == null ? false : selectedAnswer.IsCorrect;
            var response = new QuestionAnswerResponseModel
            {
                Answers = model.QuestionAnswer.Answers,
                CorrectlyAnswered = correctlyAnswered,
                Question = model.QuestionAnswer.Question,
                User = user
            };
            var gamePlayHistory = new GameHistory
            {
                QuestionAnswerResponseModel = new List<QuestionAnswerResponseModel> { response },
                Train = selectedTrain,
                User = user,
                StartTime = model.StartedAt,
                EndTime = DateTime.Now
            };
            _gameHistoryRepository.AddGameHistory(gamePlayHistory);

            if (eligibility.Eligible)
                return RedirectToAction("Start", "Game");

            return RedirectToAction("Complete", "Game");
        }

        public ActionResult Complete()
        {
            var user = Session["User"] as User;
            var selectedTrain = Session["SelectedTrain"] as TrainModel;
            if (user == null || selectedTrain == null)
                return RedirectToAction("Index", "JourneySelection");
            var history = _gameHistoryRepository.GetGameHistory(user, selectedTrain);
            if (history == null)
                return RedirectToAction("Index", "JourneySelection");

            var model = new GameCompletionModel();
            model.NumberAnsweredCorrectly = history.NumberAnsweredCorrectly;
            model.NumberOfQuestions = history.NumberOfQuestions;
            var duration = (history.EndTime - history.StartTime);
            model.TotalDuration = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
            model.TrainRid = selectedTrain.RId;
            //return View(model);
            return RedirectToAction("ByTrain", "Leaderboard", new { id = selectedTrain.RId });
        }
    }
}