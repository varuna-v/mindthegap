using System.Collections.Generic;
using System.Linq;
using MindTheGap.Models;

namespace MindTheGap.Repositories
{
    public class GameHistoryRepository
    {
        private static List<GameHistory> _gameHistory;
        public void AddGameHistory(GameHistory history)
        {
            if (_gameHistory == null)
                _gameHistory = new List<GameHistory>();

            var existingGameHistory = _gameHistory.FirstOrDefault(g =>
                g.User.UserName == history.User.UserName && g.Train.RId == history.Train.RId);
            if (existingGameHistory == null)
                _gameHistory.Add(history);
            else
            {
                existingGameHistory.QuestionAnswerResponseModel.Add(history.QuestionAnswerResponseModel[0]);
                existingGameHistory.EndTime = history.EndTime;
            }
        }

        public GameHistory GetGameHistory(User user, TrainModel train)
        {
            var history = _gameHistory.FirstOrDefault(g =>
                g.User.UserName == user.UserName && g.Train.RId == train.RId);
            return history;
        }

        public List<GameHistory> GetGameHistory(string trainRId)
        {
            var history = _gameHistory.Where(g => g.Train.RId == trainRId).OrderByDescending(g => g.CorrectPercentage).ToList();
            return history;
        }
    }
}