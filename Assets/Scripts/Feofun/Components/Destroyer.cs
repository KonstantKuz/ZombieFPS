using System;
using EasyButtons;
using Feofun.World.Factory.ObjectFactory;
using UnityEngine;
using Zenject;

namespace Feofun.Components
{
    public enum DestroyType
    {
        Destroy,
        Pool,
        Inactive,
    }
    public class Destroyer : MonoBehaviour
    {
        [SerializeField]
        private DestroyType _destroyType = DestroyType.Destroy;

        [Inject(Id = ObjectFactoryType.Pool)]
        private IObjectFactory _factory;

        public DestroyType DestroyType
        {
            get => _destroyType;
            set => _destroyType = value;
        }
        
        [Button]
        public void Destroy()
        {
            switch (DestroyType)
            {
                case DestroyType.Destroy:
                    Destroy(gameObject);
                    return;
                case DestroyType.Pool:
                    _factory.Destroy(gameObject);
                    return;
                case DestroyType.Inactive:
                    gameObject.SetActive(false);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(DestroyType), DestroyType, null);
            } 
        }
        
    }
}