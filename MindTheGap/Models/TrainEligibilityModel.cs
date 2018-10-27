using System;
using System.Collections.Generic;

namespace MindTheGap.Models
{
    public class TrainEligibilityModel
    {
        public bool Eligible { get; set; }
        public DateTime EligibilityEndTime { get; set; }
        public TrainModel Train { get; set; }
    }

    public class GamePlayModel
    {
        public QuestionAnswerModel QuestionAnswer { get; set; }
        public TrainEligibilityModel TrainEligibilityModel { get; set; }
        public string SelectedAnswer { get; set; }
        public DateTime StartedAt { get; set; }
    }

    public class QuestionAnswerModel
    {
        public string Question { get; set; }
        public List<AnswerModel> Answers { get; set; }
    }

    public class QuestionAnswerResponseModel : QuestionAnswerModel
    {
        public User User { get; set; }
        public bool CorrectlyAnswered { get; set; }
    }

    public class AnswerModel
    {
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}