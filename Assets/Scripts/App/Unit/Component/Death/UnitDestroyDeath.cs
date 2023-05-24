using UnityEngine;

namespace App.Unit.Component.Death
{
    public class UnitDestroyDeath : MonoBehaviour, IUnitDeath
    {
        public void PlayDeath()
        {
            Destroy(gameObject);
        }
    }
}