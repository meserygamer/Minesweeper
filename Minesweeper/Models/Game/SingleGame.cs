namespace Minesweeper.Models.Game
{
    public class SingleGame
    {
        public SingleGame(int fieldHeight, int fieldWeight, int fieldMinesCount)
        {
            GameField = new Field(fieldHeight, fieldWeight, fieldMinesCount);
            FieldVision = new FieldVision(GameField);
            GameId = Guid.NewGuid();
        }


        public Guid GameId { get; }

        public bool IsGameComplete { get; private set; } = false;

        public Field GameField { get; }

        public FieldVision FieldVision { get; }

        public string[][] PlayersVisionOnField => FieldVision.Vision;


        public void MakeTurn(FieldCoordinates coordinatesOfOppeningCell)
        {
            if (GameField.Mines is null)
                GameField.GenerateMines(coordinatesOfOppeningCell);

            if(CheckGameOnDefeatCondition(coordinatesOfOppeningCell))
                return;

            FieldVision.OpenSingleCell(coordinatesOfOppeningCell);

            CheckGameOnWinCondition();
        }

        private bool CheckMineOnPlayerCell(FieldCoordinates coordinatesOfOppeningCell) => GameField.Mines.Contains(coordinatesOfOppeningCell);

        private void CheckGameOnWinCondition()
        {
            if (FieldVision.UnopenedCellsCount <= GameField.MinesCount)
            {
                IsGameComplete = true;
                FieldVision.VisualizeVictory();
            }
        }
        private bool CheckGameOnDefeatCondition(FieldCoordinates coordinatesOfOppeningCell)
        {
            if (CheckMineOnPlayerCell(coordinatesOfOppeningCell))
            {
                IsGameComplete = true;
                FieldVision.VisualizeDefeat();
                return true;
            }
            return false;
        }
    }
}
