using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.UI.Components;
using Feofun.UI.Message;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Feofun.UI.Dialog
{
    public class DialogManager : MonoBehaviour
    {
        private readonly List<BaseDialog> _activeDialogs = new();
        
        private Dictionary<Type, BaseDialog> _dialogs;

        [Inject]
        private IMessenger _messenger;
        
        private void Awake()
        {
            _dialogs = GetComponentsInChildren<BaseDialog>(true)
                .ToDictionary(it => it.GetType(), it => it);
            DeActivateAll();
        }

        private void DeActivateAll()
        {
            _dialogs.Values.ForEach(it => it.SetActive(false));
        }
        
        public void Show(string dialogName)
        {
            var dialogType = _dialogs.Keys.FirstOrDefault(it => it.Name == dialogName);
            if (dialogType == null) {
                throw new ArgumentException($"Trying to get to non-existing dialog {dialogName}");
            }
            ShowInternal(dialogType);
            _messenger.Publish(new DialogOpenedMessage(dialogType));

        }
        public void Show<TDialog>() where TDialog : BaseDialog
        {
            ShowInternal(typeof(TDialog));
            _messenger.Publish(new DialogOpenedMessage(typeof(TDialog)));
        }
        
        public void Show<TDialog, TParam>(TParam initParam)
                where TDialog : BaseDialog, IUiInitializable<TParam>
        {
            var dialog = GetDialog(typeof(TDialog));
            ShowInternal(typeof(TDialog));
            var uiInitializable = (TDialog) dialog;
            uiInitializable.Init(initParam);
            _messenger.Publish(new DialogOpenedMessage(typeof(TDialog)));
        }
        public void Hide<TDialog>() where TDialog : BaseDialog => Hide(typeof(TDialog));

        public void Hide(Type dialogType) 
        {
            var dialog = GetDialog(dialogType);
            if (!_activeDialogs.Contains(dialog)) {
                return;
            }
            dialog.SetActive(false);
            _activeDialogs.Remove(dialog);
        }
        
        public bool IsDialogActive<TDialog>()
                where TDialog : BaseDialog
        {
            return _activeDialogs.Contains(GetDialog(typeof(TDialog)));
        }
        public void HideAll()
        {
            _activeDialogs.ForEach(it => it.SetActive(false));
            _activeDialogs.Clear();
        }
        
        private void ShowInternal(Type dialogType)
        {
            var dialog = GetDialog(dialogType);
            if (_activeDialogs.Contains(dialog)) {
                Hide(dialogType);
            }
            AddActiveDialog(dialog);
            dialog.SetActive(true);
        }

        private void AddActiveDialog(BaseDialog dialog)
        {
            _activeDialogs.Add(dialog);
            Sort();
        }
        private void Sort()
        {
            _activeDialogs.ForEach(it => { it.GetComponent<RectTransform>().SetSiblingIndex(_activeDialogs.IndexOf(it)); });
        }

        private BaseDialog GetDialog(Type dialogType)
        {
            if (!_dialogs.ContainsKey(dialogType)) {
                throw new ArgumentException($"Trying to get to non-existing dialog {dialogType.Name}");
            }
            return _dialogs[dialogType];
        }
    }
}