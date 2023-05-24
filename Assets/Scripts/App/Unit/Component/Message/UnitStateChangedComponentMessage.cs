namespace App.Unit.Component.Message
{
    public readonly struct UnitStateChangedComponentMessage
    {
        public readonly bool IsActive;

        public UnitStateChangedComponentMessage(bool isActive)
        {
            IsActive = isActive;
        }
    }
}