namespace App.Input.Data
{
    public struct Pan
    {
        public readonly float DeltaX;
        public readonly float DeltaY;
        public Pan(float deltaX, float deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }
    }
}