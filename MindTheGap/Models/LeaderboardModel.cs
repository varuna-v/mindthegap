using System;
using System.Collections.Generic;

namespace MindTheGap.Models
{
    public class LeaderboardModel
    {
        public List<LeaderModel> Leaders { get; set; }
        public DateTime ComputedAt { get; set; }
    }

    public class LeaderModel
    {
        public string UserName { get; set; }
        public int CorrectlyAnsweredCount { get; set; }
        public int TotalNumberOfQuestionsAnswered { get; set; }
        public bool IsCurrentUser { get; set; }
        public string CorrectPercentage { get; set; }
        public int Rank { get; set; }
    }

    public class LeaderboardByTrainModel : LeaderboardModel
    {
        public TrainModel Train { get; set; }
    }
}