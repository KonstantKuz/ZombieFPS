using Feofun.Components;
using UnityEngine;

namespace Feofun.Sound
{
    [RequireComponent(typeof(AudioSource))]  
    [RequireComponent(typeof(Destroyer))]
    public class AutoDestroyAudioSource : MonoBehaviour
    {
        private bool _hasStartedPlaying;
        private AudioSource _audioSource;
        private Destroyer _destroyer;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _destroyer = GetComponent<Destroyer>();
            _hasStartedPlaying = _audioSource.isPlaying;
        }

        private void Update()
        {
            if (_hasStartedPlaying && !_audioSource.isPlaying)
            {
                _destroyer.Destroy();
                return;
            }

            _hasStartedPlaying |= _audioSource.isPlaying;
        }
    }
}