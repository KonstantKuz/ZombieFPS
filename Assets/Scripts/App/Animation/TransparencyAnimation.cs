using System.Linq;
using Feofun.Core.Update;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using Zenject;

namespace App.Animation
{
    public class TransparencyAnimation : MonoBehaviour
    {
        private const string COLOR = "_BaseColor";
        
        [SerializeField] private float _animationTime = 1f;

        [Inject] private UpdateManager _updateManager;
        
        private AnimatedTransparencyMaterialWrapper[] _materials;
        private float _playbackTime;
        
        private void Awake()
        {
            _materials = GetComponentsInChildren<Renderer>().Select(it => it.material)
                .Select(it => new AnimatedTransparencyMaterialWrapper(it, COLOR)).ToArray();
        }

        public void PlayFromTransparentToOpaque()
        {
            _materials.ForEach(it => it.SetTransparent());
            _updateManager.StartUpdate(PlayAnimation);
        }

        private void PlayAnimation()
        {
            _playbackTime += Time.deltaTime;
            var progress = _playbackTime / _animationTime;
            _materials.ForEach(it => it.TweenColors(progress));
            if(progress < 1f) return;
            _playbackTime = 0f;
            _updateManager.StopUpdate(PlayAnimation);
        }

        private void OnDisable()
        {
            if (!_playbackTime.IsZero())
            {
                _updateManager.StopUpdate(PlayAnimation);
                _materials.ForEach(it => it.Restore());
            }
            _playbackTime = 0f;
        }
    }
}