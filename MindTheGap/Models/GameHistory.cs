using System;
using System.Collections.Generic;

namespace MindTheGap.Models
{
    public class GameHistory
    {
        public User User { get; set; }
        public TrainModel Train { get; set; }
        public List<QuestionAnswerResponseModel> QuestionAnswerResponseModel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}