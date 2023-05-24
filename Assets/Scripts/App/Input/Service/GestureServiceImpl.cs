using App.Input.Component;
using App.UI.Screen.World.Player.RuntimeInventory.View;
using DigitalRubyShared;

namespace App.Input.Service
{
    public class GestureServiceImpl
    {
        private const int MAX_NUMBER_OF_TOUCHES_TO_TRACK = 2;
        private const float MIN_HOLD_DURATION = 0.1f;
        
        private PanGestureRecognizer _pan;
        private PanGestureRecognizer Pan => _pan ??= CreatePan();
        private TapGestureRecognizer _tap;
        private TapGestureRecognizer Tap => _tap ??= CreateTap();
        private LongPressGestureRecognizer _hold;
        private LongPressGestureRecognizer Hold => _hold ??= CreateHold();

        public GestureServiceImpl()
        {
            FingersScript.Instance.ComponentTypesToDenyPassThrough.Add(typeof(ButtonWithHoldTime)); 
            FingersScript.Instance.ComponentTypesToDenyPassThrough.Add(typeof(GestureDenyingComponent));
        }

        private PanGestureRecognizer CreatePan()
        {
            var pan = new PanGestureRecognizer();
            pan.MaximumNumberOfTouchesToTrack = MAX_NUMBER_OF_TOUCHES_TO_TRACK;
            FingersScript.Instance.AddGesture(pan);
            return pan;
        }
        
        private TapGestureRecognizer CreateTap()
        {
            var tapGesture = new TapGestureRecognizer();
            FingersScript.Instance.AddGesture(tapGesture);
            return tapGesture;
        }

        private LongPressGestureRecognizer CreateHold()
        {
            var holdGesture = new LongPressGestureRecognizer();
            holdGesture.MinimumDurationSeconds = MIN_HOLD_DURATION;
            FingersScript.Instance.AddGesture(holdGesture);
            return holdGesture;
        }
        
        public void SubscribeOnPan(GestureRecognizerStateUpdatedDelegate panDelegate) => Pan.StateUpdated += panDelegate;
        public void UnSubscribeOnPan(GestureRecognizerStateUpdatedDelegate panDelegate) => Pan.StateUpdated -= panDelegate;

        public void SubscribeOnTap(GestureRecognizerStateUpdatedDelegate tapDelegate) => Tap.StateUpdated += tapDelegate;
        public void UnsubscribeOnTap(GestureRecognizerStateUpdatedDelegate tapDelegate) => Tap.StateUpdated -= tapDelegate;
        
        public void SubscribeOnHold(GestureRecognizerStateUpdatedDelegate tapDelegate) => Hold.StateUpdated += tapDelegate;
        public void UnsubscribeOnHold(GestureRecognizerStateUpdatedDelegate tapDelegate) => Hold.StateUpdated -= tapDelegate;
    }
}