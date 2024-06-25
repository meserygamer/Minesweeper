namespace Minesweeper.Models.Game
{
    public class Field
    {
        public Field(int height, int weight, int minesCount)
        {
            Height = height;
            Width = weight;
            MinesCount = minesCount;
        }


        public int Height { get; private set; }
        public int Width { get; private set; }
        public int MinesCount { get; private set; }

        public HashSet<FieldCoordinates> Mines { get; private set; } = null!;


        public void GenerateMines(FieldCoordinates playersStartingMove)
        {
            if(MinesCount >= Width * Height || MinesCount < 0)
                return;

            List<FieldCoordinates> possibleMineCoordinates = GeneratePossibleMineCoordinates(playersStartingMove);

            Random random = new Random();
            Mines = new HashSet<FieldCoordinates>();
            for(int i = 0; i < MinesCount; i++)
            {
                FieldCoordinates fieldCoordinates = possibleMineCoordinates[random.Next(0, possibleMineCoordinates.Count)];
                Mines.Add(fieldCoordinates);
                possibleMineCoordinates.Remove(fieldCoordinates);
            }
               
        }

        public int MinesCountAroundCell(FieldCoordinates cellCoordinates)
        {
            int minesCount = 0;
            FieldCoordinates possibleMinesCoordinates = new FieldCoordinates();

            for (int i = cellCoordinates.YRow - 1; i <= cellCoordinates.YRow + 1; i++)
            {
                for (int j = cellCoordinates.XColumn - 1; j <= cellCoordinates.XColumn + 1; j++)
                {
                    possibleMinesCoordinates.XColumn = j;
                    possibleMinesCoordinates.YRow = i;

                    if (possibleMinesCoordinates == cellCoordinates)
                        continue;

                    if (Mines.Contains(possibleMinesCoordinates))
                        minesCount++;
                }
            }

            return minesCount;
        }

        public bool IsMinedCell(FieldCoordinates cellCoordinates) => Mines.Contains(cellCoordinates);

        private List<FieldCoordinates> GeneratePossibleMineCoordinates(FieldCoordinates playersStartingMove)
        {
            List<FieldCoordinates> possibleMineCoordinates = new List<FieldCoordinates>();

            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    possibleMineCoordinates.Add(new FieldCoordinates() 
                    {
                        YRow = i,
                        XColumn = j
                    });
            possibleMineCoordinates.Remove(playersStartingMove);

            return possibleMineCoordinates;
        }
    }
}
