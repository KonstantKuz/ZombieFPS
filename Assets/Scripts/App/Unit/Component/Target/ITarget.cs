using System;
using App.Unit.Component.Layering;
using UnityEngine;

namespace App.Unit.Component.Target
{
    public interface ITarget
    {
        bool IsValid { get; }
        LayerMask Layer { get; }
        Transform Root { get; }
        Transform Center { get; }
        event Action OnTargetInvalid;
    }
}