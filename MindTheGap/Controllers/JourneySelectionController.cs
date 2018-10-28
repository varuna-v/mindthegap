using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MindTheGap.Helpers;
using MindTheGap.LDBStaffWebService;
using MindTheGap.Models;
using MindTheGap.Repositories;

namespace MindTheGap.Controllers
{
    public class JourneySelectionController : Controller
    {
        private readonly TrainStationRepository _trainStationRepository;

        public JourneySelectionController()
        {
            _trainStationRepository = new TrainStationRepository();
        }

        // GET: Journey
        public ActionResult Index()
        {
            if (!new UserHelper().IsLoggedIn(this))
                return RedirectToAction("Index", "Login");
            var stations = _trainStationRepository.GetTrainStations();
            var selectListItems = stations.Select(s => new SelectListItem() { Value = s.Code, Text = s.StationName }).ToList();
            var fromSelectList = new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Where does your journey start?"} };
            fromSelectList.AddRange(selectListItems);
            var toSelectList = new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Where does your journey end?" }};
            toSelectList.AddRange(selectListItems);
            ViewBag.FromStationList = fromSelectList;
            ViewBag.ToStationList = toSelectList;
            return View();
        }

        [HttpPost]
        public ActionResult Index(JourneySelectionModel journey)
        {
            if (journey == null || string.IsNullOrEmpty(journey.FromStationCode) ||
                string.IsNullOrEmpty(journey.ToStationCode))
            {
                ViewBag.ErrorMessage = "Please select your stations.";
                return View();
            }
            Session["Journey"] = journey;
            return RedirectToAction("SelectTrain", "JourneySelection");
        }

        public ActionResult SelectTrain()
        {
            var journey = Session["Journey"] as JourneySelectionModel;
            if (journey == null)
                return RedirectToAction("Index", "JourneySelection");

            var trainList = new TrainHelper().GetTrains(journey);

            var model = new SelectTrainModel { Trains = trainList };
            return View(model);
        }

        [HttpPost]
        public ActionResult SelectTrain(SelectTrainModel model)
        {
            var rId = model.SelectedRId;
            var train = model.Trains.FirstOrDefault(s => s.RId == rId);
            Session["SelectedTrain"] = train;
            return RedirectToAction("Index", "Game");
        }

      
    }
}