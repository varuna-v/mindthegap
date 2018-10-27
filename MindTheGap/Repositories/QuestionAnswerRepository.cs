using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MindTheGap.Models;

namespace MindTheGap.Repositories
{
    public class QuestionAnswerRepository
    {
        public QuestionAnswerModel GetRandomQuestionAnswer()
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
    }
}