namespace Minesweeper.Models.Game
{
    public class GameRepository
    {
        public Dictionary<Guid, SingleGame> Games = new Dictionary<Guid, SingleGame>();


        public SingleGame CreateSingleGame(int fieldHeight, int fieldWeight, int fieldMinesCount)
        {
            SingleGame game = new SingleGame(fieldHeight, fieldWeight, fieldMinesCount);
            Games.Add(game.GameId, game);
            return game;
        }
    }
}
