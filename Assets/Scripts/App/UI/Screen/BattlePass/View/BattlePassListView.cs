using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.BattlePass.Model;
using App.UI.Screen.BattlePass.Model;
using Feofun.Extension;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Screen.BattlePass.View
{
    public class BattlePassListView : MonoBehaviour
    {
        [SerializeField] private BattlePassLevelView _levelViewPrefab;
        [SerializeField] private Transform _placementRoot;
        
        [SerializeField] private ScrollRect _scrollRect;

        private CompositeDisposable _disposable;
        private RectTransform _focusedItem;

        public void Init(BattlePassListModel model)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            RemoveAllCreatedObjects();
            CreateLevelListView(model.Levels);
            if (model.IsNewRewardTaken)
            {
                AnimateLastTaken(model.Levels);
            }
        }

        private void CreateLevelListView(IReadOnlyCollection<IReactiveProperty<BattlePassLevelModel>> levels)
        {
            var sortedLevels = levels.OrderBy(it => it.Value.Level).ToList();

            foreach (var level in sortedLevels)
            {
                var levelView = Instantiate(_levelViewPrefab, _placementRoot);
                level.Subscribe(it => levelView.Init(it)).AddTo(_disposable);

                if (IsItemTaken(level.Value))
                {
                    _focusedItem = levelView.GetComponent<RectTransform>();
                }
            }
            
            StartCoroutine(ScrollToItemAfterLayoutUpdate());
        }
        
        private IEnumerator ScrollToItemAfterLayoutUpdate()
        {
            if(_focusedItem == null) yield break;
            yield return null;
            _scrollRect.FocusOnChild(_focusedItem);
        }

        private void AnimateLastTaken(IReadOnlyCollection<IReactiveProperty<BattlePassLevelModel>> levels)
        {
            var lastTaken = levels.LastOrDefault(it => IsItemTaken(it.Value));
            lastTaken?.Value.ReceivingAnimationCallback?.Invoke();
        }

        private bool IsItemTaken(BattlePassLevelModel level)
        {
            return level.State == BattlePassRewardState.Taken;
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
            RemoveAllCreatedObjects();
        }

        private void RemoveAllCreatedObjects()
        {
            _placementRoot.DestroyAllChildren();
        }
    }
}
