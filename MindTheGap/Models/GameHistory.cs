using System;
using System.Collections.Generic;
using System.Linq;

namespace MindTheGap.Models
{
    public class GameHistory
    {
        public User User { get; set; }
        public TrainModel Train { get; set; }
        public List<QuestionAnswerResponseModel> QuestionAnswerResponseModel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int NumberAnsweredCorrectly => QuestionAnswerResponseModel.Count(m => m.CorrectlyAnswered);
        public int NumberOfQuestions => QuestionAnswerResponseModel.Count;

        public string CorrectPercentage => ((NumberAnsweredCorrectly / (decimal) NumberOfQuestions) * 100).ToString("0.##");
    }
}