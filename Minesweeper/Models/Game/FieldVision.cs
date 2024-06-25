namespace Minesweeper.Models.Game
{
    public class FieldVision
    {
        public FieldVision(Field field) 
        {
            _field = field;
            InitializeVision();
        }


        private Field _field;


        /// <summary>
        /// Поле видимое пользователем
        /// </summary>
        public string[][] Vision { get; private set; } = null!;

        /// <summary>
        /// Количество неоткрытых клеток
        /// </summary>
        public int UnopenedCellsCount => Vision.Select((row, index) => row.Where(cell => cell == " ").Count()).Sum();


        #region Публичные методы

        public bool IsCellVisibleForUser(FieldCoordinates cellCoordinates) => Vision[cellCoordinates.YRow][cellCoordinates.XColumn] != " ";

        /// <summary>
        /// Привести поле к виду победы
        /// </summary>
        public void VisualizeDefeat() => OpenAllCell('X');

        /// <summary>
        /// Привести поле к виду поражения
        /// </summary>
        public void VisualizeVictory() => OpenAllCell('M');

        /// <summary>
        /// Открытие единичной клетки с дальнейшим распостанениям по смежным ячейкам
        /// </summary>
        /// <param name="cellCoordinates">координаты открываемой пользователем клетки</param>
        public void OpenSingleCell(FieldCoordinates cellCoordinates)
        {
            Queue<FieldCoordinates> queueCellsForOpenings = new Queue<FieldCoordinates>(){};
            queueCellsForOpenings.Enqueue(cellCoordinates);
            while (queueCellsForOpenings.TryDequeue(out FieldCoordinates cellForOpening))
                OpenCell(cellForOpening, queueCellsForOpenings);
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Первичная инициализация области видимости
        /// </summary>
        private void InitializeVision()
        {
            Vision = new string[_field.Height][];
            for (int i = 0; i < _field.Height; i++)
            {
                Vision[i] = new string[_field.Width];
                for (int j = 0; j < _field.Width; j++)
                {
                    Vision[i][j] = " ";
                }   
            } 
        }

        /// <summary>
        /// Открыть все клетки на поле, включая мины
        /// </summary>
        /// <param name="mineSubstitute">Символ заменяющий мины</param>
        private void OpenAllCell(char mineSubstitute)
        {
            FieldCoordinates fieldCoordinates = new FieldCoordinates();
            for (int i = 0; i < Vision.Length; i++)
            {
                for (int j = 0; j < Vision[i].Length; j++)
                {
                    fieldCoordinates.XColumn = j;
                    fieldCoordinates.YRow = i;

                    if (_field.Mines.Contains(fieldCoordinates))
                    {
                        Vision[i][j] = mineSubstitute.ToString();
                        continue;
                    }

                    Vision[i][j] = _field.MinesCountAroundCell(fieldCoordinates).ToString();
                }
            }
        }

        /// <summary>
        /// Открыть клетку с дальнейшим распостранением по смежным ячейкам
        /// </summary>
        /// <param name="cellCoordinates">координаты клектки</param>
        /// <param name="queueCellsForOpenings">очередь для записи смежных клеток</param>
        private void OpenCell(FieldCoordinates cellCoordinates, Queue<FieldCoordinates> queueCellsForOpenings)
        {
            if (IsCellVisibleForUser(cellCoordinates))
                return;

            int minesCountAroundCell = _field.MinesCountAroundCell(cellCoordinates);
            Vision[cellCoordinates.YRow][cellCoordinates.XColumn] = minesCountAroundCell.ToString();

            if (minesCountAroundCell > 0)
                return;
                                                                                                                     //Для определения открываемых клеток используется алгоритм "поиска в ширину"
                                                                                                                     //Модифицированный под нахождения всех путей их клетки
            FieldCoordinates[] fieldCoordinates = new FieldCoordinates[]                                                            
            {
                new FieldCoordinates() { XColumn = cellCoordinates.XColumn, YRow = cellCoordinates.YRow - 1 },
                new FieldCoordinates() { XColumn = cellCoordinates.XColumn, YRow = cellCoordinates.YRow + 1 },
                new FieldCoordinates() { XColumn = cellCoordinates.XColumn - 1, YRow = cellCoordinates.YRow },
                new FieldCoordinates() { XColumn = cellCoordinates.XColumn + 1, YRow = cellCoordinates.YRow }
            };

            for (int i = cellCoordinates.YRow - 1; i <= cellCoordinates.YRow + 1; i++)
            {
                for(int j = cellCoordinates.XColumn - 1; j <= cellCoordinates.XColumn + 1; j++)
                {
                    if (i < 0 || i >= Vision.Length ||
                    j < 0 || j >= Vision[0].Length)
                        continue;

                    if (Vision[i][j] != " ")
                        continue;

                    queueCellsForOpenings.Enqueue(new FieldCoordinates() { YRow = i, XColumn = j });
                }
            }

            foreach(var coordinates in fieldCoordinates)
            {
                if (coordinates.YRow < 0 || coordinates.YRow >= Vision.Length ||
                    coordinates.XColumn < 0 || coordinates.XColumn >= Vision[0].Length)
                    continue;

                if (Vision[coordinates.YRow][coordinates.XColumn] != " ")
                    continue;

                queueCellsForOpenings.Enqueue(coordinates);
            }
        }

        #endregion
    }
}
