using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Data;
using App.UI.Dialogs.Character.Model.Slots;
using App.UI.Dialogs.Character.View.Inventory;
using UniRx;
using UnityEngine;

namespace App.UI.Dialogs.Character.View.Slots
{
    public class SlotsView : MonoBehaviour
    {
        private Dictionary<SlotId, SlotView> _slotViews;
        private CompositeDisposable _disposable;
        
        private Dictionary<SlotId, SlotView> SlotViews =>
            _slotViews ??= GetComponentsInChildren<SlotView>().ToDictionary(slot => slot.SlotId, 
                    slot=> slot);

        public void Init(SlotsModel slotsModel)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            InitSlots(slotsModel.Slots);
        }

        public InventoryItemView GetItemView(SlotId slotId) => GetSlotView(slotId).GetItemView();
        
        public SlotView GetSlotView(SlotId slotId)
        {
            if (!SlotViews.ContainsKey(slotId)) {
                throw new NullReferenceException($"SlotView not found by SlotId:= {slotId}");
            }
            return SlotViews[slotId];
        }
        
        private void InitSlots(Dictionary<SlotId, ReactiveProperty<SlotViewModel>> slots)
        {
            foreach (var slot in slots) {
                slot.Value.Subscribe(UpdateSlotModel).AddTo(_disposable);
            }
        }
        
        private void UpdateSlotModel(SlotViewModel slotViewModel) => GetSlotView(slotViewModel.SlotId).Init(slotViewModel);

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        
    }
}