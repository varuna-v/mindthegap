using System.Collections.Generic;
using MindTheGap.Models;

namespace MindTheGap.Repositories
{
    public class TrainStationRepository
    {
        public List<TrainStation> GetTrainStations()
        {
            return new List<TrainStation>()
            {
                new TrainStation() {StationName = "Manchester Airport", Code = "MIA"},
                new TrainStation() {StationName = "Manchester Oxford Road", Code = "MCO"},
                new TrainStation() {StationName = "Manchester Piccadilly", Code = "MAN"},
                new TrainStation() {StationName = "Manchester United Football Ground", Code = "MUF"},
                new TrainStation() {StationName = "Manchester Victoria", Code = "MCV"},
                new TrainStation() {StationName = "Bolton", Code = "BON"},
                new TrainStation() {StationName = "Leeds", Code = "LDS"},
                new TrainStation() {StationName = "London Euston", Code = "EUS"},
                new TrainStation() {StationName = "Newcastle", Code = "NCL"},
                new TrainStation() {StationName = "Liverpool Lime Street", Code = "LIV"},
                new TrainStation() {StationName = "Wigan North Western", Code = "WGN"},
                new TrainStation() {StationName = "York", Code = "YRK"},
                new TrainStation() {StationName = "Birmingham New Street", Code = "BHM"},
                new TrainStation() {StationName = "Birmingham International", Code = "BHI"},
                new TrainStation() {StationName = "Birmingham Snow Hill", Code = "BSW"},
                new TrainStation() {StationName = "Bristol Parkway", Code = "BPW"},
                new TrainStation() {StationName = "Bristol Temple Meads", Code = "BRI"},
                new TrainStation() {StationName = "Edinburgh", Code = "EDB"},
                new TrainStation() {StationName = "Glasgow Central", Code = "GLC"},
                new TrainStation() {StationName = "Scarborough", Code = "SCA"}
            };
        }
    }
}
 