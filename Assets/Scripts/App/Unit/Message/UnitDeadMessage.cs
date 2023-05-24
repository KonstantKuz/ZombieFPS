namespace App.Unit.Message
{
    public struct UnitDeadMessage
    {
        public readonly Unit Unit;

        public UnitDeadMessage(Unit unit)
        {
            Unit = unit;
        }
    }
}