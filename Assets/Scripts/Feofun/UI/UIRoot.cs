using Feofun.World;
using UnityEngine;

namespace Feofun.UI
{
    public class UIRoot : RootContainer
    {
        private Canvas _canvas;

        public Canvas Canvas => _canvas ??= GetComponent<Canvas>();
    }
}