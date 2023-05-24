using System.Linq;
using Feofun.ObjectPool.Component;
using Feofun.World.Service;
using SuperMaxim.Core.Extensions;
using Zenject;

namespace Feofun.ObjectPool.Service
{
    public class PoolPreparer
    {
        [Inject] private PoolManager _poolManager;
        [Inject] private ObjectResourceService _objectResourceService;
        
        public void Prepare()
        {
            _objectResourceService.GetAllPrefabs()
                .Select(it => it.GetComponent<ObjectPoolParamsComponent>())
                .Where(it => it != null && it.PreparePoolOnInitScene)
                .ForEach(it => _poolManager.Prepare(it.PoolId, it.gameObject, it.GetPoolParams()));
        
        }
    }
}