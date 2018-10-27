using System;
using System.Collections.Generic;

namespace MindTheGap.Models
{
    public class SelectTrainModel
    {
        public List<TrainModel> Trains { get; set; }
        public string SelectedRId { get; set; }
    }
    public class TrainModel
    {
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string ScheduledDepartureTimeFormatted { get; set; }
        public DateTime ScheduledDepartureTime { get; set; }
        public string EstimatedDepartureTimeFormatted { get; set; }
        public DateTime EstimatedDepartureTime { get; set; }
        public int DelayMinutes { get; set; }
        public string RId { get; set; }
    }
}