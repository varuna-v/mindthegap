using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using MindTheGap.Helpers;
using MindTheGap.LDBStaffWebService;
using MindTheGap.Models;
using MindTheGap.Repositories;

namespace MindTheGap.Controllers
{
    public class GameController : Controller
    {
        private readonly QuestionAnswerRepository _questionAnswerRepository;

        public GameController()
        {
            _questionAnswerRepository = new QuestionAnswerRepository();
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
                ViewBag.ErrorMessage = "Your train is no longer available.";
                Session["SelectedTrain"] = null;
                Session["Journey"] = null;
                return View();
            }

            Session["SelectedTrain"] = eligibility.Train;
            return View(eligibility);
        }

        private TrainEligibilityModel GetEligibility(JourneySelectionModel journey, TrainModel train)
        {
            var trainList = new TrainHelper().GetTrains(journey);
            if (trainList == null || !trainList.Any())
                return new TrainEligibilityModel() { Eligible = false };
            var latestTrainInfo = trainList.FirstOrDefault(t => t.RId == train.RId);
            if (latestTrainInfo == null)
                return new TrainEligibilityModel() { Eligible = false };
            if ((latestTrainInfo.EstimatedDepartureTime - latestTrainInfo.ScheduledDepartureTime).TotalSeconds < 60)
                return new TrainEligibilityModel() { Eligible = false };
            if ((DateTime.Now - latestTrainInfo.EstimatedDepartureTime).TotalSeconds > -10)
                return new TrainEligibilityModel() { Eligible = false };


            return new TrainEligibilityModel()
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

            var model = new GamePlayModel() {TrainEligibilityModel = eligibility};
            model.QuestionAnswer = _questionAnswerRepository.GetRandomQuestionAnswer();
            return View(model);

        }
    }
}