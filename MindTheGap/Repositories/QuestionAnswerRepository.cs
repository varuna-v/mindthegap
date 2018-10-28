using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using MindTheGap.Models;
using Newtonsoft.Json;

namespace MindTheGap.Repositories
{
    public class QuestionAnswerRepository
    {
        private static readonly Random Rnd = new Random();
        public QuestionAnswerModel GetRandomQuestionAnswer()
        {
            var question = GetQuestion();
            var result = new QuestionAnswerModel()
            {
                Question = HttpUtility.HtmlDecode(question.question),
                Answers = new List<AnswerModel>()
            };
            var correctAnswerIndex = Rnd.Next(4);
            var incorrectAnswerCount = 0;
            for (int i = 0; i < 4; i++)
            {
                AnswerModel answer;
                if (i == correctAnswerIndex)
                {
                    answer = new AnswerModel() { Answer = HttpUtility.HtmlDecode(question.correct_answer), IsCorrect = true };
                }
                        
                else
                {
                    answer = new AnswerModel()
                    {
                        Answer = HttpUtility.HtmlDecode(question.incorrect_answers[incorrectAnswerCount]),
                        IsCorrect = false
                    };
                    incorrectAnswerCount++;
                }
                result.Answers.Add(answer);
            }

            return result;
        }
        public QuestionAnswerModel GetFakeRandomQuestionAnswer()
        {
            var a = DateTime.Now.Millisecond;
            return new QuestionAnswerModel()
            {
                Question = $"Question{a}",
                Answers = new List<AnswerModel>()
                {
                    new AnswerModel() {Answer = $"Answer{a} - 1"},
                    new AnswerModel() {Answer = $"Answer{a} - 2", IsCorrect = true},
                    new AnswerModel() {Answer = $"Answer{a} - 3"},
                    new AnswerModel() {Answer = $"Answer{a} - 4"}
                }
            };
        }

        private Result GetQuestion()
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = client.GetAsync("https://opentdb.com/api.php?amount=10&difficulty=medium&type=multiple").Result)
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string result = content.ReadAsStringAsync().Result;
                var question = JsonConvert.DeserializeObject<TriviaQuestionResult>(result);
                return question.results[0];
            }
        }
    }
    public class Result
    {
        public string category { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
    }

    public class TriviaQuestionResult
    {
        public int response_code { get; set; }
        public List<Result> results { get; set; }
    }
}