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
        public string ScheduledDepartureTimeFormatted => ScheduledDepartureTime.ToString("HH:mm");
        public DateTime ScheduledDepartureTime { get; set; }
        public string EstimatedDepartureTimeFormatted => EstimatedDepartureTime.ToString("HH:mm");
        public DateTime EstimatedDepartureTime { get; set; }
        public int DelayMinutes => (int) Math.Floor((EstimatedDepartureTime - ScheduledDepartureTime).TotalMinutes);
        public string RId { get; set; }
    }
}