using App.Unit.Component.Animation;
using Feofun.Util.SerializableDictionary;
using UnityEngine;

namespace App.Enemy.Component
{
    public class EnemySound : MonoBehaviour
    {
        [SerializeField] private AudioSource _moveSource;
        [SerializeField] private SerializableDictionary<AnimationClip, AudioClip> _moveSoundMap;

        private AnimationOverrideController _animationSwitcher;

        private void OnEnable()
        {
            _animationSwitcher = gameObject.GetComponentInChildren<AnimationOverrideController>();
            if (_animationSwitcher != null)
            {
                _animationSwitcher.OnAnimationOverrided += ChangeMoveSound;
            }
        }

        private void ChangeMoveSound(AnimationClip newAnimation)
        {
            if(!_moveSoundMap.ContainsKey(newAnimation)) return;

            _moveSource.clip = _moveSoundMap[newAnimation];
        }

        public void SetWalkSound(bool enabled)
        {
            if (!this.enabled || _moveSource.isPlaying == enabled) return;
            if (enabled)
            {
                _moveSource.Play();
            }
            else
            {
                _moveSource.Stop();
            }
        }

        private void OnDisable()
        {
            if (_animationSwitcher != null)
            {
                _animationSwitcher.OnAnimationOverrided -= ChangeMoveSound;
            }
        }
    }
}