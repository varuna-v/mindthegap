using System;
using System.Collections.Generic;
using System.Linq;
using MindTheGap.LDBStaffWebService;
using MindTheGap.Models;

namespace MindTheGap.Helpers
{
    public class TrainHelper
    {
        public List<TrainModel> GetTrains(JourneySelectionModel journey)
        {
            return new List<TrainModel>()
            {
                new TrainModel()
                {
                    FromStation = "MAN",
                    ToStation = "MIA",
                    ScheduledDepartureTime = new DateTime(2018, 10, 28, 1, 8, 0),
                    EstimatedDepartureTime = new DateTime(2018, 10, 28, 1, 9, 30),
                    RId = "123456"
                },
                new TrainModel()
                {
                    FromStation = "MAN",
                    ToStation = "MIA",
                    ScheduledDepartureTime = new DateTime(2018, 10, 28, 0, 7, 0),
                    EstimatedDepartureTime = new DateTime(2018, 10, 28, 0, 9, 0),
                    RId = "ABCDEF"
                },
                new TrainModel()
                {
                    FromStation = "MAN",
                    ToStation = "MIA",
                    ScheduledDepartureTime = new DateTime(2018, 10, 28, 1, 10, 0),
                    EstimatedDepartureTime = new DateTime(2018, 10, 28, 1, 25, 0),
                    RId = "HIJK"
                },
                new TrainModel()
                {
                    FromStation = "MAN",
                    ToStation = "MIA",
                    ScheduledDepartureTime = new DateTime(2018, 10, 28, 2, 7, 0),
                    EstimatedDepartureTime = new DateTime(2018, 10, 28, 2, 45, 0),
                    RId = "LMNO"
                }
            };
        }
        public List<TrainModel> GetRealTrains(JourneySelectionModel journey)
        {
            var client = new LDBSVServiceSoapClient();
            var accessToken = new AccessToken() { TokenValue = "e983e75e-d6c1-49c8-86ec-9031695222ad" };
            var boardWithDetails = client.GetDepBoardWithDetails(accessToken, 50,
                journey.FromStationCode, DateTime.Now, 120,
                journey.ToStationCode, FilterType.to, null, "P", false);
            return GetTrainList(journey, boardWithDetails);
        }

        private List<TrainModel> GetTrainList(JourneySelectionModel journey,
            StationBoardWithDetails boardWithDetails)
        {
            if (boardWithDetails.trainServices == null || !boardWithDetails.trainServices.Any())
                return new List<TrainModel>();

            var result = new List<TrainModel>();
            foreach (var trainService in boardWithDetails.trainServices)
            {
                if (trainService.etd == DateTime.MinValue)
                    continue;
                var model = new TrainModel();
                model.RId = trainService.rid;
                model.FromStation = journey.FromStationCode;
                model.ToStation = journey.ToStationCode;
                model.EstimatedDepartureTime = trainService.etd;
                model.ScheduledDepartureTime = trainService.std;
                result.Add(model);
            }

            return result;
        }
    }
}