using App.Unit.Component.Attack.Timer;
using DG.Tweening;
using Feofun.Util.Timer;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Screen.World.Player.RuntimeInventory.View
{
    public class WeaponReloadingView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _reloadOverlay;
        [SerializeField]
        private Image _reloadBar;

        private CompositeDisposable _disposable;
        
        private Tween _reloadAnimation;

        public void Init(IReactiveProperty<ITimer> timer)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            timer.Subscribe(PlayReloadAnimation).AddTo(_disposable);
        }
        

        private void PlayReloadAnimation([CanBeNull] ITimer timer)
        {
            _reloadOverlay.SetActive(timer != null);
            
            if(timer == null) return;
            
            _reloadAnimation?.Kill();
            _reloadBar.fillAmount = timer.Progress;
            _reloadAnimation = DOTween.To(() => _reloadBar.fillAmount, value => { _reloadBar.fillAmount = value; },
                1, timer.TimeLeft).SetEase(Ease.Linear);
        }

        private void OnDestroy() => Dispose();

        private void Dispose()
        {
            _reloadAnimation?.Kill();
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}