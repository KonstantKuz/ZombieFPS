using System;
using UnityEngine;

namespace Feofun.Vfx
{
    public class PseudoAttach: MonoBehaviour
    {
        private Transform _parent;
        
        public void Attach(Transform parent)
        {
            _parent = parent;
        }

        private void Update()
        {
            if (_parent == null)
            {
                return;
            }

            transform.SetPositionAndRotation(_parent.position, _parent.rotation);
        }

        private void OnDisable()
        {
            _parent = null;
        }
    }
}