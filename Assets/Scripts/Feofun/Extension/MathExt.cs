﻿
using UnityEngine;

namespace Feofun.Extension
{
    public static class MathExt
    {
        public static bool IsZero(this float value) => Mathf.Abs(value) < Mathf.Epsilon;

    }
}