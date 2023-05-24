using System;
using UnityEngine;

namespace App.Player.Component.Input.InputStick
{
    public interface IInputStick : IDisposable
    {
        event Action<Vector2> OnInput;
    }
}