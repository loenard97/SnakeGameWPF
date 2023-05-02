using System.Collections.Generic;

namespace SnakeGameWPF.GameLogic
{
    public class Score
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ScoreDataModel
    {
        public List<Score> DataRows { get; set; }
    }
}
