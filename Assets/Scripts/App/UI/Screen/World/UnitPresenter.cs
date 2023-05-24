using App.Session.Messages;
using App.Unit.Message;
using Dreamteck;
using Feofun.UI.Components;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World
{
    public abstract class UnitPresenter : MonoBehaviour
    {
        [SerializeField]
        private GameObject _view;
        
        [Inject] private IMessenger _messenger;
        
        [CanBeNull]
        protected abstract Unit.Unit Unit { get; }
        
        protected virtual void Awake()
        {
            _messenger.Subscribe<UnitInitMessage>(OnUnitInitialized);
            _messenger.Subscribe<SessionEndMessage>(OnSessionEndMessage);
            _messenger.Subscribe<UnitDeadMessage>(OnUnitDead); 
          
        }

        private void OnUnitInitialized(UnitInitMessage msg)
        {
            if(!msg.Unit.Equals(Unit)) return;
            Init(msg.Unit);
        }

        protected virtual void Init(Unit.Unit unit)
        {
            _view.GetComponentsInChildren<IUiInitializable<Unit.Unit>>(true)
                .ForEach(it => it.Init(unit));
            SetActive(true);
        }
        
        private void OnSessionEndMessage(SessionEndMessage obj) => SetActive(false);
        
        private void OnUnitDead(UnitDeadMessage msg)
        {
            if(!msg.Unit.Equals(Unit)) return;
            SetActive(false);
        }

        private void SetActive(bool enabled) => _view.gameObject.SetActive(enabled);
    }
}