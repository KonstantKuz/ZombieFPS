namespace App.Unit.Message
{
    public readonly struct UnitInitMessage
    {
        public readonly Unit Unit;

        public UnitInitMessage(Unit unit)
        {
            Unit = unit;
        }
    }
}