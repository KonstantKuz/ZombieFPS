using System;
using UnityEngine;

namespace App.Player.Component.Input.MovementJoystick
{
    public interface IMovementJoystick
    {
        Vector3 MoveDirection { get; }
        event Action<bool> OnUpdateRunningState;
    }
}