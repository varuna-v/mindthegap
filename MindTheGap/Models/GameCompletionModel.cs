using System;

namespace MindTheGap.Models
{
    public class GameCompletionModel
    {
        public int NumberOfQuestions { get; set; }
        public int NumberAnsweredCorrectly { get; set; }
        public string TotalDuration { get; set; }
    }
}