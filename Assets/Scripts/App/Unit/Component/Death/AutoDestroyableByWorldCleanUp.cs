using Feofun.World;
using UnityEngine;

namespace App.Unit.Component.Death
{
    public class AutoDestroyableByWorldCleanUp : MonoBehaviour, IWorldScope
    {
        public void OnWorldSetup() { }

        public void OnWorldCleanUp()
        {
           Destroy(gameObject);
        }


    }
}