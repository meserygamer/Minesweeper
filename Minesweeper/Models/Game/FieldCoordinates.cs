namespace Minesweeper.Models.Game
{
    public struct FieldCoordinates
    {
        public int XColumn { get; set; }

        public int YRow { get; set; }


        public static bool operator ==(FieldCoordinates left, FieldCoordinates right) => 
            left.XColumn == right.XColumn && left.YRow == right.YRow;

        public static bool operator !=(FieldCoordinates left, FieldCoordinates right) => 
            left.XColumn != right.XColumn || left.YRow != right.YRow;

        public override bool Equals(object? obj)
        {
            if(obj is not FieldCoordinates fieldCoordinatesRight)
                return false;

            return XColumn == fieldCoordinatesRight.XColumn && YRow == fieldCoordinatesRight.YRow;
        }
    }
}
