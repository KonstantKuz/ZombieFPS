using Dreamteck;
using UnityEngine;

namespace App.Unit.Component
{
    public class TimeStoppableComponents : MonoBehaviour, ITimeStoppable
    {
        [SerializeField] private Behaviour[] _components;

        public void StopTime()
        {
            _components.ForEach(it => it.enabled = false);
        }

        public void StartTime()
        {
            _components.ForEach(it => it.enabled = true);
        }
    }
}
