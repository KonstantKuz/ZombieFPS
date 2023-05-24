namespace App.Player.Component.Input.MovementJoystick
{
    public static class MovementJoystickExtension
    {
        public static bool IsMoving(this IMovementJoystick movementJoystick)
        {
            return movementJoystick.MoveDirection.sqrMagnitude > 0;
        }
    }
}