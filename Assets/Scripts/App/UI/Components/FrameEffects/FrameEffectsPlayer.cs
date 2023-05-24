using System.Collections.Generic;
using System.Linq;
using App.UI.Components.FrameEffects.Data;
using App.UI.Components.FrameEffects.Effects;
using Feofun.UI.Loader;
using Feofun.Util.SerializableDictionary;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.UI.Components.FrameEffects
{
    public class FrameEffectsPlayer : MonoBehaviour
    {
        private readonly Dictionary<FrameEffectType, FrameEffect> _currentEffects = new();
        
        [SerializeField]
        private SerializableDictionary<FrameEffectType, FrameEffect> _effectPrefabs;

        public void Play<TModel>(FrameEffectType type, TModel initModel)
        {
            CheckEffect(type);
            var model = InitializableUIModel<InitializableFrameEffect<TModel>, TModel>
                .Create(initModel)
                .SetPrefab((InitializableFrameEffect<TModel>)_effectPrefabs[type]);
            var effect = model.UIPrefab;
            effect.Init(model.InitParameter);
            effect.gameObject.SetActive(true);
            StartEffect(type, effect);
        }

        public bool IsPlaying(FrameEffectType type) => _currentEffects.ContainsKey(type);
        
        public void Play(FrameEffectType type)
        { 
            CheckEffect(type);
            var model = new UIModel<FrameEffect>()
                .SetPrefab(_effectPrefabs[type]);
            var effect = model.UIPrefab;
            effect.gameObject.SetActive(true);
            StartEffect(type, effect);
        }
        
        public void Stop(FrameEffectType type)
        {
            Assert.IsTrue(_effectPrefabs.ContainsKey(type), $"Effect not found, type:= {type}");

            if (!_currentEffects.ContainsKey(type)) {
                return;
            }
            var currentEffect = _currentEffects[type];
            currentEffect.StopEffect();
            _currentEffects.Remove(type);
        }

        public void StopAll() => _currentEffects.Keys.ToList().ForEach(Stop);

    
        private void CheckEffect(FrameEffectType type)
        {
            Assert.IsTrue(_effectPrefabs.ContainsKey(type), $"Effect not found, type:= {type}");
            if (_currentEffects.ContainsKey(type)) {
                Stop(type);
            }
        }
        
        private void StartEffect(FrameEffectType type, FrameEffect effect)
        {
            _currentEffects[type] = effect;
            effect.StartEffect(()=> Stop(type));
        } 
        
        private void OnDisable() => StopAll();
    }


}