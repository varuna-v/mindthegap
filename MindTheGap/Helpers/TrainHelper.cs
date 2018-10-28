using System;
using System.Collections.Generic;
using System.Linq;
using MindTheGap.LDBStaffWebService;
using MindTheGap.Models;

namespace MindTheGap.Helpers
{
    public class TrainHelper
    {
        private Random rand = new Random();
        private static KeyValuePair<DateTime, List<TrainModel>> TrainCache { get; set; }
        private static Object synclock = new Object();
        public List<TrainModel> GetTrains(JourneySelectionModel journey)
        {
            var time = DateTime.Now;

            if (!TrainCache.Equals(default(KeyValuePair<DateTime, List<TrainModel>>)) && TrainCache.Key > time &&
                TrainCache.Value != null)
                return TrainCache.Value;


            lock (synclock)
            {
                if (!TrainCache.Equals(default(KeyValuePair<DateTime, List<TrainModel>>)) && TrainCache.Key > time &&
                    TrainCache.Value != null)
                    return TrainCache.Value;

                var result = new List<TrainModel>();
                for (int i = 0; i < 5; i++)
                {
                    var delay = rand.Next(60, 180);
                    var model = new TrainModel()
                    {
                        FromStation = journey.FromStationCode,
                        ToStation = journey.ToStationCode,
                        ScheduledDepartureTime = time,
                        EstimatedDepartureTime = time.AddSeconds(delay),
                        RId = Guid.NewGuid().ToString()
                    };
                    result.Add(model);
                    time = time.AddMinutes(2);
                }
                TrainCache = new KeyValuePair<DateTime, List<TrainModel>>(time.AddMinutes(15), result);
                return result;

            }


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