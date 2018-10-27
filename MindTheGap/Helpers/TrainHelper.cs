using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MindTheGap.LDBStaffWebService;
using MindTheGap.Models;

namespace MindTheGap.Helpers
{
    public class TrainHelper
    {
        public List<TrainModel> GetTrains(JourneySelectionModel journey)
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
                model.EstimatedDepartureTimeFormatted = trainService.etd.ToString("HH:mm:ss");
                model.EstimatedDepartureTime = trainService.etd;
                model.ScheduledDepartureTimeFormatted = trainService.std.ToString("HH:mm:ss");
                model.ScheduledDepartureTime = trainService.std;
                model.DelayMinutes = (int)Math.Floor((trainService.etd - trainService.std).TotalMinutes);
                result.Add(model);
            }

            return result;
        }
    }
}