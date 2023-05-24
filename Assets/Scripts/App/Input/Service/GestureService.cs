using System;
using App.Input.Data;
using DigitalRubyShared;
using Feofun.World;
using Logger.Extension;

namespace App.Input.Service
{
    public class GestureService : IWorldScope, IDisposable
    {
        private const bool LOG_ENABLED = false;
        
        private readonly GestureServiceImpl _gestureServiceImpl;
        public event Action OnTap;
        public event Action OnHold;
        public event Action OnHoldRelease;
        public event Action<Pan> OnPan;

        public GestureService() => _gestureServiceImpl = new GestureServiceImpl();

        public void Init()
        {
            Dispose();
            _gestureServiceImpl.SubscribeOnTap(OnTapGestureCallback);
            _gestureServiceImpl.SubscribeOnPan(OnHoldGestureCallback);
            _gestureServiceImpl.SubscribeOnHold(OnHoldGestureCallback);
            _gestureServiceImpl.SubscribeOnPan(PanGestureCallback);
        }

        public void OnWorldSetup() => Init();

        public void OnWorldCleanUp() => Dispose();

        private void PanGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.CurrentTrackedTouches.Count == 0) return;
            LogGesture(gesture);

            if (gesture.State == GestureRecognizerState.Ended || gesture.State == GestureRecognizerState.Failed) {
                return;
            }
            var pan = new Pan(DeviceInfo.PixelsToUnits(gesture.DeltaX), DeviceInfo.PixelsToUnits(gesture.DeltaY));
            OnPan?.Invoke(pan);
        }
        
        private void LogGesture(GestureRecognizer gesture)
        {
            if (!LOG_ENABLED) return;
            var touch = gesture.CurrentTrackedTouches[0];
            Log("Pan gesture, state: {0}, position: {1},{2} -> {3},{4}, delta: {5},{6}, units: {7},{8}", 
                gesture.State, touch.PreviousX,
                touch.PreviousY, touch.X, touch.Y, gesture.DeltaX, gesture.DeltaY, 
                DeviceInfo.PixelsToUnits(gesture.DeltaX), DeviceInfo.PixelsToUnits(gesture.DeltaY));
        }
        
        private void Log(string text, params object[] args) => this.Logger().Trace(string.Format(text, args));

        private void OnTapGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                OnTap?.Invoke();
            }
        }

        private void OnHoldGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Executing)
            {
                OnHold?.Invoke();
            }
            if (gesture.State == GestureRecognizerState.Ended)
            {
                OnHoldRelease?.Invoke();
            }
        }

        public void Dispose()
        {
            _gestureServiceImpl.UnsubscribeOnTap(OnTapGestureCallback);
            _gestureServiceImpl.UnSubscribeOnPan(OnHoldGestureCallback);
            _gestureServiceImpl.UnsubscribeOnHold(OnHoldGestureCallback);
            _gestureServiceImpl.UnSubscribeOnPan(PanGestureCallback);
        }

     
    }
}